using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Reflection.BindingFlags;

namespace Utils
{
    public static class Serializer
    {
        private static Dictionary<Type, byte> simpleTypeCodes = new Dictionary<Type, byte>()
        {
            {typeof(byte),      0},
            {typeof(sbyte),     1},         
            {typeof(short),     2},
            {typeof(ushort),    3},
            {typeof(int),       4},
            {typeof(uint),      5},
            {typeof(long),      6},
            {typeof(ulong),     7},
            {typeof(float),     8},
            {typeof(double),    9},
            {typeof(decimal),   10},
            {typeof(bool),      11},
            {typeof(string),    12},
        };

        public static void Serialize(object graph, Stream stream)
        {
            if (graph == null)
            {
                throw new ArgumentNullException($"{nameof(graph)} is null");
            }

            if (stream == null)
            {
                throw new ArgumentNullException($"{nameof(stream)} is null");
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException($"{nameof(stream)} must be writeable.");
            }

            SerializeRecursive(graph, stream);
        }

        public static object Deserialize(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException($"{nameof(stream)} is null");
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException($"{nameof(stream)} must be readable.");
            }

            return DeserializeRecursive(stream);
        }

        private static void SerializeRecursive(object obj, Stream stream)
        {
            if (obj == null)
            {
                byte[] nullbytes = BitConverter.GetBytes((short)2);
                stream.Write(nullbytes, 0, 2);
                return;
            }
                
            var type = obj.GetType();
            if (type.GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0)
            {
                if (simpleTypeCodes.Keys.Contains(type))
                {
                    SerializeSimple(obj, stream);
                }
                else
                {
                    if (type.IsEnum)
                    {                       
                        object underlyingValue = Convert.ChangeType(obj, Enum.GetUnderlyingType(obj.GetType()));
                        SerializeSimple(underlyingValue, stream);
                    }
                    else
                    {
                        WriteTypeName(type, stream);
                        var fields = type.GetAllFields();
                        foreach (var field in fields)
                        {
                            // Write field
                            var fieldValue = field.GetValue(obj);
                            SerializeRecursive(fieldValue, stream);
                        }
                    }                  
                }               
            }

            if (type.BaseType == typeof(Array))
            {
                // Value 01 means that type is array.
                stream.Write(BitConverter.GetBytes((short)1), 0, 2);
               
                WriteTypeName(type.GetElementType(), stream);
               
                // Write length
                int size = (obj as Array).Length;
                stream.Write(BitConverter.GetBytes(size), 0, 4);

                foreach (var element in obj as Array)
                {
                    SerializeRecursive(element, stream);
                }
            }
        }

        private static void SerializeSimple(object obj, Stream stream)
        {            
            var type = obj.GetType();

            // Two zero bytes means that type is simple.
            stream.Write(BitConverter.GetBytes((short)0), 0, 2);

            // Write simple type code
            stream.WriteByte(simpleTypeCodes[type]);

            byte[] bytes = new byte[0];
            switch (type.Name)
            {
                case "Sbyte":
                    bytes = BitConverter.GetBytes((sbyte)obj);
                    break;
                case "Byte":
                    bytes = BitConverter.GetBytes((byte)obj);
                    break;
                case "Int16":
                    bytes = BitConverter.GetBytes((short)obj);
                    break;
                case "UInt16":
                    bytes = BitConverter.GetBytes((ushort)obj);
                    break;
                case "Int32":
                    bytes = BitConverter.GetBytes((int)obj);
                    break;
                case "UInt32":
                    bytes = BitConverter.GetBytes((uint)obj);
                    break;
                case "Int64":
                    bytes = BitConverter.GetBytes((long)obj);
                    break;
                case "UInt64":
                    bytes = BitConverter.GetBytes((ulong)obj);
                    break;
                case "Single":
                    bytes = BitConverter.GetBytes((float)obj);
                    break;
                case "Double":
                    bytes = BitConverter.GetBytes((double)obj);
                    break;
                case "Decimal":
                    int[] bits = decimal.GetBits((decimal)obj);
                    List<byte> byteList = new List<byte>();
                    foreach (var bitPart in bits)
                    {
                        byteList.AddRange(BitConverter.GetBytes(bitPart));
                    }

                    bytes = byteList.ToArray();
                    break;
                case "Boolean":
                    bytes = BitConverter.GetBytes((bool)obj);
                    break;
                case "String":
                    var stringBytes = new List<byte>();
                    stringBytes.AddRange(BitConverter.GetBytes(((string)obj).Length));
                    stringBytes.AddRange(Encoding.Unicode.GetBytes((string)obj));
                    bytes = stringBytes.ToArray();
                    break;                   
            }

            stream.Write(bytes, 0, bytes.Length);        
        }
      
