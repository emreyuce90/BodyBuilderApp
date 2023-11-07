using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flowner.Domain.Models {

    public interface ILogic {
        List<ILogic> Conds { get; set; }
    }

    public class Condition : ILogic {

        public List<ILogic> Conds { get; set; } = new List<ILogic>();

        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public int Index { get; private set; }

        public static Condition From(JArray array, int i) {
            var c = new Condition {
                Field = array[0].ToString(),
                Operator = $"{array[1]}",
                Value = (array[2] as JValue)?.Value,
                Index = i
            };
            return c;
        }
    }

    public class Group : ILogic {
        public bool IsAnd { get; set; } = true;
        public List<ILogic> Conds { get; set; } = new List<ILogic>();

        public void Add(ILogic logic) {
            Conds.Add(logic);
        }
    }

    /// <summary>
    /// <![CDATA[Sample: [[["LogDate",">","2020-12-07T00:00:00.000"],"or",["LogDate","<=","2020-12-08T00:00:00.000"]],"and",["Level","=","Warning"]]]]>
    /// </summary>
    public class ConditionCollection {
        public Group Group { get; set; }

        /// <summary>
        /// SQL Declaretions
        /// </summary>
        public ExpandoObject Expando { get; set; } = new ExpandoObject();

        private readonly Dictionary<string, object> _flatFieldList = new Dictionary<string, object>();
        private List<string> _excludedFilters;
        private Dictionary<string, string> _fieldNameReplacer;
        private int _index = 0;

        public void Add(JArray array, List<string> excludedFields) {
            _excludedFilters = excludedFields;
            Group = new Group();
            Expando = new ExpandoObject();
            if (array.Count > 0) {
                if (array[0] is JValue)
                    ParseCond(array, Group);
                else {
                    ParseGroup(array, Group);
                }
            }
        }

        public void SetFieldNamesReplacer(Dictionary<string, string> fieldNameReplacer) =>
            _fieldNameReplacer = fieldNameReplacer;

        public bool Contains(string fieldName) {
            return _flatFieldList.ContainsKey(fieldName);
        }

        public object GetValue(string fieldName) {
            return Contains(fieldName)
                ? _flatFieldList[fieldName]
                : null;
        }

        private void ParseCond(JArray array, Group g) {
            if (array[0] is JValue) {
                var fieldName = array[0].Value<string>();
                if (fieldName == "!")
                    return;
                var c = Condition.From(array, _index);
                if (!_excludedFilters.Contains(c.Field)) {
                    _index++;
                    g.Add(c);

                    if (!_flatFieldList.ContainsKey(c.Field))
                        _flatFieldList.Add(c.Field, c.Value);
                }
            }
        }

        private void ParseGroup(JArray array, Group g) {

            if (array[0] is JValue) {
                ParseCond(array, g);
                //var fieldName = array[0].Value<string>();
                //if (fieldName == "!")
                //    return;
                //var c = Condition.From(array, _index);
                //if (!_excludedFilters.Contains(c.Field)) {
                //    _index++;
                //    g.Add(c);

                //    if (!_flatFieldList.ContainsKey(c.Field))
                //        _flatFieldList.Add(c.Field, c.Value);
                //}
            }
            else {
                foreach (var item in array) {
                    if (item is JValue) {
                        g.IsAnd = (item as JValue).Value<string>().ToLower() == "and";
                    }
                    else if (item is JArray) {
                        if (item.HasValues) {
                            if (item[0] is JValue) {
                                ParseCond(item as JArray, g);

                                //var c = Condition.From(item as JArray, _index);
                                //if (!_excludedFilters.Contains(c.Field)) {
                                //    _index++;
                                //    g.Add(c);

                                //    if (!_flatFieldList.ContainsKey(c.Field))
                                //        _flatFieldList.Add(c.Field, c.Value);
                                //}
                            }
                            else {
                                var ng = new Group();
                                g.Add(ng);
                                ParseGroup(item as JArray, ng);
                            }
                        }
                    }
                }
            }
        }

        public string ToSqlQuery() =>
            Group == null
                ? ""
                : GetSqlString(Group);

        public string ToSqlQueryWithoutParams() =>
            Group == null
                ? ""
                : GetSqlStringWithoutParams(Group);



        private string GetSqlString(Group g) =>
            string.Join(g.IsAnd ? " AND " : " OR ", g.Conds.Select(_ => {
                if (_ is Group) {
                    var sqlString = GetSqlString(_ as Group);
                    return string.IsNullOrWhiteSpace(sqlString)
                        ? null
                        : "(" + sqlString + ")";
                }
                else {
                    var c = _ as Condition;
                    var field = c.Field;

                    // Where cond. ekleme (SqlParameters declare edecek, function'a filan parametre verirken kullanıyoruz)
                    if (field.StartsWith("@")) {
                        Expando.TryAdd(field, c.Value);
                        return null;
                    }

                    if (_excludedFilters.Contains(field))
                        return null;

                    if (_fieldNameReplacer != null && _fieldNameReplacer.ContainsKey(field))
                        field = _fieldNameReplacer[field];

                    var fieldName = Regex.IsMatch(field, @"^\w+$") ? $"[{field}]" : $"{field}";
                    var paramName = $"p{c.Index}";
                    var condition = $"{fieldName} {c.Operator} @{paramName}";

                    switch (c.Operator) {
                        case "startswith":
                            condition = $"{fieldName} LIKE @{paramName}";
                            Expando.TryAdd(paramName, $"{c.Value}%");
                            break;

                        case "endswith":
                            condition = $"{fieldName} LIKE @{paramName}";
                            Expando.TryAdd(paramName, $"%{c.Value}");
                            break;

                        case "contains":
                            condition = $"{fieldName}  LIKE @{paramName}";
                            Expando.TryAdd(paramName, $"%{c.Value}%");
                            break;

                        case "notcontains":
                            condition = $"{fieldName} NOT LIKE @{paramName}";
                            Expando.TryAdd(paramName, $"%{c.Value}%");
                            break;

                        default:
                            if (c.Value == null) {
                                condition = $"{fieldName} IS {(c.Operator == "=" ? "" : "NOT")} NULL";
                            }
                            else {
                                condition = $"{fieldName} {c.Operator} @{paramName}";
                                Expando.TryAdd(paramName, c.Value is System.DateTime time ? time.Date.ToString("yyyyMMdd HH:mm:ss") : c.Value);
                            }
                            break;
                    }

                    return condition;
                }
            }).Where(_ => _ != null));

        private string GetSqlStringWithoutParams(Group g) =>
            string.Join(g.IsAnd ? " AND " : " OR ", g.Conds.Select(_ => {
                if (_ is Group)
                    return "(" + GetSqlStringWithoutParams(_ as Group) + ")";
                else {
                    var c = _ as Condition;
                    var field = c.Field;
                    var fieldWithoutAt = field.Replace("@", "");

                    if (_excludedFilters.Contains(fieldWithoutAt))
                        return null;

                    if (_fieldNameReplacer != null && _fieldNameReplacer.ContainsKey(fieldWithoutAt))
                        field = (field.StartsWith("@") ? "@" : "") + _fieldNameReplacer[field];

                    if (_excludedFilters.Contains(field))
                        return null;

                    var paramName = $"p{c.Index}";

                    var condition = ""; // $"{field} {c.Operator} @{paramName}";

                    switch (c.Operator) {
                        case "startswith":
                            condition = $"{field} LIKE '{c.Value}%'";
                            break;

                        case "endswith":
                            condition = $"{field} LIKE '%{c.Value}";
                            break;

                        case "contains":
                            condition = $"{field} LIKE '%{c.Value}%'";
                            break;

                        case "notcontains":
                            condition = $"{field} NOT LIKE '%{c.Value}%'";
                            break;
                        default:
                            condition = c.Value == null
                                ? $"{field} IS {(c.Operator == "=" ? "" : "NOT")} NULL"
                                : $"{field} {c.Operator} '{c.Value}'";
                            break;
                    }
                    return condition;
                }
            }).Where(_ => _ != null));



    }

    /*
    public class Condition {

        public string Field { get; set; }
        public OperatorTypes Operator { get; set; }
        public object Value { get; set; }
        public bool? IsOr { get; set; }
        public string Expression { get; set; }

        public static Condition From(JArray array) {
            var c = new Condition {
                Field = array[0].ToString(),
                Operator = GetOperator($"{array[1]}"),
                Value = (array[2] as JValue).Value
            };
            return c;
        }

        public object Parse(string param) {

            var val = Value;

            Expression = "";
            switch (Operator) {
                case OperatorTypes.Equals:
                    Expression = $"([{Field}] = {param})";
                    break;
                case OperatorTypes.Contains:
                    val = $"%{Value}%";
                    Expression = $"([{Field}] LIKE {param})";
                    break;
                case OperatorTypes.StartsWith:
                    val = $"{Value}%";
                    Expression = $"([{Field}] LIKE {param})";
                    break;
                case OperatorTypes.EndsWith:
                    val = $"%{Value}";
                    Expression = $"([{Field}] LIKE {param})";
                    break;
                case OperatorTypes.DoesNotEqual:
                    Expression = $"([{Field}] <> {param})";
                    break;
                case OperatorTypes.IsLessThan:
                    Expression = $"([{Field}] < '{param})";
                    break;
                case OperatorTypes.IsGreaterThan:
                    Expression = $"([{Field}] > {param})";
                    break;
                case OperatorTypes.IsLessThanOrEqualTo:
                    Expression = $"([{Field}] <= {param})";
                    break;
                case OperatorTypes.IsGreaterThanOrEqualTo:
                    Expression = $"([{Field}] >= {param})";
                    break;
                case OperatorTypes.IsBlank:
                    Expression = $"([{Field}] = '' OR [{Field}] IS NULL)";
                    break;
                case OperatorTypes.IsNotBlank:
                    Expression = $"([{Field}] <> '' AND [{Field}] IS NOT NULL)";
                    break;
                default:
                    Expression = $"([{Field}] = {param})";
                    break;
            }
            return val;
        }

        static OperatorTypes GetOperator(string o) {
            return o switch
            {
                "=" => OperatorTypes.Equals,
                "doesnotequal" => OperatorTypes.DoesNotEqual,
                "contains" => OperatorTypes.Contains,
                "doesnotcontain" => OperatorTypes.DoesNotContain,
                "startswith" => OperatorTypes.StartsWith,
                "endswith" => OperatorTypes.EndsWith,
                "lessthen" => OperatorTypes.IsLessThan,
                "greaterthan" => OperatorTypes.IsGreaterThan,
                "lessthanorequal" => OperatorTypes.IsLessThanOrEqualTo,

                _ => OperatorTypes.Equals,
            };
        }
    }
    */

    public enum OperatorTypes {
        Equals = 1,
        Contains = 2,
        DoesNotContain = 3,
        StartsWith = 4,
        EndsWith = 5,
        DoesNotEqual = 6,
        IsLessThan = 7,
        IsGreaterThan = 8,
        IsLessThanOrEqualTo = 9,
        IsGreaterThanOrEqualTo = 10,
        IsBlank = 11,
        IsNotBlank = 12,
        IsBetween = 13,
    }
}
