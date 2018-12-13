using System;
using WiteemFramework.Enum;

namespace WiteemFramework.Filter
{
    /// <summary>
    /// 实体映射数据库表
    /// </summary>
    public class PropertyAttribute:Attribute
    {
        private string tableName;
        public ColumnKeyType columnKeyType;
        private bool isSub = false;

        /// <summary>
        /// 是否为子表
        /// 一度考虑到增删改中带子表的问题，但是没有想好具体的方案。暂时先放着吧
        /// </summary>
        public bool IsSub
        {
            get
            {
                return isSub;
            }

            set
            {
                isSub = value;
            }
        }

        public string TableName
        {
            get
            {
                return tableName;
            }

            set
            {
                tableName = value;
            }
        }

        /// <summary>
        /// 重构方法默认值
        /// </summary>
        public PropertyAttribute()
        {
            this.columnKeyType = ColumnKeyType.Default;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="tableName">表名</param>
        public PropertyAttribute(string tableName)
        {
            this.TableName = tableName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnKeyType">字段状态</param>
        public PropertyAttribute(ColumnKeyType columnKeyType)
        {
            this.columnKeyType = columnKeyType;
        }

        /// <summary>
        /// 判断实体是否对应字表
        /// </summary>
        /// <param name="isSub"></param>
        /// <param name="tableName"></param>
        public PropertyAttribute(bool isSub, string tableName)
        {
            this.columnKeyType = ColumnKeyType.Default;
            this.IsSub = true;
            this.TableName = tableName;
        }
    }
}
