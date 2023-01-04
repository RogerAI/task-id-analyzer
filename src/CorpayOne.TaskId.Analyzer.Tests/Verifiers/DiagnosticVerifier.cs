using System.Collections.Immutable;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace CorpayOne.TaskId.Analyzer.Tests.Verifiers
{
    public abstract class DiagnosticVerifier
    {
        private static readonly MetadataReference CodeAnalysisReference =
            MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);

        private static readonly MetadataReference CorlibReference =
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

        private static readonly MetadataReference CSharpSymbolsReference =
            MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);

        private static readonly MetadataReference SystemCoreReference =
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

        private static readonly MetadataReference SystemDataCommonReference =
            MetadataReference.CreateFromFile(typeof(DbConnection).Assembly.Location);

        private static readonly MetadataReference ComponentReference =
            MetadataReference.CreateFromFile(typeof(Component).Assembly.Location);

        private static readonly MetadataReference NetStandard = MetadataReference.CreateFromFile(
            Assembly.Load("netstandard, Version=2.0.0.0").Location);

        private static readonly MetadataReference SystemRuntimeReference = MetadataReference.CreateFromFile(
            Assembly.Load("System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location);

        protected Project GenerateCSharpProjectFromSource(string sourceCode)
        {
            const string testProjectName = "TestProject001";
            var projectId = ProjectId.CreateNewId(testProjectName);

            var solution = new AdhocWorkspace().CurrentSolution.AddProject(
                    projectId,
                    testProjectName,
                    testProjectName,
                    LanguageNames.CSharp)
                .AddMetadataReference(projectId, CorlibReference)
                .AddMetadataReference(projectId, SystemCoreReference)
                .AddMetadataReference(projectId, CSharpSymbolsReference)
                .AddMetadataReference(projectId, CodeAnalysisReference)
                .AddMetadataReference(projectId, SystemDataCommonReference)
                .AddMetadataReference(projectId, NetStandard)
                .AddMetadataReference(projectId, ComponentReference)
                .AddMetadataReference(projectId, SystemRuntimeReference);

            const string filename = "TestClass0.cs";
            var documentId = DocumentId.CreateNewId(projectId, filename);
            solution = solution.AddDocument(documentId, filename, SourceText.From(sourceCode));

            return solution.GetProject(projectId)!;
        }

        protected static async Task<Diagnostic[]> GenerateDiagnosticsForProject(Project project)
        {
            var diagnostics = new List<Diagnostic>();
            var compiled = await project.GetCompilationAsync();

            if (compiled == null)
            {
                return Array.Empty<Diagnostic>();
            }

            var analyzed = compiled.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new CorpayOneTaskIdAnalyzer()));

            var resultingDiagnostics = await analyzed.GetAnalyzerDiagnosticsAsync();
            
            foreach (var diag in resultingDiagnostics)
            {
                if (diag.Location == Location.None || diag.Location.IsInMetadata)
                {
                    diagnostics.Add(diag);
                }
                else
                {
                    var documents = project.Documents.ToArray();
                    for (var i = 0; i < documents.Length; i++)
                    {
                        var document = documents[i];
                        var tree = document.GetSyntaxTreeAsync()
                            .Result;
                        if (tree == diag.Location.SourceTree)
                        {
                            diagnostics.Add(diag);
                        }
                    }
                }
            }

            return diagnostics.ToArray();
        }

        public static string ReadTestData(string testDataFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"CorpayOne.TaskId.Analyzer.Tests.TestData.{testDataFileName}";

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new ArgumentException(
                    $"No embedded resource stream exists with name: {testDataFileName}");
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
