using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoHosting.Models.Database.Entities.Statistic;

namespace VideoHosting.Models.Database.Contexts
{
    public class VideoAnalyseContext
    {
        private OracleConnection _connection;

        public VideoAnalyseContext(OracleConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<RateData> GetRateData(string pageId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@rate_data_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            string sql = $@"VideoAnalysePackage.GetRateData";
            return _connection.Query<RateData>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<ViewData> GetViewData(string pageId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@view_data_cur", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            string sql = $@"VideoAnalysePackage.GetViewData";
            return _connection.Query<ViewData>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}