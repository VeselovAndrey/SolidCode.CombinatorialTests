namespace SolidCode.CombinatorialTests.Core.Tests;

using System.Reflection;

public sealed class CombinatorialRangeValuesAttributeTests
{
	[Fact]
	public void CombinatorialRangeValuesAttribute_WhenIntRangeProvided_GenerateIntValues()
	{
		// Arrange

#if NET8_0_OR_GREATER
		// Act
		var attribute = new CombinatorialRangeValuesAttribute<int>(start: -10, max: 10, step: 2);
#else
		// Act
		var attribute = new CombinatorialRangeValuesAttribute(start: -10, max: 10, step: 2);
#endif

		// Assert
		Assert.Equal(
			expected: new object?[] { -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10 },
			actual: attribute.Values);
	}

	[Fact]
	public void CombinatorialRangeValuesAttribute_WhenIntRangeProvided_ButStepIsNotProportional_GenerateIntValues()
	{
		// Arrange

		// Act
#if NET8_0_OR_GREATER
		// Act
		var attribute = new CombinatorialRangeValuesAttribute<ulong>(start: 10UL, max: 20UL, step: 3UL);
#else
		// Act
		var attribute = new CombinatorialRangeValuesAttribute(start: 10UL, max: 20UL, step: 3UL);
#endif

		// Assert
		Assert.Equal(
			expected: new object?[] { 10UL, 13UL, 16UL, 19UL },
			actual: attribute.Values);
	}

	[Theory]
	[InlineData(20U, 10U)]
	[InlineData(20U, 20U)]
	public void CombinatorialRangeValuesAttribute_WhenMaxLessThanOrEqualStart_ArgumentExceptionThrown(uint start, uint max)
	{
		// Arrange

#if NET8_0_OR_GREATER
		// Act & Assert
		Assert.Throws<ArgumentException>(() => new CombinatorialRangeValuesAttribute<uint>(start, max, step: 1));
#else
		// Act & Assert
		Assert.Throws<ArgumentException>(() => new CombinatorialRangeValuesAttribute(start, max, step: 1));
#endif
	}
}
