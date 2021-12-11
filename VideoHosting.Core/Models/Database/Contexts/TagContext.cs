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
    public class TagContext : IDisposable
    {
        private OracleConnection _connection;

        public TagContext(OracleConnection connection)
        {
            _connection = connection;
        }

        public int Insert(Tag tag)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", tag.VideoPageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_tag_name", tag.Name, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@created", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"TagPackage.AttachTag";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@created");
        }

        public int Insert(string pageId, string[] tags)
        {
            string sql = $@"TagPackage.AttachTags";
            int created = 0;
            using (OracleCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("par_video_page_id", OracleDbType.NVarchar2, ParameterDirection.Input);
                cmd.Parameters[0].Value = pageId;

                var arrPar = cmd.Parameters.Add("par_tag_names", OracleDbType.NVarchar2);
                arrPar.Direction = ParameterDirection.Input;
                arrPar.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                arrPar.Value = tags.ToArray();
                arrPar.Size = tags.Count();
                arrPar.ArrayBindSize = tags.Select(s => s.Length).ToArray();
                arrPar.ArrayBindStatus = Enumerable.Repeat(OracleParameterStatus.Success, tags.Count()).ToArray();

                var createdResult = cmd.Parameters.Add("created", OracleDbType.Int32, ParameterDirection.Output);

                cmd.ExecuteNonQuery();
                created = int.Parse(createdResult.Value.ToString());
            }
            return created;
        }

        public int Remove(Tag tag)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", tag.VideoPageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@par_tag_name", tag.Name, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@deleted", 0, OracleMappingType.Int32, ParameterDirection.Output);

            string sql = $@"TagPackage.DetachTag";
            _connection.Query(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@deleted");
        }

        public void Update(string pageId, string[] tags)
        {
            string sql = $@"TagPackage.UpdateTags";
            using (OracleCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("par_video_page_id", OracleDbType.NVarchar2, ParameterDirection.Input);
                cmd.Parameters[0].Value = pageId;

                var arrPar = cmd.Parameters.Add("par_tag_names", OracleDbType.NVarchar2);
                arrPar.Direction = ParameterDirection.Input;
                arrPar.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                arrPar.Value = tags.ToArray();
                arrPar.Size = tags.Count();
                arrPar.ArrayBindSize = tags.Select(s => s.Length).ToArray();
                arrPar.ArrayBindStatus = Enumerable.Repeat(OracleParameterStatus.Success, tags.Count()).ToArray();

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Tag> GetTagsByVideoPageId(string pageId)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            parameters.Add("@par_video_page_id", pageId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("@tags_cur", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

            string sql = $@"TagPackage.GetTagsByVideoPageId";
            return _connection.Query<Tag>(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}