namespace SolidCode.CombinatorialTests;

/// <summary>Represents an attribute that specifies a set of values for a parameter in a combinatorial test.</summary>
/// <param name="values">An array of values for the parameter in the combinatorial test.</param>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class CombinatorialValuesAttribute(params object?[] values) : CombinatorialValuesBaseAttribute
{
	/// <summary>Gets the values for a parameter in combinatorial test.</summary>
	public override object?[] Values { get; } = 0 < values.Length
		? values
		: throw new ArgumentException("At lest one value must be provided.", nameof(values));
}