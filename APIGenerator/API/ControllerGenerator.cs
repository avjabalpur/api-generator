using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenreater.Common;

namespace CodeGenreater.API
{
   public class ControllerGenerator
    {
       public static string entityNameSpace = string.Empty;
        public static void GenerateController(string ControllersPath, List<Common.Table> tables, string nameSpaceName)
        {
            entityNameSpace = nameSpaceName + ".Models.Entity";

            foreach (var table in tables)
            {
                var className = Utility.FormatPascalCase(table.Name);
                var entityName = className + "Entity";
                var modelName = className + "Model";
                var variableName = Utility.FormatCamelCase(table.Name) + "Model";
                using (var streamWriter = new StreamWriter(Path.Combine(ControllersPath, className + "Controller.cs")))
                {
                    
                    streamWriter.WriteLine("using System;");
                    streamWriter.WriteLine("using System.Collections.Generic;");
                    streamWriter.WriteLine("using System.Linq;");
                    streamWriter.WriteLine("using System.Net;");
                    streamWriter.WriteLine("using System.Net.Http;");
                    streamWriter.WriteLine("using System.Web.Http;");
                    streamWriter.WriteLine("using System.Threading.Tasks;");
                    streamWriter.WriteLine("using " + nameSpaceName + ".Models;");
                    streamWriter.WriteLine("using " + nameSpaceName + ".Models.Entity;");

                    streamWriter.WriteLine();
                    streamWriter.WriteLine("namespace "+nameSpaceName+".Controllers");
                    streamWriter.WriteLine("{");

                    streamWriter.WriteLine("\t/// <summary>");
                    streamWriter.WriteLine("\t/// Gets or sets the " + className + " controller.");
                    streamWriter.WriteLine("\t/// </summary>");

                    // for attributeRouting
                    streamWriter.WriteLine("\t [RoutePrefix(\"api\")]");

                    streamWriter.WriteLine("\tpublic class " + className + "Controller : ApiController");
                    streamWriter.WriteLine("\t{");
                    streamWriter.WriteLine("\t\t"+ modelName + " " + variableName + " = new " + modelName + "();");
                    streamWriter.WriteLine();

                    genreateGetAllMethod(table, streamWriter, className, variableName);
                    genreateGetMethod(table, streamWriter, className, variableName);
                    genreatePostMethod(table, streamWriter, className, variableName);
                    genreatePutMethod(table, streamWriter, className, variableName);
                    genreateDeleteMethod(table, streamWriter, className, variableName);

                    streamWriter.WriteLine("\t}");
                    streamWriter.WriteLine("}");
                }
            }
        }

        private static void genreateDeleteMethod(Table table, StreamWriter streamWriter, string className, string variableName)
        {
            var routeName = Utility.SplitTextToDelimater(className);
            streamWriter.Write("\t\t[Route(\"" + routeName);

            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write("/{" + Utility.FormatCamelCase(column.Name) + "}");
            }
            
                 streamWriter.WriteLine("\")]");
            streamWriter.WriteLine("\t\t[HttpDelete]");

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
            streamWriter.Write("\t\t\t " + variableName + ".Delete(");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write(Utility.FormatCamelCase(column.Name));
                if (i < (table.PrimaryKeys.Count - 1))
                {
                    streamWriter.Write(", ");
                }
            }
            streamWriter.WriteLine(");");
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void genreatePostMethod(Table table, StreamWriter streamWriter, string className, string variableName)
        {
            var entityName = className + "Entity";
            var parmaName = Utility.FormatCamelCase(className);

            var routeName = Utility.SplitTextToDelimater(className);
            streamWriter.Write("\t\t[Route(\"" + routeName+ "\")]");
            streamWriter.WriteLine("\t\t[HttpPost]");


            streamWriter.WriteLine("\t\tpublic void Post([FromBody] " + entityName + " " + parmaName + ")");
            streamWriter.WriteLine("\t\t{");
            streamWriter.WriteLine("\t\t\t" + variableName + ".Save(" + parmaName + ");");
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void genreatePutMethod(Table table, StreamWriter streamWriter, string className, string variableName)
        {
            var parmaName = Utility.FormatCamelCase(className);
            var entityName = className + "Entity";
            
            var routeName = Utility.SplitTextToDelimater(className);
            streamWriter.Write("\t\t[Route(\"" + routeName);

            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write("/{" + Utility.FormatCamelCase(column.Name) + "}");
            }

            streamWriter.WriteLine("\")]");
            streamWriter.WriteLine("\t\t[HttpPut]");

            streamWriter.Write("\t\tpublic void Put( [FromBody] " + entityName + " "+ parmaName + ",  ");
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
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                    streamWriter.WriteLine("\t\t\t"+parmaName +"."+ column.Name+ "= "+ Utility.FormatCamelCase(column.Name)+";");
            }
            streamWriter.Write("\t\t\t " + variableName + ".Update("+ parmaName);
            streamWriter.WriteLine(");");
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void genreateGetAllMethod(Table table, StreamWriter streamWriter, string className, string variableName)
        {
            var entityName = className + "Entity";
            var routeName = Utility.SplitTextToDelimater(className);

            streamWriter.WriteLine("\t\t[Route(\"" + routeName + "\")]");
            streamWriter.WriteLine("\t\t[HttpGet]");

            streamWriter.WriteLine("\t\tpublic async Task<IEnumerable<" + entityName + ">> Get()");
            streamWriter.WriteLine("\t\t{");
            streamWriter.WriteLine("\t\t\treturn await " + variableName + ".GetAll();");
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

        private static void genreateGetMethod(Table table, StreamWriter streamWriter, string className, string variableName)
        {
            var entityName = className + "Entity";

            var routeName = Utility.SplitTextToDelimater(className);
            streamWriter.Write("\t\t[Route(\"" + routeName);

            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write("/{" + Utility.FormatCamelCase(column.Name)+"}");
            }

            streamWriter.WriteLine("\")]");
            streamWriter.WriteLine("\t\t[HttpGet]");

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
            streamWriter.Write("\t\t\treturn await " + variableName + ".Get(");
            for (int i = 0; i < table.PrimaryKeys.Count; i++)
            {
                Column column = table.PrimaryKeys[i];
                streamWriter.Write(Utility.FormatCamelCase(column.Name));
                if (i < (table.PrimaryKeys.Count - 1))
                {
                    streamWriter.Write(", ");
                }
            }
            streamWriter.WriteLine(");");
            streamWriter.WriteLine("\t\t}");
            streamWriter.WriteLine();
        }

    }
}
