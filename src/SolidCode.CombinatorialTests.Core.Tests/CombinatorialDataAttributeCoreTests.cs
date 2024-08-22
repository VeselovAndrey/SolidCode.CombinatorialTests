namespace SolidCode.CombinatorialTests.Core.Tests;

using System.Reflection;

public class CombinatorialDataAttributeCoreTests
{
	[Fact]
	public void CombinatorialDataAttributeCore_GetData_ParametersHaveValidAttributes_DataGenerated()
	{
		// Arrange
		MethodInfo methodInfo = typeof(DemoTests).GetMethod(nameof(DemoTests.ValidAttributesTest))!;
		var attributeCore = new CombinatorialDataAttributeCore();

		// Act
		IEnumerable<object?[]> rawData = attributeCore.GetData(methodInfo);

		// Assert
		object?[][] data = rawData.ToArray();

		Assert.Equal(expected: 18, data.Length);
		Assert.True(Array.TrueForAll(data, d => data.Count(x => (int?)d[0] == (int?)x[0] && (string?)d[1] == (string?)x[1] && (bool?)d[2] == (bool?)x[2]) == 1)); // All combinations are unique.
	}

	[Fact]
	public void CombinatorialDataAttributeCore_GetData_ParametersHaveNoAttributesButHaveDefaults_DataGenerated()
	{
		// Arrange
		MethodInfo methodInfo = typeof(DemoTests).GetMethod(nameof(DemoTests.NoAttributesTest))!;
		var attributeCore = new CombinatorialDataAttributeCore();

		// Act
		IEnumerable<object?[]> rawData = attributeCore.GetData(methodInfo);

		// Assert
		object?[][] data = rawData.ToArray();

		Assert.Equal(expected: 12, data.Length);
		Assert.True(Array.TrueForAll(data, d => data.Count(x => (int?)d[0] == (int?)x[0] && (byte?)d[1] == (byte?)x[1] && (bool?)d[2] == (bool?)x[2]) == 1)); // All combinations are unique.
	}

	[Fact]
	public void CombinatorialDataAttributeCore_GetData_ParameterHasAttributeWithEmptyData_ExceptionThrown()
	{
		// Arrange
		MethodInfo methodInfo = typeof(DemoTests).GetMethod(nameof(DemoTests.AttributeWithEmptyDataTest))!;
		var attributeCore = new CombinatorialDataAttributeCore();

		// Act & Assert
		Assert.Throws<ArgumentException>(() => attributeCore.GetData(methodInfo));
	}

	[Fact]
	public void CombinatorialDataAttributeCore_GetData_ParameterHasAttributeWithNoData_ExceptionThrown()
	{
		// Arrange
		MethodInfo methodInfo = typeof(DemoTests).GetMethod(nameof(DemoTests.AttributeWithNoDataTest))!;
		var attributeCore = new CombinatorialDataAttributeCore();

		// Act & Assert
		Assert.Throws<ArgumentException>(() => attributeCore.GetData(methodInfo));
	}

	[Fact]
	public void CombinatorialDataAttributeCore_GetData_ParameterHasNoAttribute_ExceptionThrown()
	{
		// Arrange
		MethodInfo methodInfo = typeof(DemoTests).GetMethod(nameof(DemoTests.NoAttributeNoDefaultsTest))!;
		var attributeCore = new CombinatorialDataAttributeCore();

		// Act & Assert
		Assert.Throws<NotSupportedException>(() => attributeCore.GetData(methodInfo));
	}

	private class DemoTests
	{
		public void ValidAttributesTest(
			[CombinatorialValues(-10, 0, 10)]
			int id,

			[CombinatorialValues("N1", "N2", "N3")]
			string name,

			[CombinatorialValues(true, false)]
			bool local)
		{
		}

		public void NoAttributesTest(int id, byte code, bool local)
		{
		}

		public void AttributeWithEmptyDataTest([CombinatorialValues([])] int id)
		{
		}

		public void AttributeWithNoDataTest([CombinatorialValues] int id)
		{
		}

		public void NoAttributeNoDefaultsTest(object value)
		{
		}
	}
}

