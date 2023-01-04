using CorpayOne.TaskId.Analyzer.Tests.Verifiers;
using Microsoft.CodeAnalysis;

namespace CorpayOne.TaskId.Analyzer.Tests
{
    public class CorpayOneTaskIdAnalyzerUnitTests : DiagnosticVerifier
    {
        [Fact]
        public async Task UsesNonGenericTaskIdErrors()
        {
            await CheckRaisesDiagnosticAtLocation("NonGenericTask.cs", 15, 34);
        }

        [Fact]
        public async Task UsesGenericTaskReturningIntIdErrors()
        {
            await CheckRaisesDiagnosticAtLocation("GenericTask1.cs", 14, 16);
        }

        [Fact]
        public async Task UsesGenericTaskHiddenByArrayIdErrors()
        {
            await CheckRaisesDiagnosticAtLocation("GenericTask2.cs", 13, 21);
        }

        [Fact]
        public async Task UsesComplexObjectErrors()
        {
            await CheckRaisesDiagnosticAtLocation("GenericTask4.cs", 18, 45);
        }

        [Fact]
        public async Task UsesGenuineIdProperty()
        {
            await CheckNoDiagnosticsRaised("GenericTask3.cs");
        }

        [Fact]
        public async Task UsesGenuineIdPropertyTask5()
        {
            await CheckNoDiagnosticsRaised("GenericTask5.cs");
        }

        [Fact]
        public async Task UsesGenuineIdPropertyTask6()
        {
            await CheckNoDiagnosticsRaised("GenericTask6.cs");
        }
        private async Task CheckNoDiagnosticsRaised(string fileName)
        {
            var code = ReadTestData(fileName);

            var project = GenerateCSharpProjectFromSource(code);

            var results = await GenerateDiagnosticsForProject(project);

            Assert.Empty(results);
        }

        private async Task CheckRaisesDiagnosticAtLocation(string fileName, int line, int col)
        {
            var codeAsText = ReadTestData(fileName);

            var project = GenerateCSharpProjectFromSource(codeAsText);

            var results = await GenerateDiagnosticsForProject(project);

            var diagnostic = Assert.Single(results);

            Assert.Equal(CorpayOneTaskIdAnalyzer.DiagnosticId, diagnostic.Id);
            Assert.Equal(CorpayOneTaskIdAnalyzer.Message, diagnostic.GetMessage());
            Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);

            var pos = diagnostic.Location.GetLineSpan();
            Assert.Equal(line, pos.StartLinePosition.Line);
            Assert.Equal(col, pos.StartLinePosition.Character);
        }
    }
}