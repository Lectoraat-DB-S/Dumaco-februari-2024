using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESv1
{
    public class ColumValue<T>
    {
        public string ColumnName { get; }
        public T? Value { get; set; }

        public ColumValue(string columnName, T value)
        {
            ColumnName = columnName;
            Value = value;
        }

        public ColumValue(string columnName)
        {
            ColumnName = columnName;
            Value = default;
        }

        public void SetValue(object value)
        {
            Value = (T)value;
        }
    }
}
