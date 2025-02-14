using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLViz.API.Models
{
    public class ParsedQuery
    {
        public List<string> Tables { get; set; } = new List<string>();
        public List<TableRelationship> Relationships { get; set; } = new List<TableRelationship>();
    }
}






