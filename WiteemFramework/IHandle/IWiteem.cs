using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiteemFramework.Enum;
using WiteemFramework.Handle;
using WiteemFramework.Model;

namespace WiteemFramework
{
    public interface IWiteem
    {
        int Add<T>(T obj) where T : new();
        int Add<T>(IEnumerable<T> obj) where T : new();
        int AddPrimary<T>(T obj) where T : new();
        int AddPrimary<T>(IEnumerable<T> obj) where T : new();
        int Update<T>(T obj) where T : new();
        int Update<T>(IEnumerable<T> obj) where T : new();
        int Delete<T>(T obj) where T : new();
        int Delete<T>(IEnumerable<T> obj) where T : new();
        T GetModel<T>(int id) where T : new();
        T GetModel<T>(string id) where T : new();
        T GetModel<T>(DBSearchBase DbSearch) where T : new();
        T GetModel<T>(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters) where T : new();
        List<T> Select<T>(DBSearchBase DbSearch) where T : new();
        List<T> Select<T>(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters) where T : new();
        List<T> ExecuteQueryPage<T>(DBSearchBase dbsearch, ref int totalCount) where T : new();
        int ExecuteNonQuery(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters);
        DataTable Select(DBSearchBase DbSearch);
        DataTable Select(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters);
        DataTable ExecuteQueryPage(DBSearchBase dbsearch, ref int totalCount);
        object ExecuteScalar(string sqlstr, CommandType commandType, IEnumerable<DbParameter> parameters);
        bool ExecuteTransaction(Dictionary<string, IEnumerable<DbParameter>> dic);
    }
}
