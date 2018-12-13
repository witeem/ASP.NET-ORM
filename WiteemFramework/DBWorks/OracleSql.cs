using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WiteemFramework.Enum;
using WiteemFramework.Handle;
using WiteemFramework.Model;

namespace WiteemFramework.DBWorks
{
    public class OracleSql : SQLHelper
    {
        log4net.ILog execlog = log4net.LogManager.GetLogger(typeof(OracleSql));//获取一个日志记录器
        public OracleSql(string conConfig)
        {
            try
            {
                Constr = CommonHelper.GetConnectionString(conConfig);
                conn = new OracleConnection();
                cmd = new OracleCommand();
                adapter = new OracleDataAdapter();
                conn.ConnectionString = conConfig;
            }
            catch (Exception ex)
            {
                //
            }

        }
        public override int Add<T>(IEnumerable<T> obj, bool isIdentity)
        {
            try
            {
                int i = 0;
                int result = 0;
                int success = 0;
                //Type type = obj.GetType();
                Type type = typeof(T);
                //获取表名
                string tableName = CommonHelper.GetTableName(type);
                PropertyInfo[] pis = type.GetProperties();
                //获取所有字段，和主键名称
                List<string> columns = null;
                List<PropertyInfo> proList = new List<PropertyInfo>();
                columns = CommonHelper.GetTableColumns(pis, ref proList, isIdentity);
                //处理是否包含主键插入
                //if (isIdentity)
                //{
                //    columns = CommonHelper.GetTableColumns(pis, true);
                //}
                //else
                //{
                //    columns = CommonHelper.GetTableColumns(pis, false);
                //}
                foreach (T item in obj)
                {
                    //生成SQL语句
                    StringBuilder sqlText = new StringBuilder();
                    sqlText.Append(" INSERT INTO ");
                    sqlText.Append(tableName);
                    sqlText.Append(" (");
                    //第一个字段
                    sqlText.Append(columns[0]);
                    //第二个起所有字段
                    int loop = columns.Count;
                    for (i = 1; i < loop; i++)
                    {
                        sqlText.Append(",");
                        sqlText.Append(columns[i]);
                    }
                    sqlText.Append(") VALUES (");
                    //第一个字段
                    sqlText.Append("@");
                    sqlText.Append(columns[0]);
                    //第二个起所有字段
                    for (i = 1; i < loop; i++)
                    {
                        sqlText.Append(",@");
                        sqlText.Append(columns[i]);
                    }
                    sqlText.Append(");");
                    //生成SqlParamter
                    PropertyInfo propertyInfo = null;
                    List<OracleParameter> paras = new List<OracleParameter>();
                    for (i = 0; i < loop; i++)
                    {
                        propertyInfo = proList[i];
                        OracleParameter para = new OracleParameter(columns[i], CommonHelper.GetOracleType(propertyInfo.PropertyType), -1);
                        para.Value = propertyInfo.GetValue(item);
                        paras.Add(para);
                    }
                    result = ExecuteNonQuery(sqlText.ToString(), CommandType.Text, paras, false);
                    if (result > 0)
                    {
                        success += 1;
                    }
                }
                if (conn.State == ConnectionState.Open)
                {
                    ConColsed();
                }
                return success;

            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": Add失败，原因【" + ex.ToString() + "】");
                return -1;
            }
        }
        public override int Delete<T>(IEnumerable<T> obj)
        {
            try
            {
                int result = 0;
                int success = 0;
                //Type type = obj.GetType();
                Type type = typeof(T);
                //获取表名
                string tableName = CommonHelper.GetTableName(type);
                PropertyInfo[] pis = type.GetProperties();
                PropertyInfo identityInfo = null;
                identityInfo = CommonHelper.GetTableIdentity(pis);
                string identityName = CommonHelper.GetIdentityName(pis);
                if (identityInfo == null)
                {
                    return 0;
                }
                if (string.IsNullOrEmpty(identityName))
                {
                    identityName = identityInfo.Name;
                }
                foreach (T item in obj)
                {
                    //生成SQL语句
                    StringBuilder sqlText = new StringBuilder();
                    sqlText.Append(" DELETE FROM ");
                    sqlText.Append(tableName);
                    sqlText.Append(" WHERE 1=1 ");
                    //主键筛选
                    sqlText.Append(" AND " + identityName);
                    sqlText.Append("=@" + identityName);
                    //生成SqlParamter
                    List<OracleParameter> paras = new List<OracleParameter>();
                    OracleParameter para = new OracleParameter(identityName, CommonHelper.GetOracleType(identityInfo.PropertyType), -1);
                    para.Value = identityInfo.GetValue(item);
                    paras.Add(para);
                    result = ExecuteNonQuery(sqlText.ToString(), CommandType.Text, paras, false);
                    if (result > 0)
                    {
                        success += 1;
                    }
                }
                if (conn.State == ConnectionState.Open)
                {
                    ConColsed();
                }
                return success;
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": Delete失败，原因【" + ex.ToString() + "】");
                return -1;
            }
        }
        public override int Update<T>(IEnumerable<T> obj)
        {
            try
            {
                int i = 0;
                int result = 0;
                int success = 0;
                //Type type = obj.GetType();
                Type type = typeof(T);
                //获取表名
                string tableName = CommonHelper.GetTableName(type);
                PropertyInfo[] pis = type.GetProperties();
                //获取所有字段，和主键名称
                string identityName = CommonHelper.GetIdentityName(pis);
                //获取主键名称
                PropertyInfo identityInfo = null;
                identityInfo = CommonHelper.GetTableIdentity(pis);
                if (identityInfo == null)
                {
                    return 0;
                }
                if (string.IsNullOrEmpty(identityName))
                {
                    identityName = identityInfo.Name;
                }
                List<string> columns = null;
                List<PropertyInfo> proList = new List<PropertyInfo>();
                //获取所有字段名称
                if (CacheHelper.GetCache(identityName) != null)
                {
                    columns = (List<string>)CacheHelper.GetCache(identityName);
                }
                else
                {
                    columns = CommonHelper.GetTableColumns(pis, ref proList, true);
                    CacheHelper.SetCache(identityName, columns, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60));
                }
                foreach (T item in obj)
                {
                    //生成SQL语句
                    StringBuilder sqlText = new StringBuilder();
                    int loop = columns.Count;
                    sqlText.Append(" UPDATE ");
                    sqlText.Append(tableName);
                    sqlText.Append(" SET ");
                    //第二个起所有字段
                    for (i = 0; i < loop; i++)
                    {
                        //判断第一个字段是否为主键
                        if (columns[i] == identityName)
                        {
                            continue;
                        }
                        sqlText.Append(columns[i] + "=@" + columns[i]);
                        if (i < loop - 1)
                        {
                            sqlText.Append(",");
                        }
                    }
                    //主键筛选
                    sqlText.Append(" WHERE " + identityName);
                    sqlText.Append("=@" + identityName);
                    //生成SqlParamter
                    List<OracleParameter> paras = new List<OracleParameter>();
                    PropertyInfo propertyInfo = null;
                    for (i = 0; i < loop; i++)
                    {
                        propertyInfo = proList[i];
                        OracleParameter para = new OracleParameter(columns[i], CommonHelper.GetOracleType(propertyInfo.PropertyType), -1);
                        para.Value = propertyInfo.GetValue(item);
                        paras.Add(para);
                    }
                    result = ExecuteNonQuery(sqlText.ToString(), CommandType.Text, paras, false);
                    if (result > 0)
                    {
                        success += 1;
                    }
                }
                if (conn.State == ConnectionState.Open)
                {
                    ConColsed();
                }
                return success;
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": Update失败，原因【" + ex.ToString() + "】");
                return -1;
            }
        }
        public override T GetModel<T>(string id)
        {
            int i = 0;
            Type type = typeof(T);
            T myT = new T();
            //获取表名
            string tableName = CommonHelper.GetTableName(type);

            PropertyInfo[] pis = type.GetProperties();
            PropertyInfo identityInfo = null;
            identityInfo = CommonHelper.GetTableIdentity(pis);
            string identityName = CommonHelper.GetIdentityName(pis);
            if (identityInfo == null)
            {
                return default(T);
            }
            if (string.IsNullOrEmpty(identityName))
            {
                identityName = identityInfo.Name;
            }
            //获取所有字段，和主键名称
            List<string> columns = null;
            List<PropertyInfo> proList = new List<PropertyInfo>();
            //获取所有字段名称
            List<ColumnKeyType> filterList = new List<ColumnKeyType>();
            filterList.Add(ColumnKeyType.Default);
            filterList.Add(ColumnKeyType.Read);
            filterList.Add(ColumnKeyType.Identity);
            if (CacheHelper.GetCache(identityName) != null)
            {
                columns = (List<string>)CacheHelper.GetCache(identityName);
            }
            else
            {
                columns = CommonHelper.GetTableColumns(pis, filterList, ref proList);
                CacheHelper.SetCache(identityName, columns, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60));
            }
            //生成SQL语句
            StringBuilder sqlText = new StringBuilder();
            sqlText.Append(" SELECT ");
            //第一个字段
            sqlText.Append(columns[0]);
            //第二个起所有字段
            int loop = columns.Count;
            for (i = 1; i < loop; i++)
            {
                sqlText.Append(",");
                sqlText.Append(columns[i]);
            }
            sqlText.Append(" FROM ");
            sqlText.Append(tableName);
            sqlText.Append(" WHERE 1=1 AND ");
            sqlText.Append(identityName + "=@" + identityName);
            //生成SqlParamter
            List<OracleParameter> paras = new List<OracleParameter>();
            OracleParameter para = new OracleParameter(identityName, CommonHelper.GetOracleType(identityInfo.PropertyType), -1);
            para.Value = id;
            paras.Add(para);
            return GetModel<T>(sqlText.ToString(), CommandType.Text, paras);
        }
        public override int ExecNonQuery(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            int result = ExecuteNonQuery(sqlstr, CommandType.Text, parameters);
            return result;
        }
        public override DataTable Select(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            return ExecuteDataTable(sqlstr, commandType, parameters);
        }
        public override DataTable Select(DBSearchBase DbSearch)
        {
            return ExecuteDataTable(DbSearch);
        }
        public override object ExecScalar(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            object result = ExecScalar(sqlstr, CommandType.Text, parameters);
            return result;
        }
        public override DataTable ExecuteQueryPage(DBSearchBase dbsearch, ref int totalCount)
        {
            try
            {
                OracleParameter[] pageParas = new OracleParameter[10];
                pageParas[0] = new OracleParameter("@tab", dbsearch.DBTable);
                pageParas[1] = new OracleParameter("@PrimaryKey", dbsearch.DBPrimaryKey);
                pageParas[2] = new OracleParameter("@strFld", dbsearch.DBFields);
                pageParas[3] = new OracleParameter("@strWhere", dbsearch.DBWhere);
                pageParas[4] = new OracleParameter("@PageIndex", dbsearch.PageIndex);
                pageParas[5] = new OracleParameter("@PageSize", dbsearch.PageSize);
                pageParas[6] = new OracleParameter("@Sort", dbsearch.DBSort);
                pageParas[7] = new OracleParameter("@Order", dbsearch.DBOrder);
                pageParas[8] = new OracleParameter("@IsDistinct", dbsearch.IsDistinct);
                pageParas[9] = new OracleParameter("@TotalCount", OracleType.Int32, 8);
                pageParas[9].Direction = ParameterDirection.Output;
                return ExecuteQueryPage("pro_common_pageList", pageParas, ref totalCount);
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecuteQueryPage失败，原因【" + ex.ToString() + "】");
                return null;
            }
        }
        public override bool ExecuteTransaction(Dictionary<string, IEnumerable<DbParameter>> dic)
        {
            return ExecTransaction(dic);
        }
    }
}
