using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
namespace SchemaGenerator
{
    public static class SchemaGenerator
    {
        private static List<ISchemaGenerator> generators;
        static SchemaGenerator()
        {
            generators = new List<ISchemaGenerator>();
        }
        public static void AddGenerator(ISchemaGenerator generator)
        {
            generators.Add(generator);
        }
        public static string Generate(string assemblyPath)
        {
            
            Assembly assembly = null;
            if (System.IO.Path.IsPathRooted(assemblyPath))
            {
                assembly = Assembly.LoadFile(assemblyPath);
            }
            else
            {
                assembly = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\" + assemblyPath);
            }
            StringBuilder results = new StringBuilder();
            string[] resourceNames = assembly.GetManifestResourceNames();
            var pattern = @"^(.+\.schema_[a-zA-Z0-9_-]+\.sql)$";
            var regex = new Regex(pattern,RegexOptions.Singleline);
            Console.WriteLine(pattern);
            foreach (string resourceName in resourceNames)
            {
                if (regex.IsMatch(resourceName.ToLower()))
                {
                    Console.WriteLine("Found Schema Resource: " + resourceName);
                    var stream = new StreamReader(assembly.GetManifestResourceStream(resourceName));
                    results.Append(stream.ReadToEnd());
                    results.AppendLine();
                    results.AppendLine();
                    stream.Close();

                }

            }


            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                foreach (var generator in generators)
                {
                    results.Append(generator.Generate(type));
                }
            }

            return results.ToString(); ;
        }
    }
}
