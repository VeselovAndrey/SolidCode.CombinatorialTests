namespace SolidCode.CombinatorialTests.XUnit;

using System.Reflection;
using Xunit.Sdk;

/// <summary>Represents an attribute that provides combinatorial test data for a test method.</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CombinatorialDataAttribute : DataAttribute
{
	private readonly CombinatorialDataAttributeCore _core = new CombinatorialDataAttributeCore();

	/// <inheritdoc />
	public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
		=> _core.GetData(testMethod);
}
