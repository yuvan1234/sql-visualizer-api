using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SQLViz.API.Models;

namespace SQLVisualizer.Services
{
    public class SqlParser
    {
            public ParsedQuery ParseQuery(string sqlQuery)
            {
                var result = new ParsedQuery();
                var query = sqlQuery.ToUpper();

                try
                {
                    // Parse Tables with aliases
                    var fromPattern = @"FROM\s+(\w+)(?:\s+(?:AS\s+)?(\w+))?";
                    var fromMatches = Regex.Matches(query, fromPattern);

                    // Dictionary to store table-alias mappings
                    var tableAliasMap = new Dictionary<string, string>();

                    foreach (Match match in fromMatches)
                    {
                        var tableName = match.Groups[1].Value;
                        if (!result.Tables.Contains(tableName))
                        {
                            result.Tables.Add(tableName);
                        }

                        // If there's an alias, store the mapping
                        if (match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[2].Value))
                        {
                            tableAliasMap[match.Groups[2].Value] = tableName;
                        }
                    }

                    // Parse Joins with alias handling
                    var joinPattern = @"(INNER|LEFT|RIGHT|FULL|CROSS)?\s*JOIN\s+(\w+)(?:\s+(?:AS\s+)?(\w+))?\s+ON\s+(\w+)\.(\w+)\s*=\s*(\w+)\.(\w+)";
                    var joinMatches = Regex.Matches(query, joinPattern);

                    foreach (Match match in joinMatches)
                    {
                        var joinedTable = match.Groups[2].Value;
                        var joinedTableAlias = match.Groups[3].Value;

                        // Add joined table if not already added
                        if (!result.Tables.Contains(joinedTable))
                        {
                            result.Tables.Add(joinedTable);
                        }

                        // Store alias mapping if present
                        if (!string.IsNullOrEmpty(joinedTableAlias))
                        {
                            tableAliasMap[joinedTableAlias] = joinedTable;
                        }

                        var sourceAlias = match.Groups[4].Value;
                        var targetAlias = match.Groups[6].Value;

                        var relationship = new TableRelationship
                        {
                            JoinType = string.IsNullOrEmpty(match.Groups[1].Value) ? "INNER" : match.Groups[1].Value,
                            // Use actual table names instead of aliases
                            SourceTable = tableAliasMap.ContainsKey(sourceAlias) ? tableAliasMap[sourceAlias] : sourceAlias,
                            SourceColumn = match.Groups[5].Value,
                            TargetTable = tableAliasMap.ContainsKey(targetAlias) ? tableAliasMap[targetAlias] : targetAlias,
                            TargetColumn = match.Groups[7].Value
                        };

                        result.Relationships.Add(relationship);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error parsing SQL query: {ex.Message}");
                }
            }
        }
    }