        private static object DeserializeRecursive(Stream stream)
        {
            var shortBuffer = new byte[2];
            stream.Read(shortBuffer, 0, 2);
            var length = BitConverter.ToInt16(shortBuffer, 0);
            if (length == 0)
            {
                return DeserializeSimple(stream);
            }

            if (length == 1)
            {
                return DeserializeArray(stream);
            }

            if (length == 2)
            {
                return null;
            }

            var typeBuffer = new byte[length];
            stream.Read(typeBuffer, 0, length);
            string typeName = Encoding.Unicode.GetString(typeBuffer);
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = null;
            foreach (var assembly in asms)
            {
                types = assembly.GetTypes().Where(t => t.FullName == typeName);
                if (types.Count() != 0)
                {
                    break;
                }
            }

            if (types.Count() == 0)
            {
                throw new InvalidOperationException($"Can't find type {typeName} in current assembly.");
            }

            var type = types.First();
            object result = null;
            if (type.IsGenericTypeDefinition)
            {
                int argCount = type.GetGenericArguments().Length;
                List<Type> args = new List<Type>();
                for (int i = 0; i < argCount; i++)
                {
                    args.Add(ReadType(stream));
                }

                type = type.MakeGenericType(args.ToArray());
            }

            result = Activator.CreateInstance(type);
            
            var fields = type.GetAllFields();
            foreach (var field in fields)
            {
                var value = DeserializeRecursive(stream);
                field.SetValue(result, value);
            }

            return result;
        }

        private static object DeserializeSimple(Stream stream)
        {
            int intCode = stream.ReadByte();
            byte code = (byte)intCode;
            var type = simpleTypeCodes.FirstOrDefault(x => x.Value == code).Key;
            object result = null;
            var buffer = new byte[32];
            switch (type.Name)
            {
                case "Sbyte":
                    result = (sbyte)stream.ReadByte();
                    break;
                case "Byte":
                    result = stream.ReadByte();
                    break;
                case "Int16":
                    stream.Read(buffer, 0, 2);
                    result = BitConverter.ToInt16(buffer, 0);
                    break;
                case "UInt16":
                    stream.Read(buffer, 0, 2);
                    result = BitConverter.ToUInt16(buffer, 0);
                    break;
                case "Int32":
                    stream.Read(buffer, 0, 4);
                    result = BitConverter.ToInt32(buffer, 0);
                    break;
                case "UInt32":
                    stream.Read(buffer, 0, 4);
                    result = BitConverter.ToUInt32(buffer, 0);
                    break;
                case "Int64":
                    stream.Read(buffer, 0, 8);
                    result = BitConverter.ToInt32(buffer, 0);
                    break;
                case "UInt64":
                    stream.Read(buffer, 0, 8);
                    result = BitConverter.ToUInt32(buffer, 0);
                    break;
                case "Single":
                    stream.Read(buffer, 0, 4);
                    result = BitConverter.ToSingle(buffer, 0);
                    break;
                case "Double":
                    stream.Read(buffer, 0, 8);
                    result = BitConverter.ToDouble(buffer, 0);
                    break;
                case "Decimal":
                    stream.Read(buffer, 0, 16);

                    var i1 = BitConverter.ToInt32(buffer, 0);
                    var i2 = BitConverter.ToInt32(buffer, 4);
                    var i3 = BitConverter.ToInt32(buffer, 8);
                    var i4 = BitConverter.ToInt32(buffer, 12);

                    result = new decimal(new int[] { i1, i2, i3, i4 });
                    break;
                case "Boolean":
                    stream.Read(buffer, 0, 1);
                    result = BitConverter.ToBoolean(buffer, 0);
                    break;
                case "String":
                    stream.Read(buffer, 0, 4);
                    int length = BitConverter.ToInt32(buffer, 0);
                    var stringBuffer = new byte[length * 2];
                    stream.Read(stringBuffer, 0, length * 2);
                    result = Encoding.Unicode.GetString(stringBuffer);
                    break;
            }

