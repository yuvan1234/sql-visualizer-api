using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLViz.API.Models
{
    public class TableRelationship
    {
        public string SourceTable { get; set; }
        public string SourceColumn { get; set; }
        public string TargetTable { get; set; }
        public string TargetColumn { get; set; }
        public string JoinType { get; set; }
    }
}