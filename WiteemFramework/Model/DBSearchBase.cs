using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiteemFramework.Model
{
    public class DBSearchBase
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string DBTable { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string DBFields { get; set; }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public string DBWhere { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string DBPrimaryKey { get; set; }
        /// <summary>
        /// 当前页码(分页使用)
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页展示条数(分页使用)
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string DBOrder { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string DBSort { get; set; }
        /// <summary>
        /// 是否去重
        /// </summary>
        public bool IsDistinct { get; set; }
    }
}
