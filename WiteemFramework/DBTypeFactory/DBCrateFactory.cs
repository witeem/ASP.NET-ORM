using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WiteemFramework.DBWorks;
using WiteemFramework.Enum;
using WiteemFramework.Handle;

namespace WiteemFramework.DBTypeFactory
{
    public abstract class DBCrateFactory
    {
        static SQLHelper mysql;
        public static SQLHelper GetSQLHelperInstance(DBEnum DbType, string args)
        {

            mysql = (SQLHelper)CacheHelper.GetCache(DbType.ToString().ToLower() + "_" + args);
            if (mysql == null)
            {
                //根据配置信息获取命名空间
                string DBNameSpace = CommonHelper.GetAppSetting("DBNameSpace");
                if (string.IsNullOrEmpty(DBNameSpace))
                {
                    DBNameSpace = "WiteemFramework.DBWorks";
                }
                IEnumerable<Type> DBGroups = null;
                Type[] Assemblys = Assembly.GetExecutingAssembly().GetTypes();
                //返回命名空间的程序集
                DBGroups = Assemblys.Where(m => m.Namespace == DBNameSpace);
                foreach (Type item in DBGroups)
                {
                    if (item.Name.ToLower() == DbType.ToString().ToLower())
                    {
                        if (args == null)
                            mysql = (SQLHelper)Activator.CreateInstance(item);
                        else
                            mysql = (SQLHelper)Activator.CreateInstance(item, args);
                        CacheHelper.SetCache(DbType.ToString().ToLower() + "_" + args, mysql, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10));
                        return mysql;
                    }
                }
                return null;
            }
            return mysql;
        }
    }
}
