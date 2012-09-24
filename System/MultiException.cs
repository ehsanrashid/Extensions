namespace System
{
    using Collections;
    using Text;

    /*
     *   * Problem
         I had been working on a base class for reading and writing to a database.
         I knew that when a new object (and hence record) was created there needed to be a mechanism for setting default values.
         I also knew that even with the default values there might not be enough information that the object could be saved.
         For example: In a contact database, a new contact would by default have a blank name and blank phone number.
         Trying to save such a contact should not succeed and instead notify the user about the failure.
         Too many applications and web pages do this one failure at a time.
         I wanted to be able to cover all of the failures in one step.
         This presents an interesting problem in any user interface.
         How can an object notify the environment about multiple failures?
         The standard way to notify the environment about any failure is to throw an exception.
         But I wanted to throw multiple exceptions and have them all be handled at one time.
         So was born the idea of a MultiException object.

         * Solution
         The MultiException object should itself act as an Exception, but it should also act as a Collection.
         This way, when a save occurs, the data class has the ability to add exceptions to the MultiException and then throw the MultiException.
         The application can then catch the MultiException and enumerate all of the Exceptions displaying the reason for each one to the end user.

         * Implementation
         The MultiException class turned out to be more of a chore then I originally thought.
         The easiest way to create a class that acts as a collection is to base it on the CollectionBase class.
         The problem with this approach was that I wanted MultiException to be based on ApplicationException and .Net only allows us one base class.
         .Net does, however, allow for a class to be based on one class and multiple interfaces.
         MultiException was created based on Exception but also implements IList. Being based on ApplicationException means that MultiException can be thrown as an exception.
         Implementing IList means that new exceptions can be added, gives us the ability to use indexes ( the [ ] operator ), and even allows us to use foreach loops!
         Basing the class on ApplicationException was the easy part. Implementing IList was a bit more vague.
         It's not that implementing an interface is difficult, it's just that I didn't want to expose the IList interface. Why not, you ask? Because the IList interface is far too generic.
 
         * Difficulties
         Notice how you don't even mark the prefixed (IList) version public or private.
         C# understands that the prefixed version is there only to suffice the interface which makes this method operate in a special way.
         The prefixed version is used while the object is cast as an IList, but isn't even available while the object is a standard MultiException.
 
         * Note
         I use MultiException any time I have more then one error to report, and hiding implementation members is a hard-to-find trick. I hope you'll be able to use both in your future projects.
 
         *  * Usage
 
            try
            {
                // Create MultiException
                MultiException ME = new MultiException();

                // Add some exceptions. They do not all have
                // to be System.Exception only, but can be
                // derrived from System.Exception.
                ME.Add(new Exception("A vague error occured"));
                ME.Add(new ArgumentException("Field is not optional", "FirstName"));
                ME.Add(new ArgumentException("Value must be supplied", "LastName"));
                ME.Add(new ArgumentNullException("PhoneNum"));

                // Trow the exception
                throw ME; // No pun intended...
            }
            catch (MultiException E)
            {
                // Display Error
                MessageBox.Show(E.Message);
                MessageBox.Show("See Output Window for additional information.");

                // Dump Additional Info
                foreach (Exception ex in E)
                {
                    Debug.WriteLine("Error Type: " + ex.GetType().ToString() + " Error Message: " + ex.Message);
                }
            }
     */

    using Collections.Generic;

    /// <summary>
    /// MultiException is based on Exception but also implements the IList<Exception/> interface.
    /// </summary>
    public class MultiException : Exception, IList<Exception>
    {
        public List<Exception> Exceptions { get; private set; }

        public MultiException()
        {
            Exceptions = new List<Exception>();
        }

        #region Properties

        public Exception this[int index]
        {
            get { return Exceptions[index]; }
            set
            {
                if (default(Exception) == value) throw new ArgumentNullException();
                Exceptions[index] = value;
            }
        }

        public override string Message
        {
            get
            {
                // If no exceptions were found, return stock message
                if (Exceptions.Count == 0)
                {
                    return
                        "A MultiException was thrown but no internal exceptions were available to provide additional information.";
                }
                // Otherwise, build the message based on member messages
                var sbMsg = new StringBuilder();
                for (var i = 0; i < Exceptions.Count; ++i)
                {
                    var exp = Exceptions[i];
                    sbMsg.Append(exp.Message);
                    if (i < (Exceptions.Count - 1)) sbMsg.AppendLine();
                }
                return sbMsg.ToString();
            }
        }

        #endregion

        public void Add(Exception exp)
        {
            Exceptions.Add(exp);
        }

        public void AddRange(IEnumerable<Exception> enumerable)
        {
            Exceptions.AddRange(enumerable);
        }

        public void AddRange(params Exception[] arrExp)
        {
            Exceptions.AddRange(arrExp);
        }

        public bool Contains(Exception exp)
        {
            return Exceptions.Contains(exp);
        }

        public bool Remove(Exception exp)
        {
            return Exceptions.Remove(exp);
        }

        // Throw an Exception if object is not an Exception
        void ValidateType(Object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(Exception))) throw new ArgumentException("Value must be of type Exception");
        }

        public bool IsReadOnly { get { return false; } }

        public void Clear()
        {
            Exceptions.Clear();
        }

        #region IList<Exception> Members

        public int IndexOf(Exception exp)
        {
            return Exceptions.IndexOf(exp);
        }

        public void Insert(int index, Exception exp)
        {
            Exceptions.Insert(index, exp);
        }

        public void RemoveAt(int index)
        {
            Exceptions.RemoveAt(index);
        } 

        #region ICollection Members

        public int Count
        {
            get { return Exceptions.Count; }
        }

        public void CopyTo(Exception[] array, int index)
        {
            Exceptions.CopyTo(array, index);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator<Exception> IEnumerable<Exception>.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator<Exception>;
        }

        public IEnumerator GetEnumerator()
        {
            return Exceptions.GetEnumerator();
        }

        #endregion

        #endregion
    }
}