namespace DynamicFunctions.Compilation.Exceptions;

public class UnexpectedArgsCountInFunctionException(string functionName) 
    : DynamicFunctionsException($"Unexpected count of arguments in function '{functionName}'");
