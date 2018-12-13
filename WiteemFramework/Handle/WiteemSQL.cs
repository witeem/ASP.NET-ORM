using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using WiteemFramework.Enum;
using WiteemFramework.Model;
using WiteemFramework.DBWorks;
using WiteemFramework.DBTypeFactory;

namespace WiteemFramework.Handle
{
    public class WiteemSQL : IWiteem
    {
        #region =======属性========
        /// <summary>
        /// 数据库连接字符串键名Key
        /// </summary>k
        private string connKey = "MSSQL";
        /// <summary>
        /// 数据库数据库连接字符串
        /// </summary>
        protected string ConnStr;
        protected SQLHelper mySql;

        public string ConnKey
        {
            get
            {
                return connKey;
            }

            set
            {
                connKey = value;
            }
        }
        #endregion

        #region =======构造函数========
        public WiteemSQL()
        {
            mySql = new MSSql(this.ConnKey);
        }


        public WiteemSQL(string dataKey)
        {
            mySql = new MSSql(dataKey);
        }


        public WiteemSQL(DBEnum dbType)
        {
            mySql = DBCrateFactory.GetSQLHelperInstance(dbType, null);
        }


        public WiteemSQL(DBEnum dbType, string dataKey)
        {
            mySql = DBCrateFactory.GetSQLHelperInstance(dbType, dataKey);
        }
        #endregion

        #region 新增 Add
        /// <summary>
        /// 新增 Add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Add<T>(T obj) where T : new()
        {
            if (mySql != null)
            {
                return mySql.Add(obj,false);
            }
            return 0;
        }
        /// <summary>
        /// 新增 Add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Add<T>(IEnumerable<T> obj) where T : new()
        {

            if (mySql != null)
            {
                return mySql.Add(obj,false);
            }
            return 0;
        }
        #endregion

        #region 新增包含主键 AddPrimary
        public virtual int AddPrimary<T>(T obj) where T : new()
        {
            if (mySql != null)
            {
                return mySql.Add(obj,true);
            }
            return 0;
        }

        public virtual int AddPrimary<T>(IEnumerable<T> obj) where T : new()
        {
            if (mySql != null)
            {
                return mySql.Add(obj, true);
            }
            return 0;
        }
        #endregion

        #region 修改 Update
        /// <summary>
        /// 修改 Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Update<T>(T obj) where T : new()
        {

            if (mySql != null)
            {
                return mySql.Update(obj);
            }
            return 0;
        }
        /// <summary>
        /// 修改 Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Update<T>(IEnumerable<T> obj) where T : new()
        {

            if (mySql != null)
            {
                return mySql.Update(obj);
            }
            return 0;
        }
        #endregion

        #region 删除 Delete
        /// <summary>
        /// 删除 Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Delete<T>(T obj) where T : new()
        {

            if (mySql != null)
            {
                return mySql.Delete(obj);
            }
            return 0;
        }
        /// <summary>
        /// 删除 Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Delete<T>(IEnumerable<T> obj) where T : new()
        {

            if (mySql != null)
            {
                return mySql.Delete(obj);
            }
            return 0;
        }
        #endregion

        #region 查询 Select
        /// <summary>
        /// 查询 Select
        /// </summary>
        /// <param name="DbSearch"></param>
        /// <returns></returns>
        public virtual DataTable Select(DBSearchBase DbSearch)
        {

            if (mySql != null)
            {
                return mySql.Select(DbSearch);
            }
            return null;
        }
        /// <summary>
        /// 查询 Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DbSearch"></param>
        /// <returns></returns>
        public List<T> Select<T>(DBSearchBase DbSearch) where T : new()
        {
            if (mySql != null)
            {
                DataTable dt = new DataTable();
                dt = mySql.Select(DbSearch);
                if (dt != null)
                {
                    return CommonHelper.DataTableToList<T>(dt,true);
                }
            }
            return null;
        }

        /// <summary>
        /// 查询 Select
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable Select(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {

            if (mySql != null)
            {
                return mySql.Select(sqlstr, commandType, parameters);
            }
            return null;
        }

        public List<T> Select<T>(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters) where T : new()
        {
            if (mySql != null)
            {
                DataTable dt = new DataTable();
                dt = mySql.Select(sqlstr, commandType, parameters);
                if (dt != null)
                {
                    return CommonHelper.DataTableToList<T>(dt,true);
                }
            }
            return null;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// GetModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetModel<T>(int id) where T : new()
        {
            if (mySql != null)
            {
                return mySql.GetModel<T>(id.ToString());
            }
            return default(T);
        }
        /// <summary>
        /// GetModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetModel<T>(string id) where T : new()
        {
            if (mySql != null)
            {
                return mySql.GetModel<T>(id);
            }
            return default(T);
        }
        /// <summary>
        /// GetModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DbSearch"></param>
        /// <returns></returns>
        public T GetModel<T>(DBSearchBase DbSearch) where T : new()
        {
            if (mySql != null)
            {
                DataTable dt = new DataTable();
                dt =  mySql.Select(DbSearch);
                if (dt != null)
                {
                    return CommonHelper.DataTableToObject<T>(dt,true);
                }
            }
            return default(T);
        }
        /// <summary>
        /// GetModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetModel<T>(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters) where T : new()
        {
            if (mySql != null)
            {
                DataTable dt = new DataTable();
                dt = mySql.Select(sqlstr, commandType, parameters);
                if (dt != null)
                {
                    return CommonHelper.DataTableToObject<T>(dt,true);
                }
            }
            return default(T);
        }
        #endregion

        #region ExecuteNonQuery
            /// <summary>
            /// ExecuteNonQuery
            /// </summary>
            /// <param name="sqlstr"></param>
            /// <param name="commandType"></param>
            /// <param name="parameters"></param>
            /// <returns></returns>
        public virtual int ExecuteNonQuery(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            if (mySql != null)
            {
                int result = mySql.ExecNonQuery(sqlstr, commandType, parameters);
            }
            return 0;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters)
        {
            if (mySql != null)
            {
                object result = mySql.ExecNonQuery(sqlstr, commandType, parameters);
            }
            return null;
        }
        #endregion

        #region ExecuteTransaction
        /// <summary>
        /// ExecuteTransaction
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public bool ExecuteTransaction(Dictionary<string, IEnumerable<DbParameter>> dic)
        {
            if (mySql != null)
            {
                return mySql.ExecuteTransaction(dic);
            }
            return false;
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dbsearch"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryPage(DBSearchBase dbsearch, ref int totalCount)
        {
            if (mySql != null)
            {
                return mySql.ExecuteQueryPage(dbsearch,ref totalCount);
            }
            return null;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbsearch"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<T> ExecuteQueryPage<T>(DBSearchBase dbsearch, ref int totalCount) where T : new()
        {
            if (mySql != null)
            {
                DataTable dt = new DataTable();
                dt =  mySql.ExecuteQueryPage(dbsearch, ref totalCount);
                if (dt != null)
                {
                    return CommonHelper.DataTableToList<T>(dt,true);
                }
            }
            return null;
        }
        #endregion
    }
}
