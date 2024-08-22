namespace SolidCode.CombinatorialTests.Core.Tests;

public sealed class CombinatorialMemberValuesAttributeTests
{
	private static object?[] GetTestDataInstance() => [new TestId(42), new TestId(35), new TestId(477)];

	private record TestId(uint Id);

	public object?[] ExampleData { get; } = GetTestDataInstance();

	public static object?[] ExampleDataStatic { get; } = GetTestDataInstance();

	public object?[] GetExampleData() => GetTestDataInstance();

	public static object?[] GetExampleDataStatic() => GetTestDataInstance();

	internal object?[] ExampleDataInternal { get; } = GetTestDataInstance();

	internal static object?[] ExampleDataInternalStatic { get; } = GetTestDataInstance();

	internal object?[] GetExampleDataInternal() => GetTestDataInstance();

	internal static object?[] GetExampleDataInternalStatic() => GetTestDataInstance();

	internal object?[] _exampleDataPrivate = GetTestDataInstance();

	internal static object?[] _exampleDataPrivateStatic = GetTestDataInstance();

	[Theory]
	[InlineData(nameof(ExampleData))]
	[InlineData(nameof(ExampleDataStatic))]
	[InlineData(nameof(GetExampleData))]
	[InlineData(nameof(GetExampleDataStatic))]
	[InlineData(nameof(ExampleDataInternal))]
	[InlineData(nameof(ExampleDataInternalStatic))]
	[InlineData(nameof(GetExampleDataInternal))]
	[InlineData(nameof(GetExampleDataInternalStatic))]
	[InlineData(nameof(_exampleDataPrivate))]
	[InlineData(nameof(_exampleDataPrivateStatic))]
	public void CombinatorialMemberValuesAttribute_WhenMethodNameProvided_GenerateValues(string memberName)
	{
		// Arrange

		// Act
		var attribute = new CombinatorialMemberValuesAttribute(sourceType: typeof(CombinatorialMemberValuesAttributeTests), memberName);

		// Assert
		Assert.Equal(expected: GetTestDataInstance(), actual: attribute.Values);
	}
}
