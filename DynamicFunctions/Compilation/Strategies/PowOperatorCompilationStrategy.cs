using System.Reflection;
using System.Reflection.Emit;
using DynamicFunctions.Compilation.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.Compilation.Strategies;

public class PowOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    private static readonly MethodInfo DoublePowMethod = typeof(Math).GetMethod(nameof(Math.Pow))!;

    private static readonly MethodInfo LongPowMethod = typeof(PowOperatorCompilationStrategy)
        .GetMethod(nameof(LongPow), BindingFlags.NonPublic | BindingFlags.Static)!;

    public Type OperatorType => typeof(PowOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        var powMethod = options.Type switch
        {
            NumberType.Double => DoublePowMethod,
            NumberType.Long => LongPowMethod,
            _ => throw new UnsupportedFunctionTypeException(options.Type.ToString()),
        };

        generator.Emit(OpCodes.Call, powMethod);
    }

    private static long LongPow(long value, long power)
    {
        if (power < 0)
        {
            // Integer semantics: |value| > 1 gives a fraction truncated to zero.
            return value switch
            {
                1 => 1,
                -1 => power % 2 == 0 ? 1 : -1,
                _ => 0,
            };
        }

        var result = 1L;

        while (power > 0)
        {
            if ((power & 1) == 1)
            {
                result *= value;
            }

            value *= value;
            power >>= 1;
        }

        return result;
    }
}
