﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenreater.API;

namespace CodeGenreater.Common
{
    public static class CSGenerator
    {
        public static void Generate(string outPutFileLocation, string connectionString, string projectName, string nameSpace, bool isApi, bool isWcf, bool isDal)
        {
            var dataBase = string.Empty;
            var sqlPath = string.Empty;
            var codePath = string.Empty;
            var tables = new List<Table>();

            using (var con = new SqlConnection(connectionString))
            {
                dataBase = Utility.FormatPascalCase(con.Database);
                sqlPath = Path.Combine(outPutFileLocation, "Script");
                codePath = Path.Combine(outPutFileLocation, "Code");
                Utility.CreateSubDirectory(sqlPath, true);
                Utility.CreateSubDirectory(codePath, true);

                con.Open();
                var table = new DataTable();
                new SqlDataAdapter(new SqlCommand(Utility.CreateTableQuery(dataBase), con)).Fill(table);

                foreach (DataRow item in table.Rows)
                {
                    var tableName = item["TABLE_NAME"].ToString();
                    var tableEntity = setTableEntity(tableName, con);
                    tables.Add(tableEntity);
                }

                // Create the necessary "use [database]" statement
                SPGenerator.CreateUseDatabaseStatement(dataBase, sqlPath);

                foreach (var item in tables)
                {
                    SPGenerator.CreateSetStoredProcedure(dataBase, item, sqlPath);
                    SPGenerator.CreateGetStoredProcedure(dataBase, item, sqlPath);
                    SPGenerator.CreateDeleteStoredProcedure(dataBase, item, sqlPath);
                }

                if (isApi)
                {
                    var apiGenerator = new ApiGenerator(codePath, tables, connectionString, projectName, nameSpace);
                    apiGenerator.Generate();
                }
                
            }
        }

        public static Table setTableEntity(string tableName, SqlConnection con)
        {
            var table = new Table();
            table.Name = tableName;
            var columns = new List<Column>();
            var tableColumn = new DataTable();
            new SqlDataAdapter(new SqlCommand(Utility.CreateColumnQuery(tableName), con)).Fill(tableColumn);
            foreach (DataRow columnRow in tableColumn.Rows)
            {
                var column = new Column();
                column.Name = columnRow["COLUMN_NAME"].ToString();
                column.Type = columnRow["DATA_TYPE"].ToString();
                column.Precision = columnRow["NUMERIC_PRECISION"].ToString();
                column.Scale = columnRow["NUMERIC_SCALE"].ToString();

                // Determine the column's length
                if (columnRow["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                {
                    column.Length = columnRow["CHARACTER_MAXIMUM_LENGTH"].ToString();
                }
                else
                {
                    column.Length = columnRow["COLUMN_LENGTH"].ToString();
                }

                // Is the column a RowGuidCol column?
                if (columnRow["IS_ROWGUIDCOL"].ToString() == "1")
                {
                    column.IsRowGuidCol = true;
                }

                // Is the column an Identity column?
                if (columnRow["IS_IDENTITY"].ToString() == "1")
                {
                    column.IsIdentity = true;
                }

                // Is columnRow column a computed column?
                if (columnRow["IS_COMPUTED"].ToString() == "1")
                {
                    column.IsComputed = true;
                }

                // Is columnRow column a nulable column?
                column.IsNullable = (columnRow["IS_NULLABLE"].ToString() == "NO") ? false : true;

                table.Columns.Add(column);
            }

            // Get the list of primary keys
            DataTable primaryKeyTable = Utility.GetPrimaryKeyList(con, table.Name);
            foreach (DataRow primaryKeyRow in primaryKeyTable.Rows)
            {
                string primaryKeyName = primaryKeyRow["COLUMN_NAME"].ToString();

                foreach (Column column in table.Columns)
                {
                    if (column.Name == primaryKeyName)
                    {
                        table.PrimaryKeys.Add(column);
                        break;
                    }
                }
            }

            // Get the list of foreign keys
            DataTable foreignKeyTable = Utility.GetForeignKeyList(con, table.Name);
            foreach (DataRow foreignKeyRow in foreignKeyTable.Rows)
            {
                string name = foreignKeyRow["FK_NAME"].ToString();
                string columnName = foreignKeyRow["FKCOLUMN_NAME"].ToString();

                if (table.ForeignKeys.ContainsKey(name) == false)
                {
                    table.ForeignKeys.Add(name, new List<Column>());
                }

                List<Column> foreignKeys = table.ForeignKeys[name];

                foreach (Column column in table.Columns)
                {
                    if (column.Name == columnName)
                    {
                        foreignKeys.Add(column);
                        break;
                    }
                }
            }

            return table;
        }

        public static void generateEntity(Table table, string entityPath, string nameSpace)
        {
            var className = Utility.FormatPascalCase(table.Name) + "Entity";
            using (var streamWriter = new StreamWriter(Path.Combine(entityPath, className + ".cs")))
            {
                streamWriter.WriteLine("using System;");
                streamWriter.WriteLine();
                streamWriter.WriteLine("namespace " + nameSpace);
                streamWriter.WriteLine("{");

                streamWriter.WriteLine("\t\t/// <summary>");
                streamWriter.WriteLine("\t\t/// Gets or sets the " + className + " value.");
                streamWriter.WriteLine("\t\t/// </summary>");

                streamWriter.WriteLine("\tpublic class " + className);
                streamWriter.WriteLine("\t{");

                streamWriter.WriteLine();
                foreach (var column in table.Columns)
                {
                    string parameter = Utility.CreateMethodParameter(column);
                    string type = parameter.Split(' ')[0];
                    string name = parameter.Split(' ')[1];
                    streamWriter.WriteLine("\t\t/// <summary>");
                    streamWriter.WriteLine("\t\t/// Gets or sets the " + Utility.FormatPascalCase(name) + " value.");
                    streamWriter.WriteLine("\t\t/// </summary>");
                    streamWriter.WriteLine("\t\tpublic " + type + " " + Utility.FormatPascalCase(name) + " { get; set; }");

                    streamWriter.WriteLine();
                }

                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }
        }

        public static void generateDataAccess(Table table, string path, string nameSpace, string projectType)
        {
            var entityNameSpace = nameSpace + ".Entity";
            var className = Utility.FormatPascalCase(table.Name) + "Model";
            using (var streamWriter = new StreamWriter(Path.Combine(path, className + ".cs")))
            {
                streamWriter.WriteLine("using System;");
                streamWriter.WriteLine("using System.Collections.Generic;");
                streamWriter.WriteLine("using System.Data;");
                streamWriter.WriteLine("using System.Data.SqlClient;");
                streamWriter.WriteLine("using System.Configuration;");
                streamWriter.WriteLine("using System.Threading.Tasks;");

                streamWriter.WriteLine("using " + entityNameSpace + ";");

                streamWriter.WriteLine();
                streamWriter.WriteLine("namespace " + nameSpace);
                streamWriter.WriteLine("{");

                streamWriter.WriteLine("\t\t/// <summary>");
                streamWriter.WriteLine("\t\t/// Gets or sets the " + className + " value.");
                streamWriter.WriteLine("\t\t/// </summary>");

                streamWriter.WriteLine("\tpublic class " + className);
                streamWriter.WriteLine("\t{");

                streamWriter.WriteLine();

                streamWriter.WriteLine("\t\t#region Fields");
                streamWriter.WriteLine();
                streamWriter.WriteLine("\t\tprivate string connectionString;");
                streamWriter.WriteLine();
                streamWriter.WriteLine("\t\t#endregion");
                streamWriter.WriteLine();

                // Append the constructors
                streamWriter.WriteLine("\t\t#region Constructors");
                streamWriter.WriteLine();
                streamWriter.WriteLine("\t\tpublic " + className + "()");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tthis.connectionString = Utility.GetConnectionString();");
                streamWriter.WriteLine("\t\t}");
                streamWriter.WriteLine();
                streamWriter.WriteLine("\t\t#endregion");
                streamWriter.WriteLine();
               
                createGetAllMethodWithQuery(table, streamWriter);
                createGetMethodWithQuery(table, streamWriter);

                createSaveMethodWithQuery(table, streamWriter);
                createUpdadateMethodWithQuery(table, streamWriter);
                createDeleteMethodWithQuery(table, streamWriter);
                createMapMethod(table, streamWriter, entityNameSpace);

                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }
        }

        
        private static void createGetAllMethodWithQuery(Table table, StreamWriter streamWriter)
        {
            string className = Utility.FormatPascalCase(table.Name);
            var entityName = className + "Entity";
            string variableName = "new List<" + entityName + ">();";
            // Append the method header
            streamWriter.WriteLine("\t\t/// <summary>");
            streamWriter.WriteLine("\t\t/// Get all the record to the " + table.Name + " table.");
            streamWriter.WriteLine("\t\t/// </summary>");

            streamWriter.WriteLine("\t\tpublic async Task<List<" + entityName + ">> GetAll()");

            streamWriter.WriteLine("\t\t{");
            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t\tvar " + Utility.FormatCamelCase(table.Name) + "= " + variableName);
            var query = SqlQueryGenerator.GetSelectAllQuery(table);

            generateAdoCodeForSelectAllMethod(table, streamWriter, query, Utility.FormatCamelCase(table.Name));
            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t\treturn " + Utility.FormatCamelCase(table.Name) + ";");
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void createGetMethodWithQuery(Table table, StreamWriter streamWriter)
        {
            string className = Utility.FormatPascalCase(table.Name);
            var entityName = className + "Entity";
            string variableName = Utility.FormatCamelCase(table.Name);
            // Append the method header
            streamWriter.WriteLine("\t\t/// <summary>");
            streamWriter.WriteLine("\t\t/// Get a record to the " + table.Name + " table.");
            streamWriter.WriteLine("\t\t/// </summary>");
            streamWriter.Write("\t\tpublic async Task<" + entityName + "> Get(");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write(Utility.CreateMethodParameter(column));
                if (i < (table.PrimaryKeys.Count - 1))
                {
                    streamWriter.Write(", ");
                }
            }
            streamWriter.WriteLine(")");
            streamWriter.WriteLine("\t\t{");
            streamWriter.WriteLine("\t\t\tvar parameters = new SqlParameter[]");
            streamWriter.WriteLine("\t\t\t{");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write("\t\t\t\tnew SqlParameter(\"@" + column.Name + "\", " + Utility.FormatCamelCase(column.Name) + ")");
                if (i < (table.PrimaryKeys.Count - 1))
                {
                    streamWriter.Write(",");
                }

                streamWriter.WriteLine();
            }

            streamWriter.WriteLine("\t\t\t};");
            streamWriter.WriteLine();
            var query = SqlQueryGenerator.GetSelectQuery(table);
            generateAdoCodeForSelectMethod(table, streamWriter, query);
            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void createSaveMethodWithQuery(Table table, StreamWriter streamWriter)
        {
            string className = Utility.FormatPascalCase(table.Name);
            string variableName = Utility.FormatCamelCase(table.Name);
            var entityName = className + "Entity";
            // Append the method header
            streamWriter.WriteLine("\t\t/// <summary>");
            streamWriter.WriteLine("\t\t/// Saves a record to the " + table.Name + " table.");
            streamWriter.WriteLine("\t\t/// </summary>");
            streamWriter.WriteLine("\t\tpublic void Save(" + entityName + " " + variableName + ")");
            streamWriter.WriteLine("\t\t{");

            streamWriter.WriteLine("\t\t\tif(" + variableName + " == null)");
            streamWriter.WriteLine("\t\t\t\tthrow new Exception(" + variableName + "+\" not be null\");");
            streamWriter.WriteLine();


            // Append the parameter declarations
            streamWriter.WriteLine("\t\t\tSqlParameter[] parameters = new SqlParameter[]");
            streamWriter.WriteLine("\t\t\t{");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                Column column = table.Columns[i];
                if (column.IsIdentity)
                {
                    continue;
                }
                streamWriter.Write("\t\t\t\t" + Utility.CreateSqlParameter(table, column));
                if (i < (table.Columns.Count - 1))
                {
                    streamWriter.Write(",");
                }

                streamWriter.WriteLine();
            }
            streamWriter.WriteLine("\t\t\t};");
            var query = SqlQueryGenerator.GetInsertQuery(table);
            generateAdoCodeForExecuteNonQuery(table, streamWriter, query);
            
            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        

            private static void createUpdadateMethodWithQuery(Table table, StreamWriter streamWriter)
        {
            string className = Utility.FormatPascalCase(table.Name);
            string variableName = Utility.FormatCamelCase(table.Name);
            var entityName = className + "Entity";
            // Append the method header
            streamWriter.WriteLine("\t\t/// <summary>");
            streamWriter.WriteLine("\t\t/// Update a record to the " + table.Name + " table.");
            streamWriter.WriteLine("\t\t/// </summary>");
            streamWriter.WriteLine("\t\tpublic void Update(" + entityName + " " + variableName + ")");
            streamWriter.WriteLine("\t\t{");

            streamWriter.WriteLine("\t\t\tif(" + variableName + " == null)");
            streamWriter.WriteLine("\t\t\t\tthrow new Exception(" + variableName + "+\" not be null\");");
            streamWriter.WriteLine();


            // Append the parameter declarations
            streamWriter.WriteLine("\t\t\tSqlParameter[] parameters = new SqlParameter[]");
            streamWriter.WriteLine("\t\t\t{");

            for (int i = 0; i < table.Columns.Count; i++)
            {
                Column column = table.Columns[i];
                //if (column.IsIdentity)
                //{
                //    continue;
                //}
                streamWriter.Write("\t\t\t\t" + Utility.CreateSqlParameter(table, column));
                if (i < (table.Columns.Count - 1))
                {
                    streamWriter.Write(",");
                }

                streamWriter.WriteLine();
            }
            streamWriter.WriteLine("\t\t\t};");
            var query = SqlQueryGenerator.GetUpdateQuery(table);
            generateAdoCodeForExecuteNonQuery(table, streamWriter, query);

            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void createDeleteMethodWithQuery(Table table, StreamWriter streamWriter)
        {
            string className = Utility.FormatPascalCase(table.Name);
            string variableName = Utility.FormatCamelCase(table.Name);
            // Append the method header
            streamWriter.WriteLine("\t\t/// <summary>");
            streamWriter.WriteLine("\t\t/// Delete a record to the " + table.Name + " table.");
            streamWriter.WriteLine("\t\t/// </summary>");

            streamWriter.Write("\t\tpublic void Delete(");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write(Utility.CreateMethodParameter(column));
                if (i < (table.PrimaryKeys.Count - 1))
                {
                    streamWriter.Write(", ");
                }
            }
            streamWriter.WriteLine(")");
            streamWriter.WriteLine("\t\t{");
            streamWriter.WriteLine("\t\t\tSqlParameter[] parameters = new SqlParameter[]");
            streamWriter.WriteLine("\t\t\t{");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write("\t\t\t\tnew SqlParameter(\"@" + column.Name + "\", " + Utility.FormatCamelCase(column.Name) + ")");
                if (i < (table.PrimaryKeys.Count - 1))
                {
                    streamWriter.Write(",");
                }

                streamWriter.WriteLine();
            }

            streamWriter.WriteLine("\t\t\t};");
            streamWriter.WriteLine();
            var query = SqlQueryGenerator.GetDeleteQuery(table);
            generateAdoCodeForExecuteNonQuery(table, streamWriter, query.ToString());

            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void generateAdoCodeForExecuteNonQuery(Table table, StreamWriter streamWriter, string query)
        {
            
            streamWriter.WriteLine("\t\t\tusing (var con = new SqlConnection(this.connectionString))");
            streamWriter.WriteLine("\t\t\t{");
            streamWriter.WriteLine("\t\t\t\tcon.Open();");
            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t\t\tvar cmd = new SqlCommand(\"" + query + ", con);");
            streamWriter.WriteLine("\t\t\t\tcmd.Parameters.AddRange(parameters);");
            streamWriter.WriteLine("\t\t\t\tcmd.ExecuteNonQuery();");
            streamWriter.WriteLine("\t\t\t\tcon.Close();");
            streamWriter.WriteLine("\t\t\t}");

        }

        private static void generateAdoCodeForSelectAllMethod(Table table, StreamWriter streamWriter, string query, string variableName)
        {

            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t\tusing (var con = new SqlConnection(this.connectionString))");
            streamWriter.WriteLine("\t\t\t{");
            streamWriter.WriteLine("\t\t\t\tcon.Open();");
            streamWriter.WriteLine("\t\t\t\tvar cmd = new SqlCommand(\"" + query + "\", con);");
            streamWriter.WriteLine("\t\t\t\tusing (var reader = cmd.ExecuteReader())");
            streamWriter.WriteLine("\t\t\t\t{");
            streamWriter.WriteLine("\t\t\t\t\twhile (reader.Read())");
            streamWriter.WriteLine("\t\t\t\t\t{");
            streamWriter.WriteLine("\t\t\t\t\t " + variableName + ".Add( MapDataReader(reader));");
            streamWriter.WriteLine("\t\t\t\t\t}");
            streamWriter.WriteLine("\t\t\t\t}");
            streamWriter.WriteLine("\t\t\t}");

        }

        private static void generateAdoCodeForSelectMethod(Table table, StreamWriter streamWriter, string query)
        {

            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t\t\tusing (var con = new SqlConnection(this.connectionString))");
            streamWriter.WriteLine("\t\t\t\t{");
            streamWriter.WriteLine("\t\t\t\t\tcon.Open();");
            streamWriter.WriteLine("\t\t\t\t\tvar cmd = new SqlCommand(\"" + query + "\", con);");
            streamWriter.WriteLine("\t\t\t\t\tcmd.Parameters.AddRange(parameters);");
            streamWriter.WriteLine("\t\t\t\t\tusing (var reader = cmd.ExecuteReader())");
            streamWriter.WriteLine("\t\t\t\t\t{");

            streamWriter.WriteLine("\t\t\t\t\t\tif (reader.Read())");
            streamWriter.WriteLine("\t\t\t\t\t\t{");
            streamWriter.WriteLine("\t\t\t\t\t\treturn MapDataReader(reader);");
            streamWriter.WriteLine("\t\t\t\t\t\t}");
            streamWriter.WriteLine("\t\t\t\t\t}");
            streamWriter.WriteLine("\t\t\t\treturn null;");
            streamWriter.WriteLine("\t\t\t\t}");

        }

        public static void GenerateConfigFile(string path, string projectType, string connectionString)
        {
            var configPath = string.Empty;
            string rootFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            switch (projectType)
            {
                case "API":
                    configPath = rootFolderPath + "\\API\\WebConfig.txt";
                    var config = Utility.GetResourceAsString(configPath, "#ConnectionString", connectionString);
                    using (var streamWriter = new StreamWriter(Path.Combine(path, "Web" + ".config")))
                    {
                        streamWriter.WriteLine(config);
                    }
                    break;
                case "WCF":
                    configPath = rootFolderPath + "\\API\\WebConfig.txt";
                    break;
                case "DAL":
                    configPath = rootFolderPath + "\\API\\WebConfig.txt";
                    break;
                default:
                    break;
            }
            
        }

        private static void createMapMethod(Table table, StreamWriter streamWriter, string entityNameSpace)
        {
            string className = Utility.FormatPascalCase(table.Name);
            string variableName = Utility.FormatCamelCase(className);
            var entityName = className + "Entity";

            streamWriter.WriteLine("\t\t/// <summary>");
            streamWriter.WriteLine("\t\t/// Creates a new instance of the " + className + " class and populates it with data from the specified SqlDataReader.");
            streamWriter.WriteLine("\t\t/// </summary>");
            streamWriter.WriteLine("\t\tprivate " + entityName + " MapDataReader(IDataRecord dataReader)");
            streamWriter.WriteLine("\t\t{");
            streamWriter.WriteLine("\t\t\tvar " + variableName + " = new " + entityName + "();");
            int i = 0;
            foreach (Column column in table.Columns)
            {
                string columnNamePascal = Utility.FormatPascalCase(column.Name);
                streamWriter.WriteLine("\t\t\t" + variableName + "." + columnNamePascal + " = Utility." + Utility.GetMapperMethod(column) + "(dataReader, \"" + column.Name + "\");");
                ++i;
            }

            streamWriter.WriteLine();
            streamWriter.WriteLine("\t\t\treturn " + variableName + ";");
            streamWriter.WriteLine("\t\t}");
        }

        public static void GenerateUtilFile(string ModelsPath, string namespceName)
        {
            using (var streamWriter = new StreamWriter(Path.Combine(ModelsPath, "Utility.cs")))
            {
                streamWriter.WriteLine("using System;");
                streamWriter.WriteLine("using System.Collections.Generic;");
                streamWriter.WriteLine("using System.Data;");
                streamWriter.WriteLine("using System.Data.SqlClient;");
                streamWriter.WriteLine("using System.Configuration;");
                streamWriter.WriteLine("using System.Linq;");
                streamWriter.WriteLine("using System.Web;");
                streamWriter.WriteLine();
                streamWriter.WriteLine("namespace " + namespceName);
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("\tpublic class Utility");
                streamWriter.WriteLine("\t{");
                streamWriter.WriteLine("\t\tpublic static string GetConnectionString()");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\treturn ConfigurationManager.ConnectionStrings[\"DefaultConnection\"].ConnectionString;");
                streamWriter.WriteLine("\t\t}");
                streamWriter.WriteLine();
                streamWriter.WriteLine("\t\t#region mapperfunction new");
                
                //map guid
                streamWriter.WriteLine("\t\tpublic static Guid MapGuid(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn default(Guid);");
                streamWriter.WriteLine("\t\t\treturn reader.GetGuid(reader.GetOrdinal(columnName));");
                streamWriter.WriteLine("\t\t}");

                //map string
                streamWriter.WriteLine("\t\tpublic static string MapString(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn null;");
                streamWriter.WriteLine("\t\t\treturn reader[columnName].ToString();");
                streamWriter.WriteLine("\t\t}");


                //map int
                streamWriter.WriteLine("\t\tpublic static int MapInt(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn default(int);");
                streamWriter.WriteLine("\t\t\treturn (int)reader[columnName];");
                streamWriter.WriteLine("\t\t}");

                //map decimal
                streamWriter.WriteLine("\t\tpublic static decimal MapDecimal(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn default(decimal);");
                streamWriter.WriteLine("\t\t\treturn reader.GetDecimal(reader.GetOrdinal(columnName));");
                streamWriter.WriteLine("\t\t}");

                //map float
                streamWriter.WriteLine("\t\tpublic static float MapFloat(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn default(float);");
                streamWriter.WriteLine("\t\t\treturn reader.GetFloat(reader.GetOrdinal(columnName));");
                streamWriter.WriteLine("\t\t}");

                //map DateTime
                streamWriter.WriteLine("\t\tpublic static DateTime MapDateTime(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn default(DateTime);");
                streamWriter.WriteLine("\t\t\treturn reader.GetDateTime(reader.GetOrdinal(columnName));");
                streamWriter.WriteLine("\t\t}");

                //map Bool
                streamWriter.WriteLine("\t\tpublic static bool MapBool(IDataRecord reader, string columnName)");
                streamWriter.WriteLine("\t\t{");
                streamWriter.WriteLine("\t\t\tif (reader[columnName] == DBNull.Value)");
                streamWriter.WriteLine("\t\t\treturn default(bool);");
                streamWriter.WriteLine("\t\t\treturn reader.GetBoolean(reader.GetOrdinal(columnName));");
                streamWriter.WriteLine("\t\t}");

                streamWriter.WriteLine("\t\t#endregion");


                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }
        }
    }
}
