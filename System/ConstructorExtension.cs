namespace System
{
    using ComponentModel;
    using Data;
    using Diagnostics;
    using Linq.Expressions;
    using Reflection;

    public static class ConstructorExtension
    {
        /// <summary>
        ///   Auto Initialize an Object source with a Object data for all properties specified by the binding flags.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The source Object to have its' properties initialized. </param>
        /// <param name="data"> The data Object used to initialize the source Object. </param>
        /// <param name="flags"> The binding flags used for property accessability. See <see cref="BindingFlags" /> </param>
        public static void AutoInitialize<T>(this T self, Object data, BindingFlags flags)
        {
            // Check if data is not null
            if (default(Object) == data) return;
            // Check that data is the same type as source
            if (data.GetType() != self.GetType()) return;
            // Get all the public - instace properties that contains both getter and setter.
            var properties = self.GetType().GetProperties(flags);
            // For each property, set the value to the source from the data.
            foreach (var property in properties)
            {
                // Identify the type of this property.
                var propertyType = property.PropertyType;
                try
                {
                    // Retreive the value given the property name.
                    var objValue = property.GetValue(data, default(Object[]));
                    if (default(Object) == objValue) continue;
                    // If the Object value is already of the property type
                    if (objValue.GetType() == propertyType) // Set the Object value to the source
                        property.SetValue(self, objValue, default(Object[]));
                    else
                    {
                        // Otherwise convert the Object value using the property's converter
                        var converter = TypeDescriptor.GetConverter(propertyType);
                        // Convert the Object value.
                        var convertedValue = converter.ConvertFrom(objValue);
                        // Check that the converted data is of the same type as the property type
                        if (default(Object) == convertedValue) continue;
                        // If it is, then set the converted data to the source Object.
                        if (convertedValue.GetType() == propertyType) property.SetValue(self, convertedValue, default(Object[]));
                    }
                }
                catch (Exception exp)
                {
                    // Exception during operations
                    Debug.WriteLine(exp.Message);
                }
            }
        }

        /// <summary>
        ///   Auto initialize an Object source with an Object data for all public accessible properties.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The source Object to have its' properties initialized. </param>
        /// <param name="data"> The data Object used to initialize the source Object. </param>
        public static void AutoInitialize<T>(this T self, Object data)
        {
            const BindingFlags flags =
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;
            self.AutoInitialize(data, flags);
        }

        /// <summary>
        ///   Auto initialize all properties specified by the binding flags of the source Object with data from a data row.
        /// </summary>
        /// <typeparam name="T"> The type of the source. </typeparam>
        /// <param name="self"> The Object to be auto initialized. </param>
        /// <param name="dataRow"> The data row containing the data for initializing. </param>
        /// <param name="flags"> The binding flags used for property accessability. See <see cref="BindingFlags" /> </param>
        /// <param name="columns"> An expression to specify the columns. </param>
        public static void AutoInitialize<T>(this T self, DataRow dataRow, BindingFlags flags,
                                             Expression<Func<T, Object>>[] columns)
        {
            if (default(DataRow) == dataRow || default(Expression<Func<T, Object>>[]) == columns) return;
            // Get all the public - instace properties that contains a setter.
            var sourceType = self.GetType();
            foreach (var col in columns)
            {
                // Get the column name from the column or use the property name.
                var columnName = GetProperty(col).Name;
                // Get the property given the column name.
                var property = sourceType.GetProperty(columnName, flags);
                // If the property is found.
                if (default(PropertyInfo) == property) continue;
                // Get the property type.
                var propertyType = property.PropertyType;
                // Retreive the row value given the column name.
                var rowValue = dataRow[columnName];
                // Determin that the row value is not null (DBNull)
                if (Convert.DBNull != rowValue)
                {
                    // Get the converter for this property.
                    var converter = TypeDescriptor.GetConverter(propertyType);
                    // Convert the row value to the property type.
                    var data = converter.ConvertFrom(rowValue);
                    if (default(Object) == data) continue;
                    // If the converted type matches the property type, then set the data to the source.
                    if (data.GetType() == propertyType) property.SetValue(self, data, default(Object[]));
                }
            }
        }

        /// <summary>
        ///   Auto initialize all public properties of the source Object with data from a data row.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The Object to be auto initialized. </param>
        /// <param name="dataRow"> The data row containing the data for initializing. </param>
        /// <param name="columns"> The columns. </param>
        public static void AutoInitialize<T>(this T self, DataRow dataRow, Expression<Func<T, Object>>[] columns)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
            self.AutoInitialize(dataRow, flags, columns);
        }

        /// <summary>
        ///   Auto initialize all properties specified by the binding flags of the source Object with data from a data row.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The Object to be auto initialized. </param>
        /// <param name="dataRow"> The data row containing the data for initializing. </param>
        /// <param name="flags"> The binding flags used for property accessability. See <see cref="BindingFlags" /> </param>
        /// <remarks>
        ///   Contributed by Tri Tran, http://about.me/triqtran
        /// </remarks>
        public static void AutoInitialize<T>(this T self, DataRow dataRow, BindingFlags flags)
        {
            if (default(DataRow) != dataRow)
            {
                // Get all the public - instace properties that contains a setter.
                var properties = self.GetType().GetProperties(flags);
                foreach (var property in properties)
                {
                    // Get the column name from the column or use the property name.
                    var columnName = property.Name;
                    // Get the property type.
                    var propertyType = property.PropertyType;
                    try
                    {
                        // Retreive the row value given the column name.
                        var rowValue = dataRow[columnName];
                        // Determin that the row value is not null (DBNull)
                        if (Convert.DBNull != rowValue)
                        {
                            // Get the converter for this property.
                            var converter = TypeDescriptor.GetConverter(propertyType);
                            // Convert the row value to the property type.
                            var data = converter.ConvertFrom(rowValue);
                            if (default(Object) == data) continue;
                            // If the converted type matches the property type, then set the data to the source.
                            if (data.GetType() == propertyType) property.SetValue(self, data, default(Object[]));
                        }
                    }
                    catch (Exception exp)
                    {
                        // Exception during operation
                        // Most likely that the row does not contain the property name.
                        Debug.WriteLine(exp.Message);
                    }
                }
            }
        }

        /// <summary>
        ///   Auto initialize all public properties of the source Object with data from a data row.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="self"> The Object to be auto initialized. </param>
        /// <param name="dataRow"> The data row containing the data for initializing. </param>
        /// <remarks>
        ///   Contributed by Tri Tran, http://about.me/triqtran
        /// </remarks>
        public static void AutoInitialize<T>(this T self, DataRow dataRow)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
            self.AutoInitialize(dataRow, flags);
        }

        /// <summary>
        ///   Get the property info from the property expression.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="propertyExpression"> </param>
        /// <returns> </returns>
        /// <remarks>
        ///   Contributed by Tri Tran, http://about.me/triqtran
        /// </remarks>
        public static PropertyInfo GetProperty<T>(Expression<Func<T, Object>> propertyExpression)
        {
            var lambda = propertyExpression as LambdaExpression;
            var memberExpression = default(MemberExpression);
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                if (null != unaryExpression) memberExpression = (MemberExpression) unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression) lambda.Body;
            return (null != memberExpression)
                       ? memberExpression.Member as PropertyInfo
                       : default(PropertyInfo);
        }
    }
}