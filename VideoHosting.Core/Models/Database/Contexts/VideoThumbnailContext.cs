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

namespace VideoHosting.Models.Database.Contexts
{
    public class VideoThumbnailContext : IDisposable
    {
        private OracleConnection _connection;

        public VideoThumbnailContext(OracleConnection connection)
        {
            _connection = connection;
        }

        public VideoThumbnail GetByVideoPageId(string pageId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@thumbnail_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            string sql = $@"VideoThumbnailPackage.GetByVideoPageId";
            return _connection.QueryFirstOrDefault<VideoThumbnail>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public int Insert(VideoThumbnail preview)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", preview.VideoPageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_data", preview.Data, OracleMappingType.Blob, ParameterDirection.Input);
            parameters.Add("@par_size", preview.Size, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@par_format", preview.Format, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@created", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"VideoThumbnailPackage.CreateVideoThumbnail";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@created");
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}