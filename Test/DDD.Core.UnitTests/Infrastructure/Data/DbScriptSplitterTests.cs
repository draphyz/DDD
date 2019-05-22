using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.Data
{
    public class DbScriptSplitterTests
    {

        #region Methods


        public static IEnumerable<object[]> ScriptsAndExpectedResults()
        {
            yield return new object[]
            {
                    "batch1\r\nGO\r\nbatch2\r\nGO\r\nbatch3",
                    "GO",
                    false,
                    new string[] { "batch1\r\n", "batch2\r\n", "batch3" }
            };
            yield return new object[]
            {
                    "batch1\nGO\nbatch2\nGO\nbatch3",
                    "GO",
                    false,
                    new string[] { "batch1\n", "batch2\n", "batch3" }
            };
            yield return new object[]
            {
                    "batch1\r\nGo\r\nbatch2\r\ngo\r\nbatch3",
                    "GO",
                    false,
                    new string[] { "batch1\r\n", "batch2\r\n", "batch3" }
            };
            yield return new object[]
            {
                "instr1\r\ninstr2\r\nGO\r\ninstr3\r\ninstr4\r\ninstr5\r\nGO\r\ninstr6",
                "GO",
                false,
                new string[] { "instr1\r\ninstr2\r\n", "instr3\r\ninstr4\r\ninstr5\r\n", "instr6" }
            };
            yield return new object[]
             {
                    "--comment1\r\nbatch1\r\nGO\r\n/*comment2*/\r\nbatch2\r\nGO\r\nbatch3",
                    "GO",
                    false,
                    new string[] { "--comment1\r\nbatch1\r\n", "/*comment2*/\r\nbatch2\r\n", "batch3" }
            };
            yield return new object[]
            {
                    "--comment1\r\nbatch1\r\nGO\r\n/*comment2*/\r\nbatch2\r\nGO\r\nbatch3",
                    "GO",
                    true,
                    new string[] { "\r\nbatch1\r\n", "batch2\r\n", "batch3" }
            };
            yield return new object[]
            {
                    "batch1\r\nGO\r\n/*comment\r\ncommentsuite*/\r\nbatch2\r\nGO\r\nbatch3",
                    "GO",
                    true,
                    new string[] { "batch1\r\n", "batch2\r\n", "batch3" }
            };
        }

        [Theory]
        [MemberData(nameof(ScriptsAndExpectedResults))]
        public static void Split_WhenScriptNotEmpty_ReturnsExpectedResults(string script,
                                                                           string batchSeparator,
                                                                           bool removeComments, 
                                                                           IEnumerable<string> expectedBatches)
        {
            // Assert
            var splitter = new DbScriptSplitter();
            // Act
            var batches = splitter.Split(script, batchSeparator, removeComments);
            // Assert
            batches.Should().Equal(expectedBatches);
        }

        #endregion Methods

    }
}
