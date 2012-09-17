namespace System
{

    using Collections;
    using Collections.Generic;
    using ComponentModel;
    using Data;
    using Globalization;
    using IO;
    using Linq;
    using Reflection;
    using Runtime.Serialization;
    using Text;
    using Text.RegularExpressions;
    using Web;
    using Xml.Linq;
    using Xml.Serialization;

    /// <summary>
    /// 	Extension methods for the root data type Object
    /// </summary>
    public static class ObjectExtension
    {
        #region Check Null

        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <param name = "obj">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(Object, Object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// </remarks>
        /// <example>
        /// 	Object someObject = GetSomeObject();
        /// 	if ( someObject.IsNull() ) { // the someObject is null // }
        /// 	else { // the someObject is not null // }
        /// </example>
        public static bool IsNull(this Object obj)
        {
            return GenericExtension.IsNull(obj);
        }

        /// <summary>
        /// 	Returns TRUE, if specified target reference is equals with null reference.
        /// 	Othervise returns FALSE.
        /// </summary>
        /// <param name = "obj">Target reference. Can be null.</param>
        /// <remarks>
        /// 	Some types has overloaded '==' and '!=' operators.
        /// 	So the code "null == ((MyClass)null)" can returns <c>false</c>.
        /// 	The most correct way how to test for null reference is using "System.Object.ReferenceEquals(Object, Object)" method.
        /// 	However the notation with ReferenceEquals method is long and uncomfortable - this extension method solve it.
        /// </remarks>
        /// <example>
        /// 	Object someObject = GetSomeObject();
        /// 	if ( someObject.IsNotNull() ) { // the someObject is not null // }
        /// 	else { // the someObject is null // }
        /// </example>
        public static bool IsNotNull(this Object obj)
        {
            return GenericExtension.IsNotNull(obj);
        }

        #endregion Check Null

        #region Check Type

        /// <summary>
        /// 	Determines whether the Object is excactly of the passed type
        /// </summary>
        /// <param name = "obj">The Object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the Object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType(this Object obj, Type type)
        {
            return obj.GetType() == type;
        }

        /// <summary>
        /// 	Determines whether the Object is exactly of the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The Object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the Object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfType<T>(this Object obj)
        {
            return obj.IsOfType(typeof(T));
        }

        /// <summary>
        /// 	Determines whether the Object is of the passed type or inherits from it.
        /// </summary>
        /// <param name = "obj">The Object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the Object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits(this Object obj, Type type)
        {
            var objType = obj.GetType();
            do
            {
                if (objType == type) return true;
                if ((objType == objType.BaseType) || (default(Type) == objType.BaseType)) return false;
                objType = objType.BaseType;
            }
            while (true);
        }

        /// <summary>
        /// 	Determines whether the Object is of the passed generic type or inherits from it.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The Object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the Object is of the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOfTypeOrInherits<T>(this Object obj)
        {
            return obj.IsOfTypeOrInherits(typeof(T));
        }

        #endregion Check Type

        #region Cast

        public static T TryCast<T>(this Object obj, out bool success)
        {
            if (obj is T)
            {
                success = true;
                return (T) obj;
            }
            success = false;
            return default(T);
        }

        public static T TryCast<T>(this Object obj)
        {
            bool success;
            return TryCast<T>(obj, out success);
        }

        /// <summary>
        /// Casts the specified Object. If the Object is null a return type can be specified.
        /// </summary>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <param name="obj">The Object being casted</param>
        /// <param name="defaultValue">The default Type.</param>
        /// <returns>returns the Object as casted type. If null the default type is returned.</returns>
        public static T Cast<T>(this Object obj, T defaultValue)
        {
            if (default(Object) == obj)
                return defaultValue;
            return (T) Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Casts the specified Object to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <param name="obj">The Object being casted</param>
        /// <returns>returns the Object as casted type.</returns>
        public static T Cast<T>(this Object obj)
        {
            if (default(Object) == obj) throw new NullReferenceException();
            return (T) Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Cast an Object to the given type. Usefull especially for anonymous types.
        /// </summary>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <param name="obj">The Object to be cast</param>
        /// <returns>
        /// the casted type or null if casting is not possible.
        /// </returns>
        public static T CastAs<T>(this Object obj) where T : class, new()
        {
            if (default(Object) != obj && (obj is T))
                return obj as T;
            return default(T);
        }

        /// <summary>
        /// 	Cast an Object to the given type. Usefull especially for anonymous types.
        /// </summary>
        /// <typeparam name = "T">The type to cast to</typeparam>
        /// <param name = "obj">The Object to case</param>
        /// <returns>
        /// 	The casted type or null if casting is not possible.
        /// </returns>
        public static T CastTo<T>(this Object obj)
        {
            if (default(Object) != obj && (obj is T))
                return (T) obj;
            return default(T);
        }

        /// <summary>
        /// 	Cast an Object to the given type. Usefull especially for anonymous types.
        /// </summary>
        /// <param name="obj">The Object to be cast</param>
        /// <param name="typeTarget">The type to cast to</param>
        /// <returns>
        /// 	the casted type or null if casting is not possible.
        /// </returns>
        public static Object DynamicCast(this Object obj, Type typeTarget)
        {
            // First, it might be just a simple situation
            //if (typeTarget.IsAssignableFrom(obj.GetType())) return obj;
            if (typeTarget.IsInstanceOfType(obj)) return obj;

            // If not, we need to find a cast operator. The operator
            // may be explicit or implicit and may be included in
            // either of the two types...
            const BindingFlags pubStatBinding = BindingFlags.Public | BindingFlags.Static;
            var originType = obj.GetType();
            var names = new[] { "op_Implicit", "op_Explicit" };

            var castMethod =
                    typeTarget.GetMethods(pubStatBinding).Union(originType.GetMethods(pubStatBinding))
                        .FirstOrDefault(
                            (item) => item.ReturnType == typeTarget
                                && item.GetParameters().Length == 1
                                && item.GetParameters()[0].ParameterType.IsAssignableFrom(originType)
                                && names.Contains(item.Name));

            if (default(MethodInfo) == castMethod)
                throw new InvalidOperationException(String.Format("No matching cast operator found from {0} to {1}.", originType.Name, typeTarget.Name));

            return castMethod.Invoke(default(Object), new[] { obj });
        }

        #endregion Cast

        #region Convert To

        /// <summary>
        /// 	Determines whether the value can (in theory) be converted to the specified target type.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The value.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can be convert to the specified target type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanConvertTo<T>(this Object obj)
        {
            if (default(Object) != obj)
            {
                var targetType = typeof(T);

                var converter = TypeDescriptor.GetConverter(obj);
                if (converter.CanConvertTo(targetType))
                    return true;

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(obj.GetType()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 	Converts an Object to the specified target type or returns the default value if
        ///     those 2 types are not convertible.
        ///     <para>Any exceptions are optionally ignored (<paramref name="ignoreException"/>).</para>
        ///     <para>
        ///     If the exceptions are not ignored and the <paramref name="obj"/> can't be convert even if
        ///     the types are convertible with each other, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The value.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <param name = "ignoreException">if set to <c>true</c> ignore any exception.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this Object obj, T defaultValue, bool ignoreException)
        {
            if (ignoreException)
            {
                try
                {
                    return obj.ConvertTo<T>();
                }
                catch
                {
                    return defaultValue;
                }
            }
            return obj.ConvertTo<T>();
        }

        /// <summary>
        /// 	Converts an Object to the specified target type or returns the default value if
        ///     those 2 types are not convertible.
        ///     <para>
        ///     If the <paramref name="obj"/> can't be convert even if the types are
        ///     convertible with each other, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The value.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this Object obj, T defaultValue)
        {
            if (default(Object) != obj)
            {
                var targetType = typeof(T);

                if (obj.GetType() == targetType) return (T) obj;

                var converter = TypeDescriptor.GetConverter(obj);
                if (converter.CanConvertTo(targetType))
                    return (T) converter.ConvertTo(obj, targetType);

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(obj.GetType()))
                    return (T) converter.ConvertFrom(obj);
            }
            return defaultValue;
        }

        /// <summary>
        /// 	Converts an Object to the specified target type or returns the default value if
        ///     those 2 types are not convertible.
        ///     <para>
        ///     If the <paramref name="obj"/> can't be convert even if the types are
        ///     convertible with each other, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this Object obj)
        {
            return obj.ConvertTo(default(T));
        }

        /// <summary>
        /// 	Converts an Object to the specified target type or returns the default value.
        ///     <para>Any exceptions are ignored. </para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The value.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The target type</returns>
        public static T ConvertToAndIgnoreException<T>(this Object obj, T defaultValue)
        {
            return obj.ConvertTo(defaultValue, true);
        }

        /// <summary>
        /// 	Converts an Object to the specified target type or returns the default value.
        ///     <para>Any exceptions are ignored. </para>
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "obj">The value.</param>
        /// <returns>The target type</returns>
        public static T ConvertToAndIgnoreException<T>(this Object obj)
        {
            return obj.ConvertToAndIgnoreException(default(T));
        }

        #endregion Convert To

        #region Assignable To

        /// <summary>
        /// 	Determines whether the Object is assignable to the passed type.
        /// </summary>
        /// <param name = "obj">The Object to check.</param>
        /// <param name = "type">The target type.</param>
        /// <returns>
        /// 	<c>true</c> if the Object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo(this Object obj, Type type)
        {
            var objType = obj.GetType();
            return type.IsAssignableFrom(objType);
        }

        /// <summary>
        /// 	Determines whether the Object is assignable to the passed generic type.
        /// </summary>
        /// <typeparam name = "T">The target type.</typeparam>
        /// <param name = "obj">The Object to check.</param>
        /// <returns>
        /// 	<c>true</c> if the Object is assignable to the specified type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableTo<T>(this Object obj)
        {
            return obj.IsAssignableTo(typeof(T));
        }

        #endregion Assignable To

        #region Dynamic Invoke Method

        /// <summary>
        /// 	Dynamically invokes a method using reflection
        /// </summary>
        /// <param name = "obj">The Object to perform on.</param>
        /// <param name = "methodName">The name of the method.</param>
        /// <param name = "parameters">The parameters passed to the method.</param>
        /// <returns>The return value</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static Object InvokeMethod(this Object obj, String methodName, params Object[] parameters)
        {
            return InvokeMethod<Object>(obj, methodName, parameters);
        }

        /// <summary>
        /// 	Dynamically invokes a method using reflection and returns its value in a typed manner
        /// </summary>
        /// <typeparam name = "T">The expected return data types</typeparam>
        /// <param name = "obj">The Object to perform on.</param>
        /// <param name = "methodName">The name of the method.</param>
        /// <param name = "parameters">The parameters passed to the method.</param>
        /// <returns>The return value</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static T InvokeMethod<T>(this Object obj, String methodName, params Object[] parameters)
        {
            var type = obj.GetType();
            var method = type.GetMethod(methodName, parameters.Select(o => o.GetType()).ToArray());

            if (default(MethodInfo) == method)
                throw new ArgumentException(String.Format("Method '{0}' not found.", methodName), methodName);

            var value = method.Invoke(obj, parameters);
            return (value is T) ? (T) value : default(T);
        }

        #endregion Dynamic Invoke Method

        #region Dynamic Property

        /// <summary>
        /// 	Dynamically retrieves a property value.
        /// </summary>
        /// <typeparam name = "T">The expected return data type</typeparam>
        /// <param name = "obj">The Object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <param name = "defaultValue">The default value to return.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static T GetPropertyValue<T>(this Object obj, String propertyName, T defaultValue)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(propertyName);

            if (default(PropertyInfo) == propertyInfo)
                throw new ArgumentException(String.Format("Property '{0}' not found.", propertyName), propertyName);

            var value = propertyInfo.GetValue(obj, default(Object[]));

            return (value is T) ? (T) value : defaultValue;
        }

        /// <summary>
        /// 	Dynamically retrieves a property value.
        /// </summary>
        /// <param name = "obj">The Object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static Object GetPropertyValue(this Object obj, String propertyName)
        {
            return GetPropertyValue(obj, propertyName, default(Object));
        }

        /// <summary>
        /// 	Dynamically retrieves a property value.
        /// </summary>
        /// <typeparam name = "T">The expected return data type</typeparam>
        /// <param name = "obj">The Object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// 	<code>
        /// 		var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// 		var file = type.CreateInstance(@"c:\autoexec.bat");
        /// 		if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        /// 		var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        /// 		Console.WriteLine(reader.ReadToEnd());
        /// 		reader.Close();
        /// 		}
        /// 	</code>
        /// </example>
        public static T GetPropertyValue<T>(this Object obj, String propertyName)
        {
            return GetPropertyValue(obj, propertyName, default(T));
        }

        /// <summary>
        /// 	Dynamically sets a property value.
        /// </summary>
        /// <param name = "obj">The Object to perform on.</param>
        /// <param name = "propertyName">The Name of the property.</param>
        /// <param name = "value">The value to be set.</param>
        public static void SetPropertyValue(this Object obj, String propertyName, Object value)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(propertyName);

            if (default(PropertyInfo) == propertyInfo)
                throw new ArgumentException(String.Format("Property '{0}' not found.", propertyName), propertyName);
            if (!propertyInfo.CanWrite)
                throw new ArgumentException(String.Format("Property '{0}' does not allow writes.", propertyName), propertyName);
            propertyInfo.SetValue(obj, value, default(Object[]));
        }

        #endregion Dynamic Property

        #region Attribute

        /// <summary>
        /// 	Gets all matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The Object to look on.</param>
        /// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
        /// <returns>The found attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this Object obj, bool includeInherited) where T : Attribute
        {
            return ((obj as Type) ?? obj.GetType()).GetCustomAttributes(typeof(T), includeInherited).OfType<T>().Select(attribute => attribute);
        }

        /// <summary>
        /// 	Gets all matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The Object to look on.</param>
        /// <returns>The found attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this Object obj) where T : Attribute
        {
            return GetAttributes<T>(obj, false);
        }

        /// <summary>
        /// 	Gets the first matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The Object to look on.</param>
        /// <param name = "includeInherited">if set to <c>true</c> includes inherited attributes.</param>
        /// <returns>The found attribute</returns>
        public static T GetAttribute<T>(this Object obj, bool includeInherited) where T : Attribute
        {
            var type = (obj as Type ?? obj.GetType());
            var attributes = type.GetCustomAttributes(typeof(T), includeInherited);
            return attributes.FirstOrDefault() as T;
        }

        /// <summary>
        /// 	Gets the first matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name = "T">The attribute type tp look for.</typeparam>
        /// <param name = "obj">The Object to look on.</param>
        /// <returns>The found attribute</returns>
        public static T GetAttribute<T>(this Object obj) where T : Attribute
        {
            return GetAttribute<T>(obj, true);
        }

        #endregion Attribute

        #region As String

        /// <summary>
        /// 	If target is null, returns null.
        /// 	Othervise returns String representation of target using current culture format provider.
        /// </summary>
        /// <param name = "obj">Target transforming to String representation. Can be null.</param>
        /// <example>
        /// 	float? number = null;
        /// 	String text1 = number.AsString();
        ///
        /// 	number = 15.7892;
        /// 	String text2 = number.AsString();
        /// </example>
        public static String AsString(this Object obj)
        {
            return ReferenceEquals(obj, default(Object)) ? default(String) : obj.ToString();
        }

        /// <summary>
        /// 	If target is null, returns null.
        /// 	Othervise returns String representation of target using specified format provider.
        /// </summary>
        /// <param name = "obj">Target transforming to String representation. Can be null.</param>
        /// <param name = "formatProvider">Format provider used to transformation target to String representation.</param>
        /// <example>
        /// 	CultureInfo czech = new CultureInfo("cs-CZ");
        ///
        /// 	float? number = null;
        /// 	String text1 = number.AsString( czech );
        ///
        /// 	number = 15.7892;
        /// 	String text2 = number.AsString( czech );
        /// </example>
        public static String AsString(this Object obj, IFormatProvider formatProvider)
        {
            return String.Format(formatProvider, "{0}", obj);
        }

        /// <summary>
        /// 	If target is null, returns null.
        /// 	Othervise returns String representation of target using invariant format provider.
        /// </summary>
        /// <param name = "obj">Target transforming to String representation. Can be null.</param>
        /// <example>
        /// 	float? number = null;
        /// 	String text1 = number.AsInvariantString();
        ///
        /// 	number = 15.7892;
        /// 	String text2 = number.AsInvariantString();
        /// </example>
        public static String AsInvariantString(this Object obj)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}", obj);
        }

        #endregion As String

        #region String Dump

        public static IEnumerable<String> ToStringDumpInternal(XContainer xContainer)
        {
            foreach (var xElement in xContainer.Elements().OrderBy(o => o.Name.ToString()))
            {
                if (xElement.HasElements)
                {
                    foreach (var val in ToStringDumpInternal(xElement))
                        yield return "{" + String.Format("{0}={1}", xElement.Name, val) + "}";
                }
                else
                    yield return "{" + String.Format("{0}={1}", xElement.Name, xElement.Value) + "}";
            }
        }

        /// <summary>
        /// 	get a String representation of a given Object.
        /// </summary>
        /// <param name = "obj">the Object to dump</param>
        /// <param name = "flags">BindingFlags to use for reflection</param>
        /// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
        /// <returns></returns>
        public static String ToStringDump(this Object obj, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
        {
            return ToStringDumpInternal(obj.ToXElement(flags, maxArrayElements)).Aggregate(new StringBuilder(), (sb, elem) => sb.Append(elem)).ToString();
        }

        #endregion String Dump

        #region ToXElement

        /// <summary>
        /// 	get a XElement representation of a given Object.
        /// </summary>
        /// <param name = "obj">the Object to dump</param>
        /// <param name = "flags">BindingFlags to use for reflection</param>
        /// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
        /// <returns></returns>
        public static XElement ToXElement(this Object obj, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
        {
            try
            {
                return ToXElementInternal(obj, new HashSet<Object>(), flags, maxArrayElements);
            }
            catch
            {
                return new XElement(obj.GetType().Name);
            }
        }

        // TODO:: Please document these methods
        public static XElement ToXElementInternal(Object obj, ICollection<Object> visited, BindingFlags flags, int maxArrayElements)
        {
            if (default(Object) == obj) return new XElement("null");
            if (visited.Contains(obj)) return new XElement("Cyclic Reference");

            if (!obj.GetType().IsValueType) visited.Add(obj);

            var type = obj.GetType();
            var xelem = new XElement(CleanName(type.Name, type.IsArray));

            if (!NeedRecursion(obj, type))
            {
                xelem.Add(new XElement(CleanName(type.Name, type.IsArray), String.Empty + obj));
                return xelem;
            }

            if (obj is IEnumerable)
            {
                var i = 0;
                foreach (var elem in obj as IEnumerable)
                {
                    var subtype = elem.GetType();
                    xelem.Add(NeedRecursion(elem, subtype) ? ToXElementInternal(elem, visited, flags, maxArrayElements) : new XElement(CleanName(subtype.Name, subtype.IsArray), elem));
                    if (i++ >= maxArrayElements)
                        break;
                }
                return xelem;
            }

            foreach (var propertyInfo in type.GetProperties(flags).Where(propertyInfo => propertyInfo.CanRead))
            {
                var value = GetValue(obj, propertyInfo);
                xelem.Add(NeedRecursion(value, propertyInfo.PropertyType)
                                ? new XElement(CleanName(propertyInfo.Name, propertyInfo.PropertyType.IsArray), ToXElementInternal(value, visited, flags, maxArrayElements))
                                : new XElement(CleanName(propertyInfo.Name, propertyInfo.PropertyType.IsArray), String.Empty + value));
            }

            foreach (var fieldInfo in type.GetFields())
            {
                var value = fieldInfo.GetValue(obj);
                xelem.Add(NeedRecursion(value, fieldInfo.FieldType)
                                ? new XElement(CleanName(fieldInfo.Name, fieldInfo.FieldType.IsArray), ToXElementInternal(value, visited, flags, maxArrayElements))
                                : new XElement(CleanName(fieldInfo.Name, fieldInfo.FieldType.IsArray), String.Empty + value));
            }
            return xelem;
        }

        public static bool NeedRecursion(Object obj, Type type)
        {
            return default(Object) != obj && (!type.IsPrimitive && !(obj is String || obj is DateTime || obj is DateTimeOffset || obj is TimeSpan || obj is Delegate || obj is Enum || obj is Decimal || obj is Guid));
        }

        public static Object GetValue(Object obj, PropertyInfo propertyInfo)
        {
            Object value;
            try
            {
                value = propertyInfo.GetValue(obj, default(Object[]));
            }
            catch
            {
                try
                {
                    value = propertyInfo.GetValue(obj, new Object[] { 0 });
                }
                catch
                {
                    value = default(Object);
                }
            }
            return value;
        }

        public static String CleanName(IEnumerable<char> name, bool isArray)
        {
            var sb = new StringBuilder();

            foreach (var c in name.Where(c => Char.IsLetterOrDigit(c) && c != '`').Select(c => c))
                sb.Append(c);

            if (isArray)
                sb.Append("Array");
            return sb.ToString();
        }

        #endregion ToXElement

        #region CopyPropertiesFrom

        /// <summary>
        /// Copies the readable and writable public property values from the source Object to the target
        /// </summary>
        /// <remarks>The source and target Objects must be of the same type.</remarks>
        /// <param name="target">The target Object</param>
        /// <param name="source">The source Object</param>
        /// <param name="arrPropIgnore">An array of property names to ignore</param>
        public static void CopyPropertiesFrom(this Object target, Object source, String[] arrPropIgnore)
        {
            // Get and check the Object types
            var type = source.GetType();
            if (target.GetType() != type)
            {
                throw new ArgumentException("The source type must be the same as the target");
            }

            // Build a clean list of property names to ignore
            var lstIgnore = new List<String>();
            //foreach (var item in arrPropIgnore)
            //    if (item.IsNotEmpty() && !lstIgnore.Contains(item))
            //        lstIgnore.Add(item);
            foreach (var item in arrPropIgnore.Where((item) => item.IsNotNullOrEmpty() && !lstIgnore.Contains(item)))
                lstIgnore.Add(item);

            // Copy the properties
            foreach (var property in type.GetProperties())
            {
                if (!(property.CanWrite && property.CanRead) || lstIgnore.Contains(property.Name)) continue;

                var value = property.GetValue(source, default(Object[]));
                property.SetValue(target, value, default(Object[]));
            }
        }

        /// <summary>
        /// Copies the readable and writable public property values from the source Object to the target
        /// </summary>
        /// <remarks>The source and target Objects must be of the same type.</remarks>
        /// <param name="target">The target Object</param>
        /// <param name="source">The source Object</param>
        /// <param name="arrPropIgnore">A single property name to ignore</param>
        public static void CopyPropertiesFrom(this Object target, Object source, String arrPropIgnore)
        {
            CopyPropertiesFrom(target, source, new[] { arrPropIgnore });
        }

        /// <summary>
        /// Copies the readable and writable public property values from the source Object to the target
        /// </summary>
        /// <remarks>The source and target Objects must be of the same type.</remarks>
        /// <param name="target">The target Object</param>
        /// <param name="source">The source Object</param>
        public static void CopyPropertiesFrom(this Object target, Object source)
        {
            CopyPropertiesFrom(target, source, String.Empty);
        }

        #endregion CopyPropertiesFrom

        #region PropertiesString

        /// <summary>
        /// Returns a String representation of the Objects property values
        /// </summary>
        /// <param name="obj">The Object for the String representation</param>
        /// <param name="delimiter">The line terminstor String to use between properties</param>
        /// <returns>A String</returns>
        public static String ToPropertiesString(this Object obj, String delimiter)
        {
            if (default(Object) == obj) return String.Empty;
            var type = obj.GetType();
            var sb = new StringBuilder(type.Name);
            sb.Append(delimiter);

            foreach (var property in type.GetProperties())
            {
                if (!property.CanWrite || !property.CanRead) continue;

                var value = property.GetValue(obj, default(Object[]));
                sb.Append(property.Name);
                sb.Append(": ");
                sb.Append((default(Object) == value) ? "[NULL]" : value.ToString());
                sb.Append(delimiter);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a String representation of the Objects property values
        /// </summary>
        /// <param name="obj">The Object for the String representation</param>
        /// <returns>A String</returns>
        public static String ToPropertiesString(this Object obj)
        {
            return ToPropertiesString(obj, Environment.NewLine);
        }

        #endregion PropertiesString

        #region ToXml

        /// <summary>
        /// Serializes the Object into an XML String
        /// </summary>
        /// <remarks>
        /// The Object to be serialized should be decorated with the
        /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
        /// </remarks>
        /// <param name="obj">The Object to serialize</param>
        /// <param name="encoding">The Encoding scheme to use when serializing the data to XML</param>
        /// <returns>An XML encoded String representation of the Object</returns>
        public static String ToXml(this Object obj, Encoding encoding)
        {
            if (default(Object) == obj) throw new ArgumentException("The Object cannot be null.");
            if (default(Encoding) == encoding) throw new Exception("You must specify an encoder to use for serialization.");
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                return encoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Serializes the Object into an XML String, using the encoding method specified in
        /// <see>
        ///     <cref>ExtensionMethodsSettings.DefaultEncoding</cref>
        /// </see>
        /// </summary>
        /// <remarks>
        /// The Object to be serialized should be decorated with the
        /// <see cref="SerializableAttribute"/>, or implement the <see cref="ISerializable"/> interface.
        /// </remarks>
        /// <param name="obj">The Object to serialize</param>
        /// <returns>An XML encoded String representation of the Object</returns>
        public static String ToXml(this Object obj)
        {
            return ToXml(obj, ExtensionMethodSetting.DefaultEncoding);
        }

        #endregion ToXml

        #region Exception If Null

        /// <summary>
        /// Throws an <see cref="System.ArgumentNullException"/>
        /// if the the value is null.
        /// </summary>
        /// <param name="obj">The value to test.</param>
        /// <param name="message">The message to display if the value is null.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        public static void ExceptionIfNull(this Object obj, String message, String paramName)
        {
            if (default(Object) == obj) throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentNullException"/>
        /// if the the value is null.
        /// </summary>
        /// <param name="obj">The value to test.</param>
        /// <param name="paramName">The name of the parameter being tested.</param>
        public static void ExceptionIfNull(this Object obj, String paramName)
        {
            if (default(Object) == obj) throw new ArgumentNullException(paramName);
        }

        #endregion Exception If Null

        /// <summary>
        /// 	get a html-table representation of a given Object.
        /// </summary>
        /// <param name = "obj">the Object to dump</param>
        /// <param name = "flags">BindingFlags to use for reflection</param>
        /// <param name = "maxArrayElements">Number of elements to show for IEnumerables</param>
        /// <returns></returns>
        public static String ToHtmlTable(this Object obj, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, int maxArrayElements = 5)
        {
            return ToHtmlTableInternal(obj.ToXElement(flags, maxArrayElements), 0).Aggregate(String.Empty, (str, el) => str + el);
        }

        public static IEnumerable<String> ToHtmlTableInternal(XContainer xContainer, int padding)
        {
            yield return FormatHtmlLine("<table>", padding);
            yield return FormatHtmlLine("<tr><th>Attribute</th><th>Value</th></tr>", padding + 1);
            foreach (var xElement in xContainer.Elements().OrderBy(o => o.Name.ToString()))
            {
                if (xElement.HasElements)
                {
                    yield return FormatHtmlLine(String.Format("<tr><td>{0}</td><td>", xElement.Name), padding + 1);
                    foreach (var elem in ToHtmlTableInternal(xElement, padding + 2))
                        yield return elem;
                    yield return FormatHtmlLine("</td></tr>", padding + 1);
                }
                else
                    yield return FormatHtmlLine(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", xElement.Name, HttpUtility.HtmlEncode(xElement.Value)), padding + 1);
            }
            yield return FormatHtmlLine("</table>", padding);
        }

        public static String FormatHtmlLine(String tag, int padding)
        {
            return String.Format("{0}{1}{2}", String.Empty.PadRight(padding, '\t'), tag, Environment.NewLine);
        }

        public static bool InList<T>(this Object obj, IEnumerable<T> sequence)
        {
            //foreach (T item in sequence)
            //    if (item.Equals(obj))
            //        return true;
            //return false;

            return sequence.Any(item => item.Equals(obj));
        }

        public static String Format(this Object obj, String format)
        {
            var formatSpec = new Regex(@"{(?<name>\S+?)(:(?<format>.*?))?}");

            var matches = formatSpec.Matches(format);
            if (matches.Count == 0)
                return format;

            var sb = new StringBuilder();

            var values = new Object[matches.Count];
            var type = obj.GetType();

            var start = 0;
            for (var i = 0; i < matches.Count; ++i)
            {
                var match = matches[i];
                var propName = match.Groups["name"].Value;
                var propFormat = match.Groups["format"].Value;

                var propertyInfo = type.GetProperty(propName);
                if (default(PropertyInfo) == propertyInfo) throw new FormatException(String.Format(CultureInfo.CurrentUICulture, "Unknown property {0}.", propName));

                values[i] = propertyInfo.GetValue(obj, default(Object[]));
                sb.Append(format.Substring(start, match.Index - start));
                sb.Append("{");
                sb.Append(i.ToString(CultureInfo.InvariantCulture));
                if (!String.IsNullOrEmpty(propFormat))
                    sb.AppendFormat(":{0}", propFormat);
                sb.Append("}");

                start = match.Index + match.Length;
            }
            sb.Append(format.Substring(start));

            return String.Format(CultureInfo.CurrentUICulture, sb.ToString(), values);
        }

        public static DataSet ToDataSet(this Object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                var dataSet = new DataSet();
                using (var reader = new StringReader(writer.ToString()))
                    dataSet.ReadXml(reader);
                return dataSet;
            }
        }
    }
}