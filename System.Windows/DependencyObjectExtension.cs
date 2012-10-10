namespace System.Windows
{
    using Collections.Generic;
    using Media;


    public static class DependencyObjectExtension
    {
        /// <summary>
        /// Finds immediate parent of the child control 
        /// </summary>
        /// <typeparam name="T">Finds specific Type of parent control</typeparam>
        /// <param name="child">Child control in use</param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (null == parentObject) return null;

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }

        /// <summary>
        /// Finds child of specific type of specific name
        /// </summary>
        /// <typeparam name="T">Type of child</typeparam>
        /// <param name="parent">Current parent control</param>
        /// <param name="childName">Name of the child control to be found</param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.  
            if (null == parent) return null;
            T foundChild = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // If the child is not of the request child type child  
                var childType = child as T;
                if (null == childType)
                {
                    // recursively drill down the tree  
                    foundChild = FindChild<T>(child, childName);
                    // If the child is found, break so we do not overwrite the found child.    
                    if (foundChild != null) break;
                }
                else if (childName.IsNotNullOrEmpty())
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search    
                    if (null != frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name  
                        foundChild = (T) child;
                        break;
                    }
                }
                else
                {
                    // child control found.
                    foundChild = (T) child;
                    break;
                }
            }
            return foundChild;
        }

        /// <summary>
        /// Get List of child controls of specific types
        /// </summary>
        /// <typeparam name="T">Type of controls to be fetched</typeparam>
        /// <param name="parent">Current parent</param>
        /// <returns></returns>
        public static List<T> GetVisualChildList<T>(this DependencyObject parent) where T : DependencyObject
        {
            var lstVisualChild = new List<T>();
            GetVisualChildList(parent, lstVisualChild);
            return lstVisualChild;
        }


        private static void GetVisualChildList<T>(DependencyObject parent, List<T> lstVisualChild)
            where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    lstVisualChild.Add(child as T);
                }
                else if (child != null)
                {
                    GetVisualChildList(child, lstVisualChild);
                }
            }
        }
    }

}