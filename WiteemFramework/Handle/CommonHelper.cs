using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;
using System.Text.RegularExpressions;
using WiteemFramework.Enum;
using WiteemFramework.Filter;

namespace WiteemFramework.Handle
{
    public class CommonHelper
    {
        #region 根据Type类型获取SQL的数据类型 SqlDbType
        /// <summary>
        /// 根据Type类型获取MySQL的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlDbType GetSqlType(Type type)
        {
            SqlDbType dbtype = SqlDbType.VarChar;
            if (type.Equals(typeof(string)))
            {

            }
            else if (type.Equals(typeof(int)))
            {
                dbtype = SqlDbType.Int;
            }
            else if (type.Equals(typeof(bool)))
            {
                dbtype = SqlDbType.Bit;
            }
            else if (type.Equals(typeof(DateTime)))
            {
                dbtype = SqlDbType.DateTime;
            }
            else if (type.Equals(typeof(decimal)))
            {
                dbtype = SqlDbType.Decimal;
            }
            else if (type.Equals(typeof(float)))
            {
                dbtype = SqlDbType.Float;
            }
            else if (type.Equals(typeof(double)))
            {
                dbtype = SqlDbType.Float;
            }
            return dbtype;
        }

        public static OracleType GetOracleType(Type type)
        {
            OracleType dbtype = OracleType.NVarChar;
            if (type.Equals(typeof(string)))
            {

            }
            else if (type.Equals(typeof(int)))
            {
                dbtype = OracleType.Int32;
            }
            else if (type.Equals(typeof(bool)))
            {
                dbtype = OracleType.Int16;
            }
            else if (type.Equals(typeof(DateTime)))
            {
                dbtype = OracleType.DateTime;
            }
            else if (type.Equals(typeof(decimal)))
            {
                dbtype = OracleType.Number;
            }
            else if (type.Equals(typeof(float)))
            {
                dbtype = OracleType.Float;
            }
            else if (type.Equals(typeof(double)))
            {
                dbtype = OracleType.Double;
            }
            return dbtype;
        }
        #endregion

        #region 从Model模型中获取数据表名 GetTableName
        /// <summary>
        /// 从Model模型中获取数据表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            PropertyAttribute property = (PropertyAttribute)(type.GetCustomAttributes(false)[0]);
            return property.TableName;
        }
        #endregion

        #region 从Model模型中获取数据主键字段 GetTableIdentity
        /// <summary>
        /// 从Model模型中获取数据主键字段
        /// </summary>
        /// <param name="pis"></param>
        /// <returns></returns>
        public static PropertyInfo GetTableIdentity(PropertyInfo[] pis)
        {
            object[] infos = null;
            PropertyAttribute attribute = null;
            PropertyInfo info = null;
            foreach (PropertyInfo pi in pis)
            {
                infos = pi.GetCustomAttributes(typeof(PropertyAttribute), false);
                if (infos.Length > 0)
                {
                    attribute = (PropertyAttribute)(infos[0]);
                    if (attribute == null)
                    {
                        continue;
                    }
                    else if (attribute.columnKeyType == ColumnKeyType.Identity)
                    {
                        info = pi;
                        break;
                    }
                }
            }
            return info;
        }
        /// <summary>
        /// 从Model模型中获取数据主键名称
        /// </summary>
        /// <param name="pis"></param>
        /// <returns></returns>
        public static string GetIdentityName(PropertyInfo[] pis)
        {
            object[] fields = null;
            DataFieldAttribute Fieldattribute = null;
            PropertyInfo info = GetTableIdentity(pis);
            fields = info.GetCustomAttributes(typeof(DataFieldAttribute), false);
            if (fields.Length>0)
            {
                Fieldattribute = (DataFieldAttribute)(fields[0]);
                return Fieldattribute.FieldName;
            }
            return "";
        }
        #endregion

        #region 从Model获取需要的读取数据源的字段集 GetTableColumns
        /// <summary>
        /// 获取字段成员,不包括类型Extend
        /// </summary>
        /// <param name="pis"></param>
        /// <param name="Isidentity">是否包含主键</param>
        /// <returns></returns>
        public static List<string> GetTableColumns(PropertyInfo[] pis, ref List<PropertyInfo> proList, bool Isidentity = false)
        {
            List<string> columns = new List<string>();
            object[] infos = null;
            object[] fields = null;
            DataFieldAttribute Fieldattribute = null;
            PropertyAttribute attribute = null;
            foreach (PropertyInfo pi in pis)
            {
                //获取此成员所有自定义特性
                infos = pi.GetCustomAttributes(typeof(PropertyAttribute),false);
                fields = pi.GetCustomAttributes(typeof(DataFieldAttribute), false);
                if (fields.Length == 0)
                {
                    continue;
                }
                Fieldattribute = (DataFieldAttribute)(fields[0]);
                if (infos.Length > 0)
                {
                    attribute = (PropertyAttribute)(infos[0]);
                    if (attribute == null)
                    {
                        //columns.Add(pi.Name);
                        columns.Add(Fieldattribute.FieldName);
                        proList.Add(pi);
                    }
                    else
                    {
                        switch (attribute.columnKeyType)
                        {
                            case ColumnKeyType.Extend: break;
                            case ColumnKeyType.Identity:
                                {
                                    if (Isidentity)
                                    {
                                        //columns.Add(pi.Name);
                                        columns.Add(Fieldattribute.FieldName);
                                        proList.Add(pi);
                                    }
                                }; break;
                            default:
                                {
                                    //columns.Add(pi.Name);
                                    columns.Add(Fieldattribute.FieldName);
                                    proList.Add(pi);
                                };
                                break;
                        }
                    }
                }
            }
            return columns;
        }
        /// <summary>
        /// 根据字段状态获取字段成员
        /// </summary>
        /// <param name="pis"></param>
        /// <param name="filter">字段状态</param>
        /// <param name="isIdentity">是否包含主键</param>
        /// <returns></returns>
        public static List<string> GetTableColumns(PropertyInfo[] pis, ColumnKeyType filter, ref List<PropertyInfo> proList)
        {
            List<ColumnKeyType> list = new List<ColumnKeyType>();
            list.Add(filter);
            return GetTableColumns(pis, list,ref proList);
        }
        /// <summary>
        /// 根据字段状态获取字段成员
        /// </summary>
        /// <param name="pis"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<string> GetTableColumns(PropertyInfo[] pis, List<ColumnKeyType> filter, ref List<PropertyInfo> proList)
        {
            List<string> columns = new List<string>();
            object[] infos = null;
            object[] fields = null;
            DataFieldAttribute Fieldattribute = null;
            PropertyAttribute attribute = null;
            foreach (PropertyInfo pi in pis)
            {
                //获取此成员所有自定义特性
                infos = pi.GetCustomAttributes(typeof(PropertyAttribute), false);
                fields = pi.GetCustomAttributes(typeof(DataFieldAttribute), false);
                if (fields.Length == 0)
                {
                    continue;
                }
                Fieldattribute = (DataFieldAttribute)(fields[0]);
                if (infos.Length > 0)
                {
                    attribute = (PropertyAttribute)(infos[0]);
                    if (attribute == null)
                    {
                        continue;
                    }
                    if (filter.Count > 0)
                    {
                        foreach (ColumnKeyType item in filter)
                        {
                            if (attribute.columnKeyType == item)
                            {
                                columns.Add(Fieldattribute.FieldName);
                                proList.Add(pi);
                            }
                        }
                    }
                }
            }
            return columns;
        }
        #endregion

