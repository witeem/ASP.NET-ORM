using System;

namespace WiteemFramework.Filter
{
    /// <summary>
    /// 实体映射表字段
    /// </summary>\
    [AttributeUsage(AttributeTargets.All,AllowMultiple =true,Inherited =true)]
    public class DataFieldAttribute : Attribute
    {
        private string _fieldName;
        private string _fieldType;

        public DataFieldAttribute(string fieldname, string fieldtype)
        {
            this.FieldName = fieldname;
            this.FieldType = fieldtype;
        }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }

            set
            {
                _fieldName = value;
            }
        }

        public string FieldType
        {
            get
            {
                return _fieldType;
            }

            set
            {
                _fieldType = value;
            }
        }
    }
}