            return result;
        }

        private static object DeserializeArray(Stream stream)
        {
            // Read length of type
            var shortBuffer = new byte[2];
            stream.Read(shortBuffer, 0, 2);
            var typeLength = BitConverter.ToInt16(shortBuffer, 0);

            // Read type
            var typeBuffer = new byte[typeLength];
            stream.Read(typeBuffer, 0, typeLength);
            string typeName = Encoding.Unicode.GetString(typeBuffer);
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = null;
            foreach (var assembly in asms)
            {
                types = assembly.GetTypes().Where(t => t.FullName == typeName);
                if (types.Count() != 0)
                {
                    break;
                }
            }

            if (types.Count() == 0)
            {
                throw new InvalidOperationException($"Can't find type {typeName} in current assembly.");
            }

            var type = types.First();

            // Read Length
            var intBuffer = new byte[4];
            stream.Read(intBuffer, 0, 4);
            var length = BitConverter.ToInt32(intBuffer, 0);

            // Read Elements
            var arrayType = type.MakeArrayType();
            Array arr = Activator.CreateInstance(arrayType, new object[] { length }) as Array;

            for (int i = 0; i < length; i++)
            {
                var element = DeserializeRecursive(stream);
                arr.SetValue(element, i);
            }

            return arr;
        }

        private static List<FieldInfo> GetAllFields(this Type type)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            if (type.IsClass)
            {
                // Get all fields
                List<Type> parentTypes = new List<Type>();
                var parent = type;
                while (!parent.Equals(typeof(object)))
                {
                    parentTypes.Add(parent);
                    parent = parent.BaseType;
                }

                foreach (var pt in parentTypes)
                {
                    fields.AddRange(pt.GetFields(NonPublic | Instance));
                    fields.AddRange(pt.GetFields(Instance));
                }

                fields = fields.Distinct(new TypeComparer()).ToList();
            }
            else
            {
                fields.AddRange(type.GetFields(NonPublic | Instance));
                fields.AddRange(type.GetFields(Instance));
            }

            return fields;
        }
       
        private static void WriteTypeName(Type type, Stream stream)
        {
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var arguments = type.GetGenericArguments();
                var definition = type.GetGenericTypeDefinition();
                WriteTypeName(definition, stream);
                foreach (var arg in arguments)
                {
                    WriteTypeName(arg, stream);
                }
            }
            else
            {
                var name = type.FullName;

                // Write type name
                var typeNameBytes = Encoding.Unicode.GetBytes(name);

                // Write type name length
                short typeLength = (short)typeNameBytes.Length;
                var typeLengthBytes = BitConverter.GetBytes(typeLength);
                stream.Write(typeLengthBytes, 0, typeLengthBytes.Length);

                stream.Write(typeNameBytes, 0, typeNameBytes.Length);
            }
        }

        private static Type ReadType(Stream stream)
        {
            var shortBuffer = new byte[2];
            stream.Read(shortBuffer, 0, 2);
            var length = BitConverter.ToInt16(shortBuffer, 0);
            var typeBuffer = new byte[length];
            stream.Read(typeBuffer, 0, length);
            var typeName = Encoding.Unicode.GetString(typeBuffer);

            var asms = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = null;
            foreach (var assembly in asms)
            {
                types = assembly.GetTypes().Where(t => t.FullName == typeName);
                if (types.Count() != 0)
                {
                    break;
                }
            }

            if (types.Count() == 0)
            {
                throw new InvalidOperationException($"Can't find type {typeName} in current assembly.");
            }

            var type = types.First();

            if (type.IsGenericTypeDefinition)
            {
                int argCount = type.GetGenericArguments().Length;
                List<Type> args = new List<Type>();
                for (int i = 0; i < argCount; i++)
                {
                    args.Add(ReadType(stream));
                }

                type = type.MakeGenericType(args.ToArray());
            }

            return type;
        }

        private class TypeComparer : IEqualityComparer<FieldInfo>
        {
            public bool Equals(FieldInfo x, FieldInfo y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(FieldInfo obj)
            {
                return 1;
            }
        }
    }
}
