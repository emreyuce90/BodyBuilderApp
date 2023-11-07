using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Flowner.Domain.Models.Queries {
    public class Query {

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool RequireTotalCount { get; set; }
        public bool Paged { get; set; } = true;
        public List<QuerySort> Sort { get; set; } = new List<QuerySort>();
        public List<QueryGroup> Group { get; set; } = new List<QueryGroup>();
        public ConditionCollection Filter { get; set; }
        public List<string> Select { get; set; }
        // public string Group { get; set; }
        public int CurrentUserId { get; set; }
        public dynamic SqlParameters { get; set; }

        private string _sqlConditions;
        private string _sqlOrderBy;
        private string _sqlGroupBy;

        public string SqlCountQuery { get; private set; }

        public virtual void Load(Query query) {
            //this.Page = query.Page;
            Paged = query.Paged;
            CurrentUserId = query.CurrentUserId;
            Filter = query.Filter;
            Group = query.Group;
            //this.ItemsPerPage = query.ItemsPerPage;
            RequireTotalCount = query.RequireTotalCount;
            Skip = query.Skip;
            Sort = query.Sort;
            SqlParameters = query.SqlParameters;
            Take = query.Take;
        }

        public virtual T Load<T>(Query query) where T : Query {
            //this.Page = query.Page;
            Paged = query.Paged;
            CurrentUserId = query.CurrentUserId;
            Filter = query.Filter;
            Group = query.Group;
            //this.ItemsPerPage = query.ItemsPerPage;
            RequireTotalCount = query.RequireTotalCount;
            Skip = query.Skip;
            Sort = query.Sort;
            SqlParameters = query.SqlParameters;
            Take = query.Take;
            return (T)this;
        }

        public virtual void Prepare(Dictionary<string, object> extraParams = null, bool withDeclaretions = true) {
            if (Filter == null)
                Filter = new ConditionCollection();

            _sqlConditions = withDeclaretions ? Filter.ToSqlQuery() : Filter.ToSqlQueryWithoutParams();

            var dictionary1 = (IDictionary<string, object>)Filter.Expando;

            if (extraParams != null) {
                foreach (var item in extraParams) {
                    dictionary1.Add(item);
                }
            }

            var expando = new ExpandoObject();
            foreach (var pair in dictionary1) {
                expando.TryAdd(pair.Key, pair.Value);
            }

            if (Paged && Take > 0 && withDeclaretions) {
                // expando.TryAdd("Page", Page);
                expando.TryAdd("Take", Take);
                expando.TryAdd("Skip", Skip);
            }
            if (CurrentUserId > 0)
                expando.TryAdd("CurrentUserId", CurrentUserId);

            SqlParameters = expando;

            if (Sort != null && Sort.Count > 0) {
                _sqlOrderBy = string.Join(",", Sort.Where(_ => _.Selector != null).Select(_ => $"[{_.Selector}]{(_.Desc ? " ASC" : "")} "));
            }
            else
                _sqlOrderBy ??= "1 ASC ";

            _sqlOrderBy = _sqlOrderBy.Replace('"', ' ').Replace(';', ' ').Replace("'", "").Replace("@", "");

            if (Group != null && Group.Count > 0) {
                _sqlGroupBy = string.Join(",", Group.Where(_ => _.Selector != null).Select(_ => $"[{_.Selector}] "));
            }
        }

        public string GetSqlQueryByTableName(string tableName, string orderBy = null) {
            return GetSqlQuery($"SELECT * FROM [{tableName}]", orderBy);
        }

        public string GetSqlQuery(string sqlQuery, string orderBy = null, bool withDeclaretions = true) {

            _sqlOrderBy = orderBy;

            Prepare(null, withDeclaretions);

            var sql = "";
            var sqlCount = "";

            if (string.IsNullOrWhiteSpace(_sqlGroupBy)) {

                sql = $"SELECT {(Select?.Count > 0 ? $"[{string.Join("],[", Select.Select(_ => _.Trim()))}]" : "* ")}";
                sql += $"FROM( {sqlQuery} ) AS sqlQuery" + Environment.NewLine;

                if (!string.IsNullOrWhiteSpace(_sqlConditions))
                    sql += "WHERE " + _sqlConditions + Environment.NewLine;

                SqlCountQuery = "SELECT COUNT(0) AS [Count] FROM (" + sql + ") AS sqlQueryForCount" + Environment.NewLine;

                sqlCount = RequireTotalCount
                    ? SqlCountQuery + Environment.NewLine
                    : Environment.NewLine;

                sql += "ORDER BY " + (string.IsNullOrWhiteSpace(_sqlOrderBy) ? $" {orderBy ?? "1 DESC "} " : _sqlOrderBy);

            }
            else {
                sql = $"SELECT {_sqlGroupBy} FROM ( {sqlQuery} ) as sqlQuery GROUP BY {_sqlGroupBy}";
            }

            if (Paged && Take > 0) {
                sql += GetPagerSqlQuery() + Environment.NewLine;
            }
            if (RequireTotalCount) {
                sql += Environment.NewLine + sqlCount;
            }

            return sql;
        }

        public string GetPagerSqlQuery() {
            return Paged && Take > 0
            ? Environment.NewLine + @$" OFFSET ({Skip}) ROWS FETCH NEXT {Take} ROWS ONLY;"
            : string.Empty;
        }

    }

    public class QuerySort {
        public string Selector { get; set; }
        public bool Desc { get; set; }
    }

    public class QueryGroup {
        public string Selector { get; set; }
        public bool IsExpanded { get; set; }
    }
}
