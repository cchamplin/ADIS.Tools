using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
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
            var types = assembly.GetTypes();
            StringBuilder results = new StringBuilder();
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
