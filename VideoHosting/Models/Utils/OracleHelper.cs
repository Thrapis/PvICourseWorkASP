using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Utils
{
    public static class OracleHelper
    {
        public static OracleConnection GetDBConnection(string host, int port, string sid, string user, string password)
        {
            /*string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + sid + ")));Password=" + password + ";User ID=" + user;*/

            string connString = $"DATA SOURCE = {host}:{port}/{sid}; USER ID={user}; PASSWORD={password}; Pooling = False";

            return new OracleConnection(connString);
        }
    }
}