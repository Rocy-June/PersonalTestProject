using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketForm.Model.SQLiteModel
{
    class FilterCommand
    {
        public FilterCommand(string key, FilterCommandOperator commandOperator, object value)
        {
            Key = key;
            CommandOperator = commandOperator;
            Value = value;
        }

        public override string ToString()
        {
            string valueString;

            var valueType = Value.GetType();
            if (valueType.IsGenericType)
            {
                var valueGenericTypeList = valueType.GetGenericArguments();
                if (valueGenericTypeList.Length != 0)
                {
                    var valueGenericType = valueGenericTypeList[0];
                    valueString = valueGenericType.IsValueType
                        ? $"({string.Join(",", Value)})"
                        : $"('{string.Join("','", Value)}')";
                }
                else
                {
                    valueString = "()";
                }
            }
            else
            {
                valueString = valueType.IsValueType
                    ? Value.ToString()
                    : $"'{Value.ToString()}'";
            }

            return $"{Key} {GetFilterCommandOperString(CommandOperator)} {valueString}";
        }



        private FilterCommandOperator CommandOperator { get; set; }

        private string Key { get; set; }

        private object Value { get; set; }

        public enum FilterCommandOperator : short
        {
            Equals = SqlOperators.Equals,                                           // 0000 0001
            NotEquals = SqlOperators.Not | SqlOperators.Equals,                     // 0001 0001
            GreaterThan = SqlOperators.GreaterThan,                                 // 0000 0010
            LessThan = SqlOperators.LessThan,                                       // 0000 0100
            GreaterThanOrEquals = SqlOperators.GreaterThan | SqlOperators.Equals,   // 0000 0011
            LessThanOrEquals = SqlOperators.LessThan | SqlOperators.Equals,         // 0000 0101
            In = SqlOperators.In,                                                   // 0000 1000
            NotIn = SqlOperators.Not | SqlOperators.In                              // 0001 1000
        }

        public enum SqlOperators : short
        {
            Equals = 1,
            GreaterThan = 2,
            LessThan = 4,
            In = 8,
            Not = 16
        }

        private string GetFilterCommandOperString(FilterCommandOperator fco)
        {
            switch (fco)
            {
                case FilterCommandOperator.Equals:
                    return "=";
                case FilterCommandOperator.NotEquals:
                    return "<>";
                case FilterCommandOperator.GreaterThan:
                    return ">";
                case FilterCommandOperator.LessThan:
                    return "<";
                case FilterCommandOperator.GreaterThanOrEquals:
                    return ">=";
                case FilterCommandOperator.LessThanOrEquals:
                    return "<=";
                case FilterCommandOperator.In:
                    return "IN";
                case FilterCommandOperator.NotIn:
                    return "NOT IN";
                default:
                    throw new NotImplementedException();
            }
        }

    }

}
