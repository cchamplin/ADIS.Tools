using ADIS.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaGenerator
{
    class DataBoundObjectSchemaGenerator : ISchemaGenerator 
    {
        public string Generate(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var attributes = t.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                if (attribute is DataTable)
                {
                    var ce = attribute as DataTable;

                    SqlServerSyntaxGenerator gen = new SqlServerSyntaxGenerator();
                    if (ce.Schema == null)
                    {
                        gen.SetTable(ce.TableName);
                    }
                    else
                    {
                        gen.SetTable(ce.Schema, ce.TableName);
                    }
                    var props = t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);


                    foreach (var prop in props)
                    {
                        var propAttributes = prop.GetCustomAttributes(false);


                        DataMemberType propType = null;

                        foreach (var propAttribute in propAttributes)
                        {
                            if (propAttribute is DataMemberType)
                            {
                                propType = propAttribute as DataMemberType;

                            }
                        }

                        foreach (var propAttribute in propAttributes)
                        {
                            if (propAttribute is DataMember)
                            {
                                var cp = propAttribute as DataMember;
                                if (propType != null)
                                {
                                    if (cp.Length == -1)
                                    {
                                        gen.AddColumn(cp.ColumnName, propType.Type,cp.Nullable, cp.PrimaryKey);
                                    }
                                    else
                                    {
                                        gen.AddColumn(cp.ColumnName, propType.Type,cp.Nullable, cp.PrimaryKey, cp.Length);
                                    }
                                }
                                else
                                {

                                    if (cp.Length == -1)
                                    {
                                        gen.AddColumn(cp.ColumnName, prop.PropertyType, cp.Nullable, cp.PrimaryKey);
                                    }
                                    else
                                    {
                                        gen.AddColumn(cp.ColumnName, prop.PropertyType,cp.Nullable, cp.PrimaryKey, cp.Length);
                                    }
                                }
                            }
                        }
                    }
                    sb.Append(gen.Generate());
                    break;
                }

            }
            return sb.ToString();
        }
    }
}
