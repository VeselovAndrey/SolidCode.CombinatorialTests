namespace SolidCode.CombinatorialTests;

using System.Reflection;

internal sealed class CombinatorialDataAttributeCore
{
	public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
	{
		ParameterInfo[] methodParams = methodInfo.GetParameters();

		var values = new List<IReadOnlyList<object?>>(capacity: methodParams.Length);

		foreach (var methodParam in methodParams) {
			CombinatorialValuesBaseAttribute? combValuesAttr = methodParam.GetCustomAttribute<CombinatorialValuesBaseAttribute>();

			if (combValuesAttr is CombinatorialMemberValuesAttribute { SourceType: null } c)
				c.SourceType = methodInfo.DeclaringType;

			if (combValuesAttr is not null) {
				if (combValuesAttr.Values is { Length: > 0 })
					values.Add(combValuesAttr.Values);
				else
					throw new NotSupportedException($"Parameter '{methodParam.Name}' has an attribute '{combValuesAttr.GetType().Name}' with no values specified.");
			}
			else {
				values.Add(GetDefaultValues(methodParam.ParameterType, methodParam.Name ?? "N/A"));
			}
		}

		return GenerateCombinations(values);
	}

	private static IEnumerable<object?[]> GenerateCombinations(IReadOnlyList<IReadOnlyList<object?>> parameters)
	{
		var indexes = new int[parameters.Count];

		while (true) {
			// Build result
			var result = new object?[parameters.Count];
			for (int i = 0; i < parameters.Count; i++)
				result[i] = parameters[i][indexes[i]];

			yield return result;

			int indexToUpdate = indexes.Length - 1;
			while (true) {
				indexes[indexToUpdate]++;
				if (indexes[indexToUpdate] >= parameters[indexToUpdate].Count) {
					indexes[indexToUpdate] = 0;
					indexToUpdate--;
					if (indexToUpdate < 0)
						yield break;
				}
				else {
					break;
				}
			}
		}
	}

	private static IReadOnlyList<object?> GetDefaultValues(Type parameterType, string parameterName)
		=> parameterType switch {
			{ } t when t == typeof(bool) => CombinatorialDataDefaultValues.Bool,
			{ } t when t == typeof(bool?) => CombinatorialDataDefaultValues.BoolNullable,

			{ } t when t == typeof(byte) => CombinatorialDataDefaultValues.Byte,
			{ } t when t == typeof(byte?) => CombinatorialDataDefaultValues.ByteNullable,

			{ } t when t == typeof(short) => CombinatorialDataDefaultValues.Short,
			{ } t when t == typeof(short?) => CombinatorialDataDefaultValues.ShortNullable,

			{ } t when t == typeof(int) => CombinatorialDataDefaultValues.Int,
			{ } t when t == typeof(int?) => CombinatorialDataDefaultValues.IntNullable,

			{ } t when t == typeof(uint) => CombinatorialDataDefaultValues.UInt,
			{ } t when t == typeof(uint?) => CombinatorialDataDefaultValues.UIntNullable,

			{ } t when t == typeof(long) => CombinatorialDataDefaultValues.Long,
			{ } t when t == typeof(long?) => CombinatorialDataDefaultValues.LongNullable,

			{ } t when t == typeof(ulong) => CombinatorialDataDefaultValues.ULong,
			{ } t when t == typeof(ulong?) => CombinatorialDataDefaultValues.ULongNullable,

			{ } t when t == typeof(float) => CombinatorialDataDefaultValues.Float,
			{ } t when t == typeof(float?) => CombinatorialDataDefaultValues.FloatNullable,

			{ } t when t == typeof(double) => CombinatorialDataDefaultValues.Double,
			{ } t when t == typeof(double?) => CombinatorialDataDefaultValues.DoubleNullable,

			{ } t when t == typeof(decimal) => CombinatorialDataDefaultValues.Decimal,
			{ } t when t == typeof(decimal?) => CombinatorialDataDefaultValues.DecimalNullable,

			{ } t when t == typeof(char) => CombinatorialDataDefaultValues.Char,
			{ } t when t == typeof(char?) => CombinatorialDataDefaultValues.CharNullable,

			{ IsEnum: true } t => Enum.GetValues(t).Cast<object?>().ToList(),

			_ => throw new NotSupportedException($"Not supported parameter type: {parameterType.FullName}. Parameter: {parameterName}")
		};
}

/// <summary>Contains default values for different parameter types.</summary>
internal static class CombinatorialDataDefaultValues
{
	public static object?[] Bool { get; } = [false, true];
	public static object?[] BoolNullable { get; } = [(bool?)false, (bool?)true, (bool?)null];

	public static object?[] Byte { get; } = [(byte)0, (byte)1];
	public static object?[] ByteNullable { get; } = [(byte?)0, (byte?)1, (byte?)null];

	public static object?[] Short { get; } = [(short)-1, (short)0, (short)1];
	public static object?[] ShortNullable { get; } = [(short?)-1, (short?)0, (short?)1, (short?)null];

	public static object?[] Int { get; } = [-1, 0, 1];
	public static object?[] IntNullable { get; } = [(int?)-1, (int?)0, (int?)1, (int?)null];

	public static object?[] UInt { get; } = [0U, 1U];
	public static object?[] UIntNullable { get; } = [(uint?)0, (uint?)1, (uint?)null];

	public static object?[] Long { get; } = [-1L, 0L, 1L];
	public static object?[] LongNullable { get; } = [(long?)-1, (long?)0, (long?)1, (long?)null];

	public static object?[] ULong { get; } = [0UL, 1UL];
	public static object?[] ULongNullable { get; } = [(ulong?)0UL, (ulong?)1UL, (ulong?)null];

	public static object?[] Float { get; } = [-1f, 0f, 1f];
	public static object?[] FloatNullable { get; } = [(float?)-1f, (float?)0f, (float?)1f, (float?)null];

	public static object?[] Double { get; } = [-1d, 0d, 1d];
	public static object?[] DoubleNullable { get; } = [(double?)-1d, (double?)0d, (double?)1d, (double?)null];

	public static object?[] Decimal { get; } = [-1m, 0m, 1m];
	public static object?[] DecimalNullable { get; } = [(decimal?)-1m, (decimal?)0m, (decimal?)1m, (decimal?)null];

	public static object?[] Char { get; } = ['a', 'z'];
	public static object?[] CharNullable { get; } = [(char?)'a', (char?)'z', (char?)null];
}