using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.Compilation;

public interface ICompiler
{
    void CompileNode(ConstantNode node);                                                                                                                        
    void CompileNode(VariableNode node);
    void CompileNode(BinaryOperatorNode node);
    void CompileNode(FunctionCallNode node);
    void Complete();
}
