namespace Zanshin.Domain.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    using Zanshin.Domain.Attributes;
    using Zanshin.Domain.Extensions;

    /// <summary>
    /// </summary>
    public sealed class DataMaker
    {
        private static class EnglishAverages
        {
            /// <summary>
            /// Gets the word minimum.
            /// </summary>
            /// <value>
            /// The word minimum.
            /// </value>
            public static int WordMin
            {
                get { return 3; }
            }

            /// <summary>
            /// Gets the word maximum.
            /// </summary>
            /// <value>
            /// The word maximum.
            /// </value>
            public static int WordMax
            {
                get { return 12; }
            }

            /// <summary>
            /// Gets the sentence minimum.
            /// </summary>
            /// <value>
            /// The sentence minimum.
            /// </value>
            public static int SentenceMin
            {
                get { return 3; }
            }

            /// <summary>
            /// Gets the sentence maximum.
            /// </summary>
            /// <value>
            /// The sentence maximum.
            /// </value>
            public static int SentenceMax
            {
                get { return 25; }
            }
        }

        private static readonly Random random = new Random();

        /// <summary>
        /// Gets the char.
        /// </summary>
        /// <returns></returns>
        public static char GetChar()
        {
            return GetString(1).ToCharArray()[0];
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DataMaker Instance
        {
            get { return new DataMaker(); }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns></returns>
        public static string GetString(int length = 0)
        {
            return RandomValue(length);
        }

        /// <summary>
        /// Resets the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="one">The one.</param>
        public void ResetProperties<T>(ref T one)
        {
            Type t = typeof(T);
            var properties = t.GetProperties().ToList();
            this.CreateAndFill(t);
            properties.Each(p => { });
        }

        /// <summary>
        /// Creates a deep copy of &lt;T&gt; essentially ensuring the 
        /// item is in a separate memory space on the heap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public T DeepCopy<T>(T t)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            try
            {
                bf.Serialize(ms, t);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            ms.Flush();
            ms.Position = 0;
            return (T)(bf.Deserialize(ms));
        }

        /// <summary>
        /// Creates the and fill.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        // TODO Allow parameter to specify a property name and a value to set the property
        public T CreateAndFill<T>()
        {
            return (T)this.CreateAndFill(typeof(T));
        }

        /// <summary>
        /// Creates the and fill.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="currentDepth">The current depth.</param>
        /// <returns></returns>
        public object CreateAndFill(Type t, int currentDepth = 0)
        {
            if (t.IsValueType)
            {
                return this.CreateSimpleValueType(t);
            }

            if (t == typeof(String))
            {
                return GetString();
            }

            if (t == typeof(bool))
            {
                return random.Next() % 2 == 0;
            }

            if (t == typeof(char))
            {
                return GetChar();
            }

            // var fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            var entity = Activator.CreateInstance(t);
            var properties = t.GetProperties().ToList();

            foreach (var p in properties)
            {
                if (p.GetCustomAttributes<DoNotFillAttribute>().Any())
                {
                    continue;
                }

                if (p.PropertyType == typeof(String))
                {
                    StringLengthAttribute stringLength =
                        (StringLengthAttribute)p.GetCustomAttribute(typeof(StringLengthAttribute));

                    if (stringLength != null)
                    {
                        var min = (stringLength.MinimumLength > 0 ? stringLength.MinimumLength : 3);
                        var max = stringLength.MaximumLength;
                        p.SetValue(entity, GetString(random.Next(min, max)));
                        continue;
                    }

                    MaxLengthAttribute maxLength =
                        (MaxLengthAttribute)p.GetCustomAttribute(typeof(MaxLengthAttribute));

                    if (maxLength != null)
                    {
                        var max = maxLength.Length;
                        p.SetValue(entity, GetString(random.Next(3, max)));
                        continue;
                    }
                    p.SetValue(entity, GetString(), null);
                }
                else if (p.PropertyType == typeof(bool))
                {
                    p.SetValue(entity, (random.Next() % 2 == 0), null);
                }
                else if (p.PropertyType.IsValueType)
                {
                    ValueType valueType = ValueTypeFactory.Create(p.PropertyType);

                    if (valueType.IsDouble)
                    {
                        p.SetValue(entity, valueType.ADouble, null);
                    }
                    else if (valueType.IsDecimal)
                    {
                        p.SetValue(entity, valueType.ADecimal, null);
                    }
                    else if (valueType.IsFloat)
                    {
                        p.SetValue(entity, valueType.AFloat, null);
                    }
                    else if (valueType.IsShort)
                    {
                        p.SetValue(entity, valueType.AShort, null);
                    }
                    else if (valueType.IsUint)
                    {
                        p.SetValue(entity, valueType.AUint, null);
                    }
                    else if (valueType.IsUshort)
                    {
                        p.SetValue(entity, valueType.AUshort, null);
                    }
                    else if (valueType.IsUlong)
                    {
                        p.SetValue(entity, valueType.AUlong, null);
                    }
                    else if (valueType.IsInt)
                    {
                        p.SetValue(entity, valueType.AInt, null);
                    }
                    else if (valueType.IsNullableDecimal)
                    {
                        p.SetValue(entity, valueType.NullableDecimal, null);
                    }
                    else if (valueType.IsNullableDouble)
                    {
                        p.SetValue(entity, valueType.NullableDouble, null);
                    }
                    else if (valueType.IsNullableFloat)
                    {
                        p.SetValue(entity, valueType.NullableFloat, null);
                    }
                    else if (valueType.IsNullableInt)
                    {
                        p.SetValue(entity, valueType.NullableInt, null);
                    }
                    else if (valueType.IsNullableLong)
                    {
                        p.SetValue(entity, valueType.NullableLong, null);
                    }
                    else if (valueType.IsNullableShort)
                    {
                        p.SetValue(entity, valueType.NullableShort, null);
                    }
                    else if (valueType.IsNullableUint)
                    {
                        p.SetValue(entity, valueType.NullableUint, null);
                    }
                    else if (valueType.IsNullableUlong)
                    {
                        p.SetValue(entity, valueType.NullableULong, null);
                    }
                    else if (valueType.IsNullableUshort)
                    {
                        p.SetValue(entity, valueType.NullableUShort, null);
                    }
                }
                else
                {
                    try
                    {
                        this.CreateComplexObject(p, ref entity, currentDepth);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }

                }
            };
            // todo
            // var events = t.GetEvents().ToList();

            return entity;
        }

        private object CreateSimpleValueType(Type type)
        {
            if (type.GetCustomAttribute<DoNotFillAttribute>() != null)
            {
                return null;
            }

            ValueType valueType = ValueTypeFactory.Create(type);
            if (valueType.IsDouble)
            {
                return valueType.ADouble;
            }
            if (valueType.IsDecimal)
            {
                return valueType.ADecimal;

            }
            if (valueType.IsFloat)
            {
                return valueType.AFloat;

            }
            if (valueType.IsShort)
            {
                return valueType.AShort;

            }
            if (valueType.IsUint)
            {
                return valueType.AUint;

            }
            if (valueType.IsUshort)
            {
                return valueType.AUshort;

            }
            if (valueType.IsUlong)
            {
                return valueType.AUlong;

            }
            if (valueType.IsInt)
            {
                return valueType.AInt;
            }
            if (valueType.IsNullableDecimal)
            {
                return valueType.NullableDecimal;
            }
            if (valueType.IsNullableDouble)
            {
                return valueType.NullableDouble;
            }
            if (valueType.IsNullableFloat)
            {
                return valueType.NullableFloat;
            }
            if (valueType.IsNullableInt)
            {
                return valueType.NullableInt;
            }
            if (valueType.IsNullableLong)
            {
                return valueType.NullableLong;
            }
            if (valueType.IsNullableShort)
            {
                return valueType.NullableShort;
            }
            if (valueType.IsNullableUint)
            {
                return valueType.NullableUint;
            }
            if (valueType.IsNullableUlong)
            {
                return valueType.NullableULong;
            }
            if (valueType.IsNullableUshort)
            {
                return valueType.NullableUShort;
            }
            return null;
        }

        /// <summary>
        /// Creates the complex object.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="currentDepth">The current depth.</param>
        /// <exception cref="System.NotImplementedException">no default constructor</exception>
        private void CreateComplexObject(PropertyInfo propertyInfo, ref object entity, int currentDepth)
        {
            Type complexType = propertyInfo.PropertyType;
            Console.WriteLine(complexType);

            if (complexType.GetCustomAttribute<DoNotFillAttribute>() != null)
            {
                return;
            }

            if ((complexType.IsGenericType) &&
                (IsGenericEnumerable(complexType)) &&
                (!ReferenceEquals(null, complexType.FullName)) &&
                (!complexType.FullName.Contains("EntitySet")))
            {
                // execution only goes into this branch if the item being created 
                // is a collection type object, a list, array or dictionary.

                Console.WriteLine(propertyInfo.Name);
                Type[] typeArguments = complexType.GetGenericArguments();
                var obj = propertyInfo.GetValue(entity, null);

                if (ReferenceEquals(null, obj))
                {
                    if (propertyInfo.PropertyType.GetConstructor(Type.EmptyTypes) == null)
                    {
                        var x = propertyInfo.PropertyType.GetConstructors();
                        // TODO
                        throw new NotImplementedException("no default constructor");
                    }

                    obj = Activator.CreateInstance(propertyInfo.PropertyType);
                    // now assign it to the property.
                    propertyInfo.SetValue(entity, obj);
                }

                // Collection object should already be created when the main entity is created
                // so we can just get the value of the property here(an empty collection) and populate the collection
                // by invoking the "Add" method on the collection object
                MethodInfo addMethod = obj.GetType().GetMethod("Add");

                // the problem here is that we need to know if we create a complex item 
                // or just a simple one.

                if (typeArguments.Length > 1)
                {
                    var objParam1 = this.CreateAndFill(typeArguments[0]);
                    var objParam2 = this.CreateAndFill(typeArguments[1]);

                    addMethod.Invoke(obj, new[]
                    {
                        objParam1, objParam2
                    });
                }
                else
                {
                    var objParam = this.CreateAndFill(typeArguments[0]);
                    addMethod.Invoke(obj, new[]
                    {
                        objParam
                    });
                }
            }
            // big if there
            else
            {
                if (currentDepth < 2)
                {
                    var obj = this.CreateAndFill(complexType, ++currentDepth);
                    propertyInfo.SetValue(entity, obj, null);
                }
                else
                {
                    var obj = Activator.CreateInstance(complexType);
                    propertyInfo.SetValue(entity, obj, null);
                }
            }
        }

        // Checks to see if the type is a Generic type that implements IEnumerable
        private static bool IsGenericEnumerable(Type t)
        {
            var genArgs = t.GetGenericArguments();

            if ((genArgs.Length == 1) &&
                (typeof(IEnumerable<>).MakeGenericType(genArgs[0]).IsAssignableFrom(t)))
            {
                return true;
            }

            if ((genArgs.Length == 2) &&
                (typeof(IDictionary<,>).MakeGenericType(genArgs[0], genArgs[1]).IsAssignableFrom(t)))
            {
                return true;
            }

            return t.BaseType != null && IsGenericEnumerable(t.BaseType);
        }

        private static string RandomValue(int length = 0)
        {
            StringBuilder sb = new StringBuilder();

            if (length == 0)
            {
                int words = random.Next(EnglishAverages.SentenceMin, EnglishAverages.SentenceMax);

                for (int i = 0; i < words; i++)
                {
                    int wordLength = random.Next(EnglishAverages.WordMin, EnglishAverages.WordMax);
                    sb.Append((Char)random.Next(65, 90));
                    for (int x = 1; x < wordLength; x++)
                    {
                        // 97, 122
                        sb.Append((Char)random.Next(97, 122));
                    }
                    sb.Append(" ");
                }
            }

            if (length == 0)
            {
                return sb.ToString().Trim() + ".";
            }
            for (int z = 0; z < length; z++)
            {
                sb.Append((Char)random.Next(97, 122));
            }
            return sb.ToString();

        }
    }

    public static class ValueTypeFactory
    {
        private static readonly Random random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Creates the specified t.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static ValueType Create(Type t)
        {
            if (t.GetCustomAttribute<DoNotFillAttribute>() != null)
            {
                return null;
            }

            ValueType valueType = new ValueType();

            //isDouble = isDecimal = isFloat = isShort = false;
            if (t == null)
            {
                return valueType;
            }

            if (t == typeof(int))
            {
                valueType.IsValueType = true;
                valueType.IsInt = true;
                return valueType;
            }
            if (t == typeof(long))
            {
                valueType.IsValueType = true;
                valueType.IsLong = true;
                return valueType;
            }
            if (t == typeof(short))
            {
                valueType.IsValueType = true;
                valueType.IsShort = true;
                return valueType;
            }
            if (t == typeof(double))
            {
                valueType.IsValueType = true;
                valueType.IsDouble = true;
                return valueType;
            }
            if (t == typeof(decimal))
            {
                valueType.IsValueType = true;
                valueType.IsDecimal = true;
                return valueType;
            }
            if (t == typeof(ulong))
            {
                valueType.IsValueType = true;
                valueType.IsUlong = true;
                return valueType;
            }
            if (t == typeof(uint))
            {
                valueType.IsValueType = true;
                valueType.IsUint = true;
                return valueType;
            }

            if (t == typeof(ushort))
            {
                valueType.IsValueType = true;
                valueType.IsUshort = true;
                return valueType;
            }

            if (t.BaseType == typeof(Nullable))
            {
                throw new NotImplementedException();
            }
            return valueType;
        }

        /// <summary>
        /// Gets the integer.
        /// </summary>
        /// <returns></returns>
        public static int GetInteger()
        {
            return random.Next();
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <returns></returns>
        public static long GetLong()
        {
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <returns></returns>
        public static double GetDouble()
        {
            double d = random.Next();
            return d;
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <returns></returns>
        public static decimal GetDecimal()
        {
            decimal d = random.Next();
            return d;
        }

        /// <summary>
        /// Gets the float.
        /// </summary>
        /// <returns></returns>
        public static float GetFloat()
        {
            float d = random.Next();
            return d;
        }

        /// <summary>
        /// Gets the short.
        /// </summary>
        /// <returns></returns>
        public static short GetShort()
        {
            var s = (short)random.Next();
            return s;
        }

        /// <summary>
        /// Gets the uint.
        /// </summary>
        /// <returns></returns>
        public static uint GetUint()
        {
            var u = (uint)random.Next();
            return u;
        }

        /// <summary>
        /// Gets the ushort.
        /// </summary>
        /// <returns></returns>
        public static ushort GetUshort()
        {
            var u = (ushort)random.Next();
            return u;
        }

        /// <summary>
        /// Gets the ulong.
        /// </summary>
        /// <returns></returns>
        public static ulong GetUlong()
        {
            var u = (ulong)random.Next();
            return u;
        }

        /// <summary>
        /// Gets the byte.
        /// </summary>
        /// <returns></returns>
        public static byte GetByte()
        {
            var u = (byte)random.Next();
            return u;
        }

        /// <summary>
        /// Gets the sbyte.
        /// </summary>
        /// <returns></returns>
        public static sbyte GetSbyte()
        {
            var u = (sbyte)random.Next();
            return u;
        }
    }

    public sealed class ValueType
    {
        public bool IsValueType { get; set; }

        public bool IsByte { get; set; }
        public byte AByte
        {
            get { return ValueTypeFactory.GetByte(); }
        }

        public bool IsSbyte { get; set; }
        public sbyte ASbyte
        {
            get { return ValueTypeFactory.GetSbyte(); }
        }

        public bool IsChar { get; set; }
        public char AChar
        {
            get { return DataMaker.GetChar(); }
        }

        public bool IsStruct { get; set; }
        public T CreateStruct<T>()
        {
            // this might be bad lol
            return Activator.CreateInstance<T>();
        }

        public bool IsEnum { get; set; }
        public int CreateEnumValue()
        {
            return 0;
        }

        public bool IsDouble { get; set; }
        public double ADouble
        {
            get { return ValueTypeFactory.GetDouble(); }
        }

        public bool IsFloat { get; set; }
        public float AFloat
        {
            get { return ValueTypeFactory.GetFloat(); }
        }

        public bool IsShort { get; set; }
        public short AShort
        {
            get { return ValueTypeFactory.GetShort(); }
        }

        public bool IsDecimal { get; set; }
        public decimal ADecimal
        {
            get { return ValueTypeFactory.GetDecimal(); }
        }

        public bool IsUint { get; set; }
        public uint AUint
        {
            get { return ValueTypeFactory.GetUint(); }
        }

        public bool IsUshort { get; set; }
        public ushort AUshort
        {
            get { return ValueTypeFactory.GetUshort(); }
        }

        public bool IsUlong { get; set; }
        public ulong AUlong
        {
            get { return ValueTypeFactory.GetUlong(); }
        }

        public bool IsInt { get; set; }
        public int AInt
        {
            get { return ValueTypeFactory.GetInteger(); }
        }

        public bool IsLong { get; set; }
        public long ALong
        {
            get { return ValueTypeFactory.GetLong(); }
        }

        // nullables    
        public bool IsNullableDouble { get; set; }
        public double? NullableDouble
        {
            get { return ValueTypeFactory.GetDouble(); }
        }

        public bool IsNullableFloat { get; set; }
        public float? NullableFloat
        {
            get { return ValueTypeFactory.GetFloat(); }
        }

        public bool IsNullableShort { get; set; }
        public short? NullableShort
        {
            get { return ValueTypeFactory.GetShort(); }
        }

        public bool IsNullableDecimal { get; set; }
        public decimal? NullableDecimal
        {
            get { return ValueTypeFactory.GetDecimal(); }
        }

        public bool IsNullableUint { get; set; }
        public uint? NullableUint
        {
            get { return ValueTypeFactory.GetUint(); }
        }

        public bool IsNullableUshort { get; set; }
        public ushort? NullableUShort
        {
            get { return ValueTypeFactory.GetUshort(); }
        }

        public bool IsNullableUlong { get; set; }
        public ulong? NullableULong
        {
            get { return (ulong?)ValueTypeFactory.GetLong(); }
        }

        public bool IsNullableInt { get; set; }
        public int? NullableInt
        {
            get { return ValueTypeFactory.GetInteger(); }
        }

        public bool IsNullableLong { get; set; }
        public long? NullableLong
        {
            get { return ValueTypeFactory.GetLong(); }
        }

    }

    public static class AsciiTable
    {
        //    "A" =  65;
        //    "a" = 97 
        //"B" = 66; 
        //    "b" = 98;
        //"C" = 67;
        //    "c" = 99 
        //"D" = 68;
        //    "d" = 100 
        //"E" = 69;
        //    "e" = 101 
        //"F" = 70;
        //    "f" = 102 
        //"G" = 71; 
        //    "g" = 103 
        //"H" = 72; 
        //    "h" = 104 
        //"I" = 73; 
        //    "i" = 105 
        //"J" = 74;
        //    "j" = 106 
        //"K" = 75; 
        //    "k" = 107 
        //"L" = 76;
        //    "l" = 108 
        //"M" = 77;
        //    "m" = 109 
        //"N" = 78;
        //    "n" = 110 
        //"O" = 79;
        //    "o" = 111 
        //"P" = 80;
        //    "p" = 112 
        //"Q" = 81;
        //    "q" = 113 
        //"R" = 82;
        //    "r" = 114 
        //"S" = 83;
        //    "s" = 115 
        //"T" = 84; 
        //    "t" = 116 
        //"U" = 85; 
        //    "u" = 117 
        //"V" = 86;
        //    "v" = 118 
        //"W" = 87;
        //    "w" = 119 
        //"X" = 88; 
        //    "x" = 120 
        //"Y" = 89; 
        //    "y" = 121 
        //"Z" = 90; 
        //    "z" = 122

    }
}