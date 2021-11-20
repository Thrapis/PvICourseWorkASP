using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoHosting.Models.Utils;

namespace VideoHosting.Models.Database.Connections
{
    public static class MainOracleConnection
    {
        public static OracleConnection GetConnection()
        {
            string host = "WIN-QSRMNQOT9SQ";
            int port = 1521;
            string sid = "ORCLPDB";
            string user = "BAA";
            string password = "12345";
            return OracleHelper.GetDBConnection(host, port, sid, user, password);
        }
    }
}