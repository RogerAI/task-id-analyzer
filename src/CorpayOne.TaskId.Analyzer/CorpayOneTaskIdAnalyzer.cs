using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CorpayOne.TaskId.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CorpayOneTaskIdAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "TaskIdAnalyzer";

        public const string Message = "Task Id property invoked, this was almost certainly a mistake";

        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Task.Id invocation",
            Message,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The System.Threading.Tasks.Task type has an Id property that is an int, this is often used incorrectly where await was not used.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);

            context.RegisterCompilationStartAction(ctx =>
            {
                ctx.RegisterSyntaxNodeAction(IdentityUseOfTaskIdProperty, SyntaxKind.SimpleMemberAccessExpression);
            });
        }

        private void IdentityUseOfTaskIdProperty(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is MemberAccessExpressionSyntax mae))
            {
                return;
            }

            var leftPart = mae.Expression;

            if (leftPart == null)
            {
                return;
            }

            var leftPartType = context.SemanticModel.GetTypeInfo(leftPart);

            if (leftPartType.Type == null)
            {
                return;
            }

            var leftTypeName = leftPartType.Type.ToDisplayString();
            if (leftTypeName != "System.Threading.Tasks.Task")
            {
                if (leftPartType.Type.BaseType != null)
                {
                    var leftBaseTypeName = leftPartType.Type.BaseType.ToDisplayString();
                    if (leftBaseTypeName != "System.Threading.Tasks.Task")
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            var rightPart = mae.Name;

            if (rightPart == null)
            {
                return;
            }

            var rightPartType = context.SemanticModel.GetTypeInfo(rightPart);

            if (rightPartType.Type == null)
            {
                return;
            }

            var rightTypeString = rightPartType.Type.ToDisplayString();
            if (rightTypeString != "int")
            {
                return;
            }

            var memberName = rightPart.TryGetInferredMemberName();

            if (memberName == "Id")
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(Rule, mae.GetLocation()));
            }
        }
    }
}
