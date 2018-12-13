using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiteemFramework.Enum
{
    /// <summary>
    /// 字段状态
    /// </summary>
    [Serializable]
    [Flags]
    public enum ColumnKeyType
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Default = 1,
        /// <summary>
        /// Extend状态下，不参与读取、增加、修改
        /// </summary>
        Extend = 2,
        /// <summary>
        /// Read状态下不参与增加、修改
        /// </summary>
        Read = 3,
        /// <summary>
        /// 标记为主键
        /// </summary>
        Identity = 8
    }
}
