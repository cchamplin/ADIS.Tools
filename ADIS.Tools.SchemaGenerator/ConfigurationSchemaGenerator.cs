using ADIS.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaGenerator
{
    class ConfigurationSchemaGenerator : ISchemaGenerator
    {
        public string Generate(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var attributes = t.GetCustomAttributes(false);

            foreach (var attribute in attributes) {
                if (attribute is ConfigurationEntity)
                {
                    var ce = attribute as ConfigurationEntity;
                    if (ce.Type == ConfigurationEntityType.DataBound)
                    {
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


                            ConfigurationPropertyType propType = null;

                            foreach (var propAttribute in propAttributes)
                            {
                                if (propAttribute is ConfigurationPropertyType)
                                {
                                    propType = propAttribute as ConfigurationPropertyType;
                                    
                                }
                            }

                            foreach (var propAttribute in propAttributes)
                            {
                                if (propAttribute is ConfigurationProperty)
                                {
                                    var cp = propAttribute as ConfigurationProperty;
                                    if (propType != null)
                                    {
                                        if (cp.Length == -1)
                                        {
                                            gen.AddColumn(cp.Column, propType.Type,cp.Nullable, cp.Primary);
                                        }
                                        else
                                        {
                                            gen.AddColumn(cp.Column, propType.Type,cp.Nullable, cp.Primary, cp.Length);
                                        }
                                    }
                                    else
                                    {

                                        if (cp.Length == -1)
                                        {
                                            gen.AddColumn(cp.Column, prop.PropertyType,cp.Nullable, cp.Primary);
                                        }
                                        else
                                        {
                                            gen.AddColumn(cp.Column, prop.PropertyType,cp.Nullable, cp.Primary, cp.Length);
                                        }
                                    }
                                }
                            }
                        }
                        sb.Append(gen.Generate());
                        break;
                    }
                }
            }
            return sb.ToString();
        }
    }
}
