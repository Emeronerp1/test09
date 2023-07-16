using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.Common
{
    public sealed class SqlHelper
    {
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
                // Do nothing if we get no data    
                return;

            // Set the parameters values

            int i = 0;
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if ((commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1))
                    throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i = i + 1;
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            int i;
            int j;

            if ((commandParameters == null) && (parameterValues == null))
                // Do nothing if we get no data
                return;

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
                throw new ArgumentException("Parameter count does not match Parameter Value count.");

            // Value array
            j = commandParameters.Length - 1;
            var loopTo = j;
            for (i = 0; i <= loopTo; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if ((paramInstance.Value == null))
                        commandParameters[i].Value = DBNull.Value;
                    else
                        commandParameters[i].Value = paramInstance.Value;
                }
                else if ((parameterValues[i] == null))
                    commandParameters[i].Value = DBNull.Value;
                else
                    commandParameters[i].Value = parameterValues[i];
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if ((command == null))
                throw new ArgumentNullException("command");
            if ((commandText == null || commandText.Length == 0))
                throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
                mustCloseConnection = false;

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;
            command.CommandTimeout = 0;
            // If we were provided a transaction, assign it.
            if (!(transaction == null))
            {
                if (transaction.Connection == null)
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (!(commandParameters == null))
                AttachParameters(command, commandParameters);
            return;
        } // PrepareCommand

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if ((command == null))
                throw new ArgumentNullException("command");
            if ((!(commandParameters == null)))
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if ((!(p == null)))
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                            p.Value = DBNull.Value;
                        command.Parameters.Add(p);
                    }
                }
            }
        } // Atta

        #region "ExecuteDataset"
        public static DataSet ExecuteDataset(string ConnectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(ConnectionString, commandType, commandText, (SqlParameter[])null);
        } // 

        public static DataSet ExecuteDataset(string ConnectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((ConnectionString == null || ConnectionString.Length == 0))
                throw new ArgumentNullException("ConnectionString");
            SqlConnection connection;
            connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if (!(connection == null))
                    connection.Dispose();
            }
        } // ExecuteDataset

        public static DataSet ExecuteDataset(string ConnectionString, string spName, params object[] parameterValues)
        {
            if ((ConnectionString == null || ConnectionString.Length == 0))
                throw new ArgumentNullException("ConnectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(ConnectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(ConnectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteDataset(ConnectionString, CommandType.StoredProcedure, spName);
        } // ExecuteData

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {

            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter dataAdatpter;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Create the DataAdapter & DataSet
            dataAdatpter = new SqlDataAdapter(cmd);
            try
            {
                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();
            }
            // add this new for 400 club
            catch (Exception ex)
            {
            }

            finally
            {
                if ((!(dataAdatpter == null)))
                    dataAdatpter.Dispose();
            }
            if ((mustCloseConnection))
                connection.Close();

            // Return the dataset
            return ds;
        } // ExecuteDataset

        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {

                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        } // E

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        } // ExecuteDataset

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if (!(transaction == null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter dataAdatpter;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Create the DataAdapter & DataSet
            dataAdatpter = new SqlDataAdapter(cmd);
            try
            {
                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();
            }
            finally
            {
                if ((!(dataAdatpter == null)))
                    dataAdatpter.Dispose();
            }

            // Return the dataset
            return ds;
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if (!(transaction == null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        } // E

        #endregion

        #region "ExecuteScalar"
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        } // ExecuteScalar

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done.
            SqlConnection connection;
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if (!(connection == null))
                    connection.Dispose();
            }
        } // ExecuteScalar

        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;

            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        } // ExecuteScalar

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
        } // ExecuteScalar

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((connection == null))
                    throw new ArgumentNullException("connection");

                SqlCommand cmd = new SqlCommand();
                object retval = default(object);
                bool mustCloseConnection = false;

                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

                // Execute the command & return the results
                retval = cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                if ((mustCloseConnection))
                    connection.Close();

                return retval;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } // ExecuteScalar

        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        } // ExecuteScalar

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
        } // ExecuteScalar

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if (!(transaction == null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            SqlCommand cmd = new SqlCommand();
            object retval;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();

            return retval;
        } // ExecuteScalar

        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if (!(transaction == null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        } // ExecuteScalar
        #endregion

        public static int ExecuteNonQuery(string ConnectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(ConnectionString, commandType, commandText, (SqlParameter[])null);
        } // ExecuteNonQuery

        public static int ExecuteNonQuery(string ConnectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((ConnectionString == null || ConnectionString.Length == 0))
                throw new ArgumentNullException("ConnectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection;
            connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if (!(connection == null))
                    connection.Dispose();
            }
        } // ExecuteNonQuery


        public static int ExecuteNonQuery(string ConnectionString, string spName, params object[] parameterValues)
        {
            if ((ConnectionString == null || ConnectionString.Length == 0))
                throw new ArgumentNullException("ConnectionString");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)

                commandParameters = SqlHelperParameterCache.GetSpParameterSet(ConnectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, spName);
        } // ExecuteNonQuery

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
        } // ExecuteNonQuery

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            int retval;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if ((mustCloseConnection))
                connection.Close();

            return retval;
        } // ExecuteNonQuery

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string spName, params object[] parameterValues)
        {
            if ((connection == null))
                throw new ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");
            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        } // ExecuteNonQuery

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        } // ExecuteNonQuery

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if (!(transaction == null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            int retval;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            return retval;
        } // ExecuteNonQuery


        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if ((transaction == null))
                throw new ArgumentNullException("transaction");
            if (!(transaction == null) && (transaction.Connection == null))
                throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if ((spName == null || spName.Length == 0))
                throw new ArgumentNullException("spName");

            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        } // ExecuteNonQuery





        public sealed class SqlHelperParameterCache
        {

            private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter, params object[] parameterValues)
            {
                if ((connection == null))
                    throw new ArgumentNullException("connection");
                if ((spName == null || spName.Length == 0))
                    throw new ArgumentNullException("spName");
                SqlCommand cmd = new SqlCommand(spName, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter[] discoveredParameters;
                connection.Open();
                SqlCommandBuilder.DeriveParameters(cmd);
                connection.Close();
                if (!includeReturnValueParameter)
                    cmd.Parameters.RemoveAt(0);

                discoveredParameters = new SqlParameter[cmd.Parameters.Count - 1 + 1];
                cmd.Parameters.CopyTo(discoveredParameters, 0);

                // Init the parameters with a DBNull value
                //  SqlParameter discoveredParameter;
                foreach (SqlParameter discoveredParameter in discoveredParameters)
                    discoveredParameter.Value = DBNull.Value;

                return discoveredParameters;
            }

            private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
            {
                int i;
                int j = originalParameters.Length - 1;
                SqlParameter[] clonedParameters = new SqlParameter[j + 1];
                var loopTo = j;
                for (i = 0; i <= loopTo; i++)
                    clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();

                return clonedParameters;
            } // CloneParameters


            public static SqlParameter[] GetSpParameterSet(string ConnectionString, string spName)
            {
                return GetSpParameterSet(ConnectionString, spName, false);
            } // GetSpParameterSet
            public static SqlParameter[] GetSpParameterSet(string ConnectionString, string spName, bool includeReturnValueParameter)
            {
                if ((ConnectionString == null || ConnectionString.Length == 0))
                    throw new ArgumentNullException("ConnectionString");
                SqlConnection connection;
                connection = new SqlConnection(ConnectionString);
                try
                {
                    return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
                }
                finally
                {
                    if (!(connection == null))
                        connection.Dispose();

                }
            } // GetS

            public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
            {
                return GetSpParameterSet(connection, spName, false);
            } // GetSpParameterSet

            public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
            {
                if ((connection == null))
                    throw new ArgumentNullException("connection");
                SqlConnection clonedConnection;
                clonedConnection = (SqlConnection)((ICloneable)connection).Clone();
                try
                {
                    return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
                }
                finally
                {
                    if (!(clonedConnection == null))
                        clonedConnection.Dispose();
                }
            } // GetSpParameterSet

            private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
            {
                if ((connection == null))
                    throw new ArgumentNullException("connection");

                SqlParameter[] cachedParameters;
                string hashKey;

                if ((spName == null || spName.Length == 0))
                    throw new ArgumentNullException("spName");

                hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter == true ? ":include ReturnValue Parameter" : "").ToString();

                cachedParameters = (SqlParameter[])paramCache[hashKey];

                if ((cachedParameters == null))
                {
                    SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                    paramCache[hashKey] = spParameters;
                    cachedParameters = spParameters;
                }

                return CloneParameters(cachedParameters);
            } // GetSpParameterSet

        }


    }
}
