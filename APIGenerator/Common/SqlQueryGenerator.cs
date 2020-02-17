using System;
using System.Text;

namespace CodeGenreater.Common
{
    public class SqlQueryGenerator
    {
        /// <summary>
        /// Generate the sql query for Get
        /// </summary>
        /// <param name="databaseName"></param>
        public static string GetSelectAllQuery(Table table)
        {
            // Create the stored procedure name
            var sb = new StringBuilder();
            sb.Append(" SELECT * FROM [" + table.Name+"]");
            return sb.ToString();
        }

        /// <summary>
        /// Generate the sql query for Get
        /// </summary>
        /// <param name="databaseName"></param>
        public static string GetSelectQuery(Table table)
        {
            // Create the stored procedure name
            var sb = new StringBuilder();
            var primaryKey = string.Empty;
            sb.Append("SELECT * FROM [" + table.Name + "]");
            sb.Append(" WHERE ");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];

                if (i == 0)
                {
                    sb.Append(" [" + column.Name + "] = @" + column.Name);
                }
                else
                {
                    sb.Append("\tand [" + column.Name + "] = @" + column.Name);
                }
            }
            return sb.ToString();
        }

        public static string GetInsertQuery(Table table)
        {
            // Create the stored procedure name
            var sb = new StringBuilder();
            var primaryKey = string.Empty;

            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                var column = table.PrimaryKeys[i];
                if (i == (table.PrimaryKeys.Count - 1))
                {
                    primaryKey = column.Name;
                }

            }

            sb.AppendLine(" insert into [" + table.Name + "] ( \"");
            // Create the parameter list
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Column column = table.Columns[i];

                // Ignore any identity columns
                if (column.IsIdentity == false)
                {
                    // Append the column name as a parameter of the insert statement
                    if (i < (table.Columns.Count - 1))
                    {
                        sb.AppendLine("\t\t\t\t+\"[" + column.Name + "],\"");
                    }
                    else
                    {
                        sb.AppendLine("\t\t\t\t+\"[" + column.Name + "]\"");
                    }
                }
            }

            sb.AppendLine("\t\t\t\t+\")\"");
            sb.AppendLine("\t\t\t\t+\" values ( \"");
            // Create the values list
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Column column = table.Columns[i];

                // Is the current column an identity column?
                if (column.IsIdentity == false)
                {
                    // Append the necessary line breaks and commas
                    if (i < (table.Columns.Count - 1))
                    {
                        sb.AppendLine("\t\t\t\t+\"@" + column.Name + ",+\"");
                    }
                    else
                    {
                        sb.AppendLine("\t\t\t\t+\"@" + column.Name +"\"");
                    }
                }
            }

            sb.AppendLine("\t\t\t\t+\" )\"");

            // Should we include a line for returning the identity?
            foreach (Column column in table.Columns)
            {
                // Is the current column an identity column?
                if (column.IsIdentity)
                {
                    sb.Append("\t\t\t\t+\"select scope_identity()\"");
                    break;
                }
                else if (column.IsRowGuidCol)
                {
                    sb.Append("\t\t\t\t+\" Select @" + column.Name+ "\"");
                    break;
                }
            }

            return sb.ToString();
        }

        public static string GetUpdateQuery(Table table)
        {
            // Create the stored procedure name
            var sb = new StringBuilder();
            var primaryKey = string.Empty;

            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                var column = table.PrimaryKeys[i];
                if (i == (table.PrimaryKeys.Count - 1))
                {
                    primaryKey = column.Name;
                }

            }

            sb.AppendLine(" UPDATE [" + table.Name + "] SET \"");
            // Create the parameter list

            bool firstLine = true;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = (Column)table.Columns[i];

                // Ignore Identity and RowGuidCol columns
                if (table.PrimaryKeys.Contains(column) == false)
                {
                    if (firstLine)
                    {
                        sb.AppendLine("");
                        firstLine = false;
                    }
                    else
                    {
                        sb.Append("\t");
                    }

                    sb.Append("\t\t\t\t+\"[" + column.Name + "] = @" + column.Name+"");

                    if (i < (table.Columns.Count - 1))
                    {
                        sb.AppendLine(",\"");
                    }
                }
            }
            sb.AppendLine("\"");
            sb.AppendLine("\t\t\t\t+\" Where \"");


            // Create the where clause
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                var column = table.PrimaryKeys[i];

                if (i == 0)
                {
                    sb.Append("\t\t\t\t+\" [" + column.Name + "] = @" + column.Name+"\"");
                }
                else
                {
                    sb.Append("\t\t\t\t+\"and [" + column.Name + "] = @" + column.Name + "\"");
                }
            }
            return sb.ToString();
        }

        internal static object GetDeleteQuery(Table table)
        {
            // Create the stored procedure name
            var sb = new StringBuilder();
            var primaryKey = string.Empty;
            sb.Append("DELETE [" + table.Name + "]");
            sb.AppendLine(" WHERE \"");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];

                if (i == 0)
                {
                    sb.Append("\t\t\t\t+\" [" + column.Name + "] = @" + column.Name + "\"");
                }
                else
                {
                    sb.Append("\t\t\t\t+\"and [" + column.Name + "] = @" + column.Name + "\"");
                }
            }
            return sb.ToString();
        }
    }

}