        #region 检查是否满足某种正则表达式 IsRegexMatch
        /// <summary>
        /// 检查是否满足某种正则表达式
        /// </summary>
        private static bool IsRegexMatch(string str, string Express)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            return Regex.IsMatch(str, Express);

        }
        #endregion

        #region DataTable 转 Object
        /// <summary>
        /// DataTable 转 Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T DataTableToObject<T>(DataTable dt, bool IsDataField) where T : new()
        {
            Type type = typeof(T);
            string tempName = string.Empty;
            T myt = new T();
            PropertyInfo[] propertys = myt.GetType().GetProperties();
            PropertyInfo[] array = propertys;
            int i = 0;
            DataFieldAttribute attribute = null;
            PropertyAttribute proAttribute = null;
            while (i < array.Length)
            {
                PropertyInfo pi = array[i];
                if (IsDataField)
                {
                    object[] infos = null;
                    object[] pros = null;
                    infos = pi.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    pros = pi.GetCustomAttributes(typeof(PropertyAttribute), false);
                    if (infos.Length > 0)
                    {
                        attribute = (DataFieldAttribute)(infos[0]);
                        if (pros.Length>0)
                        {
                            proAttribute = (PropertyAttribute)(pros[0]);
                            if (proAttribute.columnKeyType != ColumnKeyType.Extend)
                            {
                                tempName = attribute.FieldName;
                            }
                        }else
                            tempName = attribute.FieldName;
                    }
                }
                else
                    tempName = pi.Name;

                if (dt.Columns.Contains(tempName))
                {
                    if (pi.CanWrite)
                    {
                        object value = dt.Rows[0][tempName];
                        //if (value.GetType().Equals(typeof(DateTime)))
                        //    value = Convert.ToString(value);
                        if (value != DBNull.Value)
                            pi.SetValue(myt, value, null);
                    }
                }
                i += 1;
                continue;
            }
            return myt;
        }
        #endregion

        #region DataTable转换成List<T>
        /// <summary>
        ///  DataTable转换成List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="IsDataField"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt, bool IsDataField) where T : new()
        {
            List<T> ts = new List<T>();
            Type type = typeof(T);
            string tempName = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                T myt = new T();
                PropertyInfo[] propertys = myt.GetType().GetProperties();
                PropertyInfo[] array = propertys;
                int i = 0;
                DataFieldAttribute attribute = null;
                PropertyAttribute proAttribute = null;
                while (i < array.Length)
                {
                    PropertyInfo pi = array[i];
                    if (IsDataField)
                    {
                        object[] infos = null;
                        object[] pros = null;
                        infos = pi.GetCustomAttributes(typeof(DataFieldAttribute), false);
                        pros = pi.GetCustomAttributes(typeof(PropertyAttribute), false);
                        if (infos.Length > 0)
                        {
                            attribute = (DataFieldAttribute)(infos[0]);
                            if (pros.Length > 0)
                            {
                                proAttribute = (PropertyAttribute)(pros[0]);
                                if (proAttribute.columnKeyType != ColumnKeyType.Extend)
                                {
                                    tempName = attribute.FieldName;
                                }
                            }
                            else
                                tempName = attribute.FieldName;
                        }
                    }
                    else
                        tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (pi.CanWrite)
                        {
                            object value = dr[tempName];
                            //if (value.GetType().Equals(typeof(DateTime)))
                            //    value = System.Convert.ToString(value);
                            if (value != DBNull.Value)
                                pi.SetValue(myt, value, null);
                        }
                    }
                    i += 1;
                    continue;
                }
                ts.Add(myt);
            }
            return ts;
        }
        #endregion

        #region 获取配置信息
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="mykey"></param>
        /// <returns></returns>
        public static string GetConnectionString(string mykey)
        {
            string result = ConfigurationManager.ConnectionStrings[mykey].ConnectionString;
            if (string.IsNullOrEmpty(result))
            {
                return "";
            }
            return result;
        }

        /// <summary>
        /// 获取提供程序名称属性
        /// </summary>
        /// <param name="mykey"></param>
        /// <returns></returns>
        public static string GetproviderName(string mykey)
        {
            string result = ConfigurationManager.ConnectionStrings[mykey].ProviderName;
            if (string.IsNullOrEmpty(result))
            {
                return "";
            }
            return result;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="mykey"></param>
        /// <returns></returns>
        public static string GetAppSetting(string mykey)
        {
            string result = ConfigurationManager.AppSettings[mykey];
            if (string.IsNullOrEmpty(result))
            {
                return "";
            }
            return result;
        }
        #endregion
    }
}
