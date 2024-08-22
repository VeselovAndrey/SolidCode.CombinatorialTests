namespace SolidCode.CombinatorialTests;

#if NET8_0_OR_GREATER

using System.Numerics;

/// <summary>Represents an attribute that specifies a set of range-based values for a parameter in a combinatorial test.</summary>
/// <typeparam name="TNumber">The type of the values.</typeparam>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class CombinatorialRangeValuesAttribute<TNumber> : CombinatorialValuesBaseAttribute
	where TNumber : INumber<TNumber>
{
	/// <inheritdoc />
	public override object?[] Values { get; }

	/// <summary>Initializes a new instance of the <see cref="CombinatorialRangeValuesAttribute{TNumber}"/> class.</summary>
	/// <param name="start">The start value of the data set.</param>
	/// <param name="max">The maximum possible value in the data set.</param>
	/// <param name="step">The step between values in the set.</param>
	public CombinatorialRangeValuesAttribute(TNumber start, TNumber max, TNumber step)
	{
		if (max <= start)
			throw new ArgumentException("The end value must be greater than the start value.", nameof(max));

		int items = 0;
		for (TNumber i = start; i <= max; i += step)
			items++;

		Values = new object[items];

		int item = 0;
		for (TNumber i = start; i <= max; i += step) {
			Values[item] = i;
			item++;
		}
	}
}

#else

/// <summary>Represents an attribute that specifies a set of range-based values for a parameter in a combinatorial test.</summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class CombinatorialRangeValuesAttribute : CombinatorialValuesBaseAttribute
{
	/// <inheritdoc />
	public override object?[] Values { get; }

	/// <summary>Initializes a new instance of the <see cref="CombinatorialRangeValuesAttribute"/> class.</summary>
	/// <param name="start">The start value of the data set.</param>
	/// <param name="max">The maximum possible value in the data set.</param>
	/// <param name="step">The step between values in the set.</param>
	public CombinatorialRangeValuesAttribute(int start, int max, int step)
	{
		if (max <= start)
			throw new ArgumentException("The end value must be greater than the start value.", nameof(max));

		int items = 0;
		for (int i = start; i <= max; i += step)
			items++;

		Values = new object[items];

		int item = 0;
		for (int i = start; i <= max; i += step) {
			Values[item] = i;
			item++;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="CombinatorialRangeValuesAttribute"/> class.</summary>
	/// <param name="start">The start value of the data set.</param>
	/// <param name="max">The maximum possible value in the data set.</param>
	/// <param name="step">The step between values in the set.</param>
	public CombinatorialRangeValuesAttribute(ulong start, ulong max, ulong step)
	{
		if (max <= start)
			throw new ArgumentException("The end value must be greater than the start value.", nameof(max));

		ulong items = 0;
		for (ulong i = start; i <= max; i += step)
			items++;

		Values = new object[items];

		int item = 0;
		for (ulong i = start; i <= max; i += step) {
			Values[item] = i;
			item++;
		}
	}
}
#endif