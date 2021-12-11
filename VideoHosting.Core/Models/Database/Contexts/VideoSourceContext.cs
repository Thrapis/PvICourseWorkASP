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
    public class VideoSourceContext : IDisposable
    {
        private OracleConnection _connection;

        public VideoSourceContext(OracleConnection connection)
        {
            _connection = connection;
        }

        public VideoSource GetByVideoPageIdAndQuality(string pageId, int quality)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_quality", quality, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@source_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            string sql = $@"VideoSourcePackage.GetByVideoPageIdAndQuality";
            return _connection.QueryFirstOrDefault<VideoSource>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public int Insert(VideoSource video)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", video.VideoPageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_data", video.Data, OracleMappingType.Blob, ParameterDirection.Input);
            parameters.Add("@par_size", video.Size, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@par_quality", video.Quality, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("@par_format", video.Format, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@created", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"VideoSourcePackage.CreateVideoSource";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@created");
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}