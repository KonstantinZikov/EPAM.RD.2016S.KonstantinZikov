using Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System;

namespace AttributeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.Load("Attributes");
            var userTypes = assembly.GetTypes().Where(
                (t) => t.CustomAttributes.Where(
                    (a) => a.AttributeType == typeof(InstantiateUserAttribute)
                    ).Count() > 0
                );
            var objects = new List<object>();

            foreach(var type in userTypes)
            {
                var instantinateAttrs = type.GetCustomAttributes<InstantiateUserAttribute>();
                foreach(var attribute in instantinateAttrs)
                {
                    objects.Add(CreateUser(type, attribute));
                }                         
            }

            var assemblyInstantinateAttrs = 
                assembly.GetCustomAttributes<InstantiateAdvancedUserAttribute>();
            foreach(var assemblyAttribute in assemblyInstantinateAttrs)
            {
                objects.Add(CreateUser(typeof(AdvancedUser), assemblyAttribute));
            }

            foreach(var obj in objects)
            {
                
                Console.WriteLine(obj);
                ValidateProperties(obj);
                ValidateFields(obj);
                Console.WriteLine("===========================");
            }

            Console.ReadKey();
        }

        private static object CreateUser(Type type, Attribute attribute)
        {
            var constructorParamsValues = new Dictionary<string, object>();
            var constructor = type.GetConstructors().First();
            var matchAttributes = constructor.GetCustomAttributes
                <MatchParameterWithPropertyAttribute>();
            var constructorParams = constructor.GetParameters();
            foreach (var constructorParam in constructorParams)
            {
                var atr = matchAttributes.Where((a) => a.Parameter == constructorParam.Name).First();
                var propName = atr.Property;
                var prop = type.GetProperty(propName);
                object defaultValue = null;

                var attributeProperties = attribute.GetType().GetProperties();
                var linkedValue = attributeProperties.Where((z) => z.Name == atr.Property);
                
                if (linkedValue.Count()!=0)
                {
                    defaultValue = linkedValue.First().GetValue(attribute);
                }
                else
                {
                    var defaultValueAttr = prop.GetCustomAttribute<DefaultValueAttribute>();
                    defaultValue = defaultValueAttr.Value;
                }               
                constructorParamsValues.Add(atr.Parameter, defaultValue);
            }

            var obj = constructor.Invoke(constructorParamsValues.Values.ToArray());
            foreach (var attributeProperty in attribute.GetType().GetProperties())
            {
                if (attributeProperty.Name != "TypeId")
                {
                    var objProp = obj.GetType().GetProperty(attributeProperty.Name);
                    if (objProp.CanWrite)
                    {
                        objProp.SetValue(obj, attributeProperty.GetValue(attribute));
                    }                   
                }
            }
            return obj;
        }

        private static void ValidateProperties(object obj)
        {
            var userProperties = obj.GetType().GetProperties(BindingFlags.NonPublic |
                         BindingFlags.Instance | BindingFlags.Public);
            foreach(var property in userProperties)
            {               
                if (property.PropertyType == typeof(string))
                {
                    var intAttr = property.GetCustomAttribute
                        <StringValidatorAttribute>();
                    if (intAttr != null)
                    {
                        var propertyValue = (string)property.GetValue(obj);
                        if (propertyValue.Length > intAttr.SymbolCount)
                        {
                            Console.WriteLine($"!!!!!Object's \"{obj}\" property {property.Name} is invalid.");
                        }
                        else
                        {
                            Console.WriteLine($"Object's \"{obj}\" property {property.Name} is valid:)");
                        }
                    }
                }
                else if (property.PropertyType == typeof(int))
                {
                    var intAttr = property.GetCustomAttribute
                        <IntValidatorAttribute>();
                    if (intAttr != null)
                    {
                        var propertyValue = (int)property.GetValue(obj);
                        if (propertyValue < intAttr.MinValue ||
                            propertyValue > intAttr.MaxValue)
                        {
                            Console.WriteLine($"!!!!!Object's \"{obj}\" property {property.Name} is invalid.");
                        }
                        else
                        {
                            Console.WriteLine($"Object's \"{obj}\" property {property.Name} is valid:)");
                        }
                    }
                }
            }
        }

        private static void ValidateFields(object obj)
        {
            var userFields = obj.GetType().GetFields(BindingFlags.NonPublic |
                         BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in userFields)
            {
                if (field.FieldType == typeof(string))
                {
                    var intAttr = field.GetCustomAttribute
                        <StringValidatorAttribute>();
                    if (intAttr != null)
                    {
                        var propertyValue = (string)field.GetValue(obj);
                        if (propertyValue.Length > intAttr.SymbolCount)
                        {
                            Console.WriteLine($"!!!!!Object's \"{obj}\" field {field.Name} is invalid.");
                        }
                        else
                        {
                            Console.WriteLine($"Object's \"{obj}\" field {field.Name} is valid:)");
                        }
                    }
                }
                if (field.FieldType == typeof(int))
                {
                    var intAttr = field.GetCustomAttribute
                        <IntValidatorAttribute>();
                    if (intAttr != null)
                    {
                        var propertyValue = (int)field.GetValue(obj);
                        if (propertyValue < intAttr.MinValue ||
                            propertyValue > intAttr.MaxValue)
                        {
                            Console.WriteLine($"!!!!!Object's \"{obj}\" field {field.Name} is invalid.");
                        }
                        else
                        {
                            Console.WriteLine($"Object's \"{obj}\" field {field.Name} is valid:)");
                        }
                    }
                }
            }
        }
    }
}
