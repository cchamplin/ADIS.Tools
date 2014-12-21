using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            string file = "assembly.json";
            List<string> assemblies = new List<string>();         
            if (args.Length == 1)
            {
                file = args[0];
            }
            if (!File.Exists(file))
            {
                System.Console.WriteLine("Unable to assembly.json file");
            }
            StreamReader sw = new StreamReader(file);
            string data = sw.ReadToEnd();
            sw.Close();

            var serializer = new FastSerialize.Serializer(typeof(FastSerialize.JsonSerializerString));
            assemblies = serializer.Deserialize<List<string>>(data);

            SchemaGenerator.AddGenerator(new ConfigurationSchemaGenerator());
            SchemaGenerator.AddGenerator(new DataBoundObjectSchemaGenerator());
            foreach (var assemblyPath in assemblies)
            {
                Console.WriteLine("Generating Schema for assembly " + assemblyPath);
                Console.WriteLine(SchemaGenerator.Generate(assemblyPath));
            }
            Console.ReadLine();
        }
    }
}
