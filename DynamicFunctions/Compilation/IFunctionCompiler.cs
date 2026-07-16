using System.Reflection.Emit;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.Compilation;

public interface IFunctionCompiler
{
    DynamicMethod Compile(ISyntaxNode syntaxNodesTree, CompilationRequest request);
}
