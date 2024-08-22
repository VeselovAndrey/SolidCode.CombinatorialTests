namespace SolidCode.CombinatorialTests;

/// <summary>Represents a base attribute that specifies a set of values for a parameter in a combinatorial test.</summary>
[AttributeUsage(AttributeTargets.Parameter)]
public abstract class CombinatorialValuesBaseAttribute : Attribute
{
	/// <summary>Gets the values for the combinatorial test.</summary>
	public abstract object?[] Values { get; }
}