namespace DynamicFunctions.Compilation.Exceptions;

public class UnsupportedOperatorException(string operatorName)
    : DynamicFunctionsException($"Unsupported operator: '{operatorName}'");
