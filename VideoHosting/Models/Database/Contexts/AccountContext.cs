using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoHosting.Models.Database.Entities;

namespace VideoHosting.Models.Database.Contexts
{
	public class AccountContext : IDisposable
	{
		private OracleConnection _connection;

		public AccountContext(OracleConnection connection)
		{
			_connection = connection;
		}

		public int CreateAccount(string email, string login, string password)
		{
			OracleDynamicParameters parameters = new OracleDynamicParameters();
			parameters.Add("@par_email", email, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@par_login", login, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@par_password", password, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@created", 0, OracleMappingType.Int32, ParameterDirection.Output);

			string sql = $@"AccountPackage.CreateAccount";
			_connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
			return parameters.Get<int>("@created");
		}

		public Account SignIn(string email, string password)
		{
			OracleDynamicParameters parameters = new OracleDynamicParameters();
			parameters.Add("@par_email", email, OracleMappingType.NVarchar2, ParameterDirection.InputOutput);
			parameters.Add("@par_password", password, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@account_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

			string sql = $@"AccountPackage.SignIn";
			return _connection.QueryFirstOrDefault<Account>(sql, parameters, commandType: CommandType.StoredProcedure);
		}

		public int ChangeAccountPassword(string email, string old_password, string new_password)
		{
			OracleDynamicParameters parameters = new OracleDynamicParameters();
			parameters.Add("@par_email", email, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@par_old_password", old_password, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@par_new_password", new_password, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@changed", 0, OracleMappingType.Int32, ParameterDirection.Output);

			string sql = $@"AccountPackage.ChangeAccountPassword";
			_connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
			return parameters.Get<int>("@changed");
		}

		public int GetIdByEmail(string email)
		{
			OracleDynamicParameters parameters = new OracleDynamicParameters();
			parameters.Add("@par_email", email, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@ret_id", 0, OracleMappingType.Int32, ParameterDirection.Output);

			string sql = $@"AccountPackage.GetIdByEmail";
			_connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
			return parameters.Get<int>("@ret_id");
		}

		public string GetLoginByEmail(string email)
        {
			OracleDynamicParameters parameters = new OracleDynamicParameters();
			parameters.Add("@par_email", email, OracleMappingType.NVarchar2, ParameterDirection.Input);
			parameters.Add("@ret_login", "", OracleMappingType.NVarchar2, ParameterDirection.Output);

			string sql = $@"AccountPackage.GetLoginByEmail";
			_connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
			return parameters.Get<string>("@ret_login");
		}

		public string GetLoginById(int id)
		{
			OracleDynamicParameters parameters = new OracleDynamicParameters();
			parameters.Add("@par_id", id, OracleMappingType.Int32, ParameterDirection.Input);
			parameters.Add("@ret_login", "", OracleMappingType.NVarchar2, ParameterDirection.Output);

			string sql = $@"AccountPackage.GetLoginById";
			_connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
			return parameters.Get<string>("@ret_login");
		}

		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}