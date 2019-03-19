using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telegramBot_BL.Enums;

namespace telegramBot_BL.Helpers
{
    public class GithubSearchQueryBuilder
    {
        //serch cretirias
        private Dictionary<string, HashSet<string>> _conditions;
        private GithubSearchType? _type;
        private int? _first;
        private int? _last;
        private string _before;
        private string _after;

        //returned values 
        private bool _codeCount;
        private bool _issueCount;
        private bool _repositoryCount;
        private bool _userCount;
        private bool _wikiCount;
        private HashSet<string> _nodeProperties;
        private HashSet<string> _edgeProperties;
        private HashSet<GithubPageInfoProperty> _pageInfoProperties;

        public GithubSearchQueryBuilder()
        {
            _conditions = new Dictionary<string, HashSet<string>>();
            _nodeProperties = new HashSet<string>();
            _edgeProperties = new HashSet<string>();
            _pageInfoProperties = new HashSet<GithubPageInfoProperty>();

        }

        public void AddCondition(string filed, string value)
        {
            if (!_conditions.ContainsKey(filed))
                _conditions.Add(filed, new HashSet<string>());
            _conditions[filed].Add(value);
        }

        public void RemoveCondition(string filed, string value)
        {
            if (_conditions.ContainsKey(filed))
            {
                _conditions[filed].Remove(value);
                if(_conditions[filed].Count==0)
                    _conditions.Remove(filed);
            }
        }

        public void SetSearchObejctType(GithubSearchType type)
        {
            _type = type;
        }

        public void SetFirst(int? first)
        {
            if (_last != null)
                throw new ArgumentException("can set both first and last");
            _first = first;
        }

        public void SetLast(int? last)
        {
            if (_first != null)
                throw new ArgumentException("can set both first and last");
            _last = last;
        }

        public void SetBefore(string before)
        {
            _before = before;
        }

        public void SetAfter(string after)
        {
            _after = after;
        }

        public void setReturnCodeCount(bool codeCount)
        {
            _codeCount = codeCount;
        }

        public void setReturnIssueCount(bool issueCount)
        {
            _issueCount = issueCount;
        }

        public void setReturnRepositoryCount(bool repositoryCount)
        {
            _repositoryCount = repositoryCount;
        }

        public void setReturnUserCount(bool userCount)
        {
            _userCount = userCount;
        }

        public void setReturnWikiCount(bool wikiCount)
        {
            _wikiCount = wikiCount;
        }

        public void AddReturnPageInfoProperty(GithubPageInfoProperty property)
        {
            _pageInfoProperties.Add(property);
        }

        public void RemoveRetunPageInfoProperty(GithubPageInfoProperty property)
        {
            _pageInfoProperties.Remove(property);
        }

        public void AddReturnNodeProperty(string property)
        {
            _nodeProperties.Add(property);
        }

        public void RemoveReturnNodeProperty(string property)
        {
            _nodeProperties.Remove(property);
        }

        public void AddReturnEdgeProperty(string property)
        {
            _edgeProperties.Add(property);
        }

        public void RemoveReturnEdgeProperty(string property)
        {
            _edgeProperties.Remove(property);
        }

        public string build()
        {
            if (_first == null && _last == null)
                throw new ArgumentException("first or last must be set");
            if (_type == null)
                throw new ArgumentException("type must be set");


            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("query{search(");
            //add filters if any was given
            if (_conditions.Count > 0)
            {
                addConditions(queryBuilder);
                queryBuilder.Append(",");
            }
            //add type
            queryBuilder.Append(" type:");
            queryBuilder.Append(_type.ToString());
            //add first/last and before/after
            addLimits(queryBuilder);
            //finish search cretirias
            queryBuilder.Append(") {");
            //add return values
            addReturnValues(queryBuilder);
            //end query
            queryBuilder.Append("}}");
            return queryBuilder.ToString();
        }

        private void addReturnValues(StringBuilder queryBuilder)
        {
            StringBuilder returnBuilder = new StringBuilder();
            if (_codeCount)
                returnBuilder.Append(",codeCount");
            if (_issueCount)
                returnBuilder.Append(",issueCount");
            if (_repositoryCount)
                returnBuilder.Append(",repositoryCount");
            if (_userCount)
                returnBuilder.Append(",userCount");
            if (_wikiCount)
                returnBuilder.Append(",wikiCount");
            if(_nodeProperties.Count>0)
            {
                returnBuilder.Append(",nodes{... on ");
                string type = _type.ToString().ToLower();
                type =type.First().ToString().ToUpper() + type.Substring(1);
                returnBuilder.Append(type);
                returnBuilder.Append(" {");
                foreach (string prop in _nodeProperties)
                {
                    returnBuilder.Append(prop);
                    returnBuilder.Append(",");
                }
                returnBuilder.Remove(returnBuilder.Length - 1, 1);
                returnBuilder.Append("}}");
            }
            if (_edgeProperties.Count > 0)
            {
                returnBuilder.Append(",edges{");
                foreach (string prop in _edgeProperties)
                {
                    returnBuilder.Append(prop);
                    returnBuilder.Append(",");
                }
                returnBuilder.Remove(returnBuilder.Length - 1, 1);
                returnBuilder.Append("}");
            }
            returnBuilder.Remove(0, 1);
            queryBuilder.Append(returnBuilder.ToString());
        }

        private void addLimits(StringBuilder queryBuilder)
        {
            if (_first != null)
            {
                queryBuilder.Append(", first:");
                queryBuilder.Append(_first);
            }
            if (_last != null)
            {
                queryBuilder.Append(", last:");
                queryBuilder.Append(_last);
            }
            if (_before != null)
            {
                queryBuilder.Append(", before:");
                queryBuilder.Append(_before);
            }
            if (_after != null)
            {
                queryBuilder.Append(", after:");
                queryBuilder.Append(_after);
            }
        }

        private void addConditions(StringBuilder queryBuilder)
        {
            queryBuilder.Append("query:\"");
            StringBuilder filterBuilder = new StringBuilder();
            foreach (var condition in _conditions)
            {
                string values = String.Join("+", condition.Value);
                filterBuilder.Append("+");
                filterBuilder.Append(values);
                filterBuilder.Append("+in:");
                filterBuilder.Append(condition.Key);
            }
            filterBuilder.Remove(0, 1);
            queryBuilder.Append(filterBuilder.ToString());
            queryBuilder.Append("\"");
        }
    }
}
