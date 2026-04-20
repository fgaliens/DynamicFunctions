namespace DynamicFunctions.Compilation.Exceptions;

public class UnknownFunctionException(string functionName) 
    : DynamicFunctionsException($"Unknown function name: '{functionName}'");