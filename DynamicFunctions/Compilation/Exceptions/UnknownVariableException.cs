namespace DynamicFunctions.Compilation.Exceptions;

public class UnknownVariableException(string variableName) 
    : DynamicFunctionsException($"Unknown variable name: '{variableName}'");