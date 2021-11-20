using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Database.Entities.Sinthetic;

namespace VideoHosting.Models.Database.Contexts
{
    public class InfoPackageContext : IDisposable
    {
        private OracleConnection _connection;

        public InfoPackageContext(OracleConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<VideoPreviewInfo> GetNFirstVideoPreview(int n)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_count", n, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@video_preview_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            string sql = $@"InfoPackage.GetNFirstVideoPreview";
            return _connection.Query<VideoPreviewInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public VideoPageInfo GetVideoPageInfo(string pageId, int accountId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_account_id", accountId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@page_info_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            string sql = $@"InfoPackage.GetVideoPageInfo";
            return _connection.QueryFirstOrDefault<VideoPageInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<CommentInfo> GetCommentsOfVideoPage(string pageId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@comment_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            string sql = $@"InfoPackage.GetCommentsOfVideoPage";
            return _connection.Query<CommentInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<CommentInfo> GetCommentsOfVideoPageAfter(string pageId, int afterCommentId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_after_comment_id", afterCommentId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@comment_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            string sql = $@"InfoPackage.GetCommentsOfVideoPageAfter";
            return _connection.Query<CommentInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}