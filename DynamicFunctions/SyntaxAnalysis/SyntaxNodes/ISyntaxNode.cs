using DynamicFunctions.Compilation;

namespace DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

public interface ISyntaxNode
{
    void Accept(ISyntaxNodeCompiler visitor);
}
