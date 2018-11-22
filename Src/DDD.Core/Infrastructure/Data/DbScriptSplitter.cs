using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Used to split sql scripts in batches of statements.
    /// </summary>
    public class DbScriptSplitter
    {

        #region Methods

        public IEnumerable<string> Split(string script, string batchSeparator = "GO", bool removeComments = true)
        {
            Condition.Requires(nameof(script), script).IsNotNullOrWhiteSpace();
            Condition.Requires(nameof(batchSeparator), batchSeparator).IsNotNullOrWhiteSpace();
            if (removeComments)
                script = RemoveComments(script);
            var commands = Regex.Split(script,
                                      $@"^\s*({batchSeparator}[ \t]+[0-9]+|{batchSeparator})(?:\s+|$)",
                                      RegexOptions.IgnoreCase | RegexOptions.Multiline,
                                      TimeSpan.FromMilliseconds(1000.0));
            return commands.Where(c => !IsSeparatorOrEmpty(c, batchSeparator));
        }

        private static bool IsSeparatorOrEmpty(string command, string separator)
        {
            return command.StartsWith(separator, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(command);
        }

        private static string RemoveComments(string script)
        {
            return Regex.Replace(script,
                                 @"(--[^\r?\n]*)|(/\*[\w\W]*?(?=\*/)\*/)",
                                 string.Empty,
                                 RegexOptions.Multiline,
                                 TimeSpan.FromMilliseconds(1000.0));
        }

        #endregion Methods

    }
}
