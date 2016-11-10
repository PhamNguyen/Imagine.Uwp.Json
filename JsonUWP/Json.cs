using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Data.Json;
using System.Reflection;
using JsonUWP.Attributes;
using System.Diagnostics;
using System.Collections;

namespace JsonUWP
{
    public static class TypeExtensions
    {
        public static bool IsTypeOf<T>(this Type type)
        {
            return typeof(T) == type;
        }
    }

    public static class Json
    {
        public static T Parse<T>(String _data)
        {
            var result = default(T);

            if (String.IsNullOrEmpty(_data))
                return result;

            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass)
            {
                var types = typeInfo.ImplementedInterfaces;

                var collection = types.FirstOrDefault(p => p.FullName.Contains("Generic") || p.FullName.Contains("IList") || p.FullName.Contains("Collection") || p.FullName.Contains("IEnumerable"));

                if (collection != null)
                {
                    var itemType = type.GetGenericArguments().FirstOrDefault();

                    var items = (IList)Activator.CreateInstance(type);

                    var jsonArray = JsonArray.Parse(_data);

                    foreach (var _json in jsonArray)
                    {
                        var item = ConvertObject(itemType, _json.ToString());
                        items.Add(item);

                    }
                    result = (T)items;
                }
                else
                {
                    result = (T)ConvertObject(type, _data);
                }

            }
            else
            {
                result = (T)ConvertObject(type, _data);
            }

            return result;
        }

        public static object ConvertObject(Type type, String _data)
        {

            JsonObject jsonObject = JsonObject.Parse(_data);
            var result = Activator.CreateInstance(type);

            var properties = type.GetProperties();
            Dictionary<int, bool> checkedIndexs = new Dictionary<int, bool>();
            int count = properties.Count();
            foreach (var json in jsonObject)
            {
                if (count == 0)
                    break;
                for (int i = 0; i < count; i++)
                {
                    // Check Impelement checked

                    bool isChecked = false;
                    if (checkedIndexs.ContainsKey(i))
                        isChecked = checkedIndexs[i];
                    if (isChecked) continue;

                    // Check Property Sign
                    var property = properties.GetValue(i) as PropertyInfo;
                    var sign = property.GetCustomAttribute<SignAttribute>();
                    if (sign == null)
                        continue;

                    if (json.Key.Equals(sign.Name))
                    {

                        var propertyType = property.PropertyType;
                        var typeInfo = propertyType.GetTypeInfo();
                        // Value Type
                        if (typeInfo.IsValueType)
                        {

                            if (propertyType.IsTypeOf<int>())
                            {
                                int number = 0;
                                if (int.TryParse(json.Value.GetString(), out number))
                                {
                                    property.SetValue(result, Convert.ChangeType(number, propertyType));
                                }
                                else
                                {
                                    property.SetValue(result, Convert.ChangeType(0, propertyType));
                                }

                            }
                            else if (propertyType.IsTypeOf<bool>())
                            {
                                property.SetValue(result, Convert.ChangeType(json.Value.GetBoolean(), propertyType));
                            }
                            else if (propertyType.IsTypeOf<long>())
                            {
                                long number = 0;
                                if (long.TryParse(json.Value.GetString(), out number))
                                {
                                    property.SetValue(result, Convert.ChangeType(number, propertyType));
                                }
                                else
                                {
                                    property.SetValue(result, Convert.ChangeType(0, propertyType));
                                }
                            }
                            else if (propertyType.IsTypeOf<double>())
                            {
                                double number = 0;
                                if (double.TryParse(json.Value.GetString(), out number))
                                {
                                    property.SetValue(result, Convert.ChangeType(number, propertyType));
                                }
                                else
                                {
                                    property.SetValue(result, Convert.ChangeType(0, propertyType));
                                }
                            }
                            else if (propertyType.IsTypeOf<byte>())
                            {
                                byte number = 0;
                                if (byte.TryParse(json.Value.GetString(), out number))
                                {
                                    property.SetValue(result, Convert.ChangeType(number, propertyType));
                                }
                                else
                                {
                                    property.SetValue(result, Convert.ChangeType(0, propertyType));
                                }
                            }
                        }
                        else if (propertyType.IsTypeOf<string>())
                        {
                            property.SetValue(result, Convert.ChangeType(json.Value.GetString(), propertyType));
                        }
                        else if (typeInfo.IsClass)
                        {
                            // generic
                            if (typeInfo.IsGenericType)
                            {
                                var types = typeInfo.ImplementedInterfaces;

                                var collection = types.FirstOrDefault(p => p.FullName.Contains("Generic") || p.FullName.Contains("IList") || p.FullName.Contains("Collection") || p.FullName.Contains("IEnumerable"));

                                if (collection != null) // Generic Collection
                                {
                                    var itemType = propertyType.GetGenericArguments().FirstOrDefault();

                                    String _arrayData = json.Value != null ? json.Value.ToString() : String.Empty;

                                    if (!String.IsNullOrEmpty(_arrayData))
                                    {
                                        var items = (IList)Activator.CreateInstance(propertyType);

                                        var jsonArray = JsonArray.Parse(_arrayData);
                                        //     List<object> data = new List<Object>();
                                        foreach (var _json in jsonArray)
                                        {
                                            var item = ConvertObject(itemType, _json.ToString());
                                            items.Add(item);
                                            //      data.Add(item);
                                        }
                                        property.SetValue(result, Convert.ChangeType(items, propertyType));
                                    }
                                }
                                else // Generic class
                                {
                                    property.SetValue(result, Convert.ChangeType(ConvertObject(propertyType, json.Value.ToString()), propertyType));
                                }
                            }
                            else // reference class
                            {
                                //property.SetValue(result, Convert.ChangeType(json.Value.GetString(), propertyType));
                                property.SetValue(result, Convert.ChangeType(ConvertObject(propertyType, json.Value.ToString()), propertyType));
                            }
                        }
                        else if (propertyType.IsByRef)
                        {
                            int j = 0;
                        }
                        else if (propertyType.IsGenericParameter)
                        {
                            int j = 0;
                        }
                        else if (typeInfo.IsGenericType)
                        {
                            int j = 0;
                        }


                        // Ref Type

                        // Array

                        // Mark elememnt checked
                        checkedIndexs[i] = true;
                        break;
                    }

                }
            }

            return result;
        }
    }
}
