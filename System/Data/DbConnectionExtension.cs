using System.Linq;

namespace System.Data
{
    /// <summary>
    /// Extensions for IDbConnection
    /// </summary>
    public static class DbConnectionExtension
    {

        /// <summary>
        /// Returns true if the database connection is in one of the states received.
        /// </summary>
        public static bool StateIsWithin(this IDbConnection connection, params ConnectionState[] states)
        {
            return (default(IDbConnection) != connection
                && (states != null && states.Length > 0)
                && (states.Where(s => (connection.State & s) == s).Any()));
        }

        /// <summary>
        /// Returns true if the database connection is in the specified state.
        /// </summary>
        public static bool IsInState(this IDbConnection connection, ConnectionState state)
        {
            return (default(IDbConnection) != connection
                && (connection.State & state) == state);
        }

        /// <summary>
        /// Open the Database connection if not already opened.
        /// </summary>
        public static void OpenIfNot(this IDbConnection connection)
        {
            if (default(IDbConnection) == connection) return;
            if (!connection.IsInState(ConnectionState.Open))
                connection.Open();
        }

        public static void CloseIfNot(this IDbConnection connection)
        {
            if (default(IDbConnection) == connection) return;
            if (!connection.IsInState(ConnectionState.Closed))
                connection.Close();
        }


    }
}