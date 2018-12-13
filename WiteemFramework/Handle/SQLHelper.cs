using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using WiteemFramework.Enum;
using WiteemFramework.Model;

namespace WiteemFramework.Handle
{
    public abstract class SQLHelper
    {
        protected string Constr;
        protected DbConnection conn = null;
        protected DbCommand cmd = null;
        protected DbDataAdapter adapter = null;
        log4net.ILog execlog = log4net.LogManager.GetLogger(typeof(SQLHelper));//获取一个日志记录器
        public abstract int Add<T>(IEnumerable<T> obj,bool isIdentity) where T : new();
        public abstract int Update<T>(IEnumerable<T> obj) where T : new();
        public abstract int Delete<T>(IEnumerable<T> obj) where T : new();
        public abstract T GetModel<T>(string id) where T : new();
        public abstract int ExecNonQuery(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters);
        public abstract DataTable Select(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters);
        public abstract DataTable Select(DBSearchBase DbSearch);
        public abstract DataTable ExecuteQueryPage(DBSearchBase dbsearch, ref int totalCount);
        public abstract object ExecScalar(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters);
        public abstract bool ExecuteTransaction(Dictionary<string, IEnumerable<DbParameter>> dic);

        #region Add
        /// <summary>
        /// Add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Add<T>(T obj, bool isIdentity) where T : new()
        {
            List<T> list = new List<T>();
            list.Add(obj);
            return Add<T>(list, isIdentity);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update<T>(T obj) where T : new()
        {
            List<T> list = new List<T>();
            list.Add(obj);
            return Update<T>(list);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Delete<T>(T obj) where T : new()
        {
            List<T> list = new List<T>();
            list.Add(obj);
            return Delete<T>(list);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// GetModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected T GetModel<T>(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters) where T : new()
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(sqlstr, commandType, parameters);
            if (dt != null)
            {
                return CommonHelper.DataTableToObject<T>(dt, true);
            }
            return default(T);
        }
        #endregion

        #region 对数据库执行增删改操作 ExecuteNonQuery  
        /// <summary>
        /// 对数据库执行增删改操作
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual int ExecuteNonQuery(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters,bool IsConClosed = true)
        {
            int count = 0;
            try
            {
                cmd = ExecCmd(sqlstr, commandType, parameters);
                if (cmd == null)
                {
                    return 0;
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecuteNonQuery失败，原因【" + ex.ToString() + "】");
                return 0;
            }
            finally
            {
                CmdDispose();
                if (IsConClosed)
                {
                    ConColsed();
                }
            }
            return count;
        }
        #endregion

        #region 返回查询结果的第一行第一列，忽略其它行和列 ExecuteScalar
        /// <summary>
        /// 返回查询结果的第一行第一列，忽略其它行和列 ExecuteScalar
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual object ExecuteScalar(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters, bool IsConClosed = true)
        {
            object result = null;
            try
            {
                cmd = ExecCmd(sqlstr, commandType, parameters);
                if (cmd == null)
                {
                    return null;
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecuteScalar失败，原因【" + ex.ToString() + "】");
                return null;
            }
            finally
            {
                CmdDispose();
                if (IsConClosed)
                {
                    ConColsed();
                }
            }
            return result;
        }
        #endregion

        #region 执行一个查询,并返回查询结果 ExecuteDataTable
        /// <summary>
        /// 执行一个查询,并返回查询结果
        /// </summary>
        /// <returns></returns>
        protected virtual DataTable ExecuteDataTable(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters, bool IsConClosed = true)
        {
            DataTable data = new DataTable();//实例化DataTable，用于装载查询结果集 
            try
            {
                cmd = ExecCmd(sqlstr, commandType, parameters);
                if (cmd == null)
                {
                    return null;
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                //通过包含查询SQL的SqlCommand实例来实例化SqlDataAdapter   
                adapter.SelectCommand = cmd;
                adapter.Fill(data);//填充DataTable   
                if (data.Rows.Count > 0)
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecCmd失败，原因【" + ex.ToString() + "】");
                return null;
            }
            finally
            {
                CmdDispose();
                if (IsConClosed)
                {
                    ConColsed();
                }
            }
            return null;
        }
        /// <summary>
        /// 执行一个查询,并返回查询结果
        /// </summary>
        /// <param name="DbSearch"></param>
        /// <returns></returns>
        protected virtual DataTable ExecuteDataTable(DBSearchBase DbSearch, bool IsConClosed = true)
        {
            if (DbSearch == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(DbSearch.DBTable))
            {
                return null;
            }
            DataTable data = new DataTable();//实例化DataTable，用于装载查询结果集 
            try
            {
                cmd.Connection = conn;
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append(" SELECT ");
                if (DbSearch.IsDistinct)
                {
                    sqlText.Append(" DISTINCT ");
                }
                sqlText.Append(DbSearch.DBFields);
                sqlText.Append(" FROM " + DbSearch.DBTable);
                sqlText.Append(" WHERE 1=1 ");
                sqlText.Append(DbSearch.DBWhere);
                //排序
                if (!string.IsNullOrEmpty(DbSearch.DBOrder))
                {
                    sqlText.Append(" ORDER BY " + DbSearch.DBOrder);
                    sqlText.Append(DbSearch.DBSort.ToLower() == "desc" ? " DESC " : " ASC ");
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.CommandText = sqlText.ToString();
                cmd.CommandType = CommandType.Text;
                adapter.SelectCommand = cmd;
                adapter.Fill(data);//填充DataTable   
                if (data.Rows.Count > 0)
                {
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecuteDataTable失败，原因【" + ex.ToString() + "】");
                return null;
            }
            finally
            {
                CmdDispose();
                if (IsConClosed)
                {
                    ConColsed();
                }
            }
        }
        #endregion

        #region 分页查询 ExecuteQueryPage
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dbsearch"></param>
        /// <param name="parameters"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        protected virtual DataTable ExecuteQueryPage(string ProcedureStr, IEnumerable<DbParameter> parameters, ref int totalCount, bool IsConClosed = true)
        {
            DataTable data = new DataTable();//实例化DataTable，用于装载查询结果集 
            DbDataReader dbread = null;
            int parasCount = 0;
            cmd.Connection = conn;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                //cmd.CommandText = "pro_common_pageList";
                cmd.CommandText = ProcedureStr;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (DbParameter para in parameters)
                    {
                        cmd.Parameters.Add(para);
                        parasCount++;
                    }
                }
                dbread = cmd.ExecuteReader();
                data.Load(dbread);
                //错误的地方
                //totalCount = Convert.ToInt32(cmd.Parameters[parasCount].Value);
                //返回总记录数（修正后）
                totalCount = Convert.ToInt32(cmd.Parameters[parasCount - 1].Value);
                if (totalCount > 0)
                {
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecuteQueryPage失败，原因【" + ex.ToString() + "】");
                return null;
            }
            finally
            {
                CmdDispose();
                if (IsConClosed)
                {
                    ConColsed();
                }
            }
        }
        #endregion

        #region 事务处理
        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        protected virtual bool ExecTransaction(Dictionary<string, IEnumerable<DbParameter>> dic)
        {
            DbTransaction myTran = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                myTran = conn.BeginTransaction();
                int success = 0;
                foreach (string item in dic.Keys)
                {
                    cmd.Parameters.Clear();
                    IEnumerable<DbParameter> param = dic[item];
                    cmd = ExecCmd(item, CommandType.Text, param);
                    cmd.Transaction = myTran;
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        success += 1;
                    }
                }
                if (success > 0)
                {
                    //提交事务
                    myTran.Commit();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecTransaction失败，原因【" + ex.ToString() + "】");
                myTran.Rollback();
                return false;
            }
            finally
            {
                ConColsed();
            }
        }
        #endregion

        #region DbCommand 参数整合
        /// <summary>
        /// DbCommand 参数整合
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand ExecCmd(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            cmd.Connection = conn;
            cmd.CommandText = sqlstr;
            try
            {
                //设置command的CommandType为指定的CommandType   
                cmd.CommandType = commandType;
                //如果同时传入了参数，则添加这些参数
                if (parameters != null)
                {
                    foreach (DbParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                return cmd;
            }
            catch (Exception ex)
            {
                execlog.Debug(DateTime.Now.ToString() + ": ExecCmd失败，原因【" + ex.ToString() + "】");
                return null;
            }
        }
        #endregion

        #region Command释放
        /// <summary>
        /// Command释放
        /// </summary>
        protected void ConColsed()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        protected void CmdDispose()
        {
            if (cmd != null)
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        #endregion
    }
}
