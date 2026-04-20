using DynamicFunctions.Compilation;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

public sealed class ConstantNode : ISyntaxNode
{
    public ConstantNode(NumberToken token)
    {
        Token = token;
    }

    public NumberToken Token { get; }
    
    public void Accept(ICompiler compiler)
    {
        compiler.CompileNode(this);
    }
}
