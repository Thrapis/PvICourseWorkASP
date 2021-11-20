using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoHosting.Models.Database.Entities;
using static VideoHosting.Models.Database.Entities.AuthVideoView;

namespace VideoHosting.Models.Database.Contexts
{
    public class VideoPageContext : IDisposable
    {
        private OracleConnection _connection;

        public VideoPageContext(OracleConnection connection)
        {
            _connection = connection;
        }

        public VideoPage GetById(string id)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_id", id, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@page_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            string sql = $@"VideoPagePackage.CreateVideoPage";
            return _connection.QueryFirstOrDefault<VideoPage>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<VideoPage> GetAllByAccountId(int accountId) {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_account_id", accountId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@pages_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            string sql = $"VideoPagePackage.GetAllByAccountId";
            return _connection.Query<VideoPage>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public int Insert(VideoPage page)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_id", page.Id, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_account_id", page.AccountId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@par_video_name", page.VideoName, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@created", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"VideoPagePackage.CreateVideoPage";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@created");
        }

        public IEnumerable<VideoPage> GetNFirst(int n)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_count", n, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@pages_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            string sql = $@"VideoPagePackage.GetNFirst";
            return _connection.Query<VideoPage>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public int GetViewsCountOfVideoById(string pageId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@ret_views", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"VideoPagePackage.GetViewsCountOfVideoById";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@ret_views");
        }

        public void AddAuthViewToVideoById(string pageId, int accountId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_account_id", accountId, OracleMappingType.Int32, ParameterDirection.Input);

            string sql = $@"VideoPagePackage.AddAuthViewToVideoById";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public void AddNonAuthViewToVideoById(string pageId, string ipAdress)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_ip_address", ipAdress, OracleMappingType.NVarchar2, ParameterDirection.Input);

            string sql = $@"VideoPagePackage.AddNonAuthViewToVideoById";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public void SetRateToVideoById(string pageId, int accountId, RateType rate)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_account_id", accountId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@par_rate", rate, OracleMappingType.Int32, ParameterDirection.Input);

            string sql = $@"VideoPagePackage.SetRateToVideoById";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public int AddComment(Comment comment)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", comment.VideoPageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_account_id", comment.AccountId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@par_text", comment.Text, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@created", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"VideoPagePackage.AddComment";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@created");
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}