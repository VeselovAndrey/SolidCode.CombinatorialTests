namespace SolidCode.CombinatorialTests.MSTest;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Represents an attribute that provides combinatorial test data for a test method.</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CombinatorialDataAttribute : Attribute, ITestDataSource
{
	private readonly CombinatorialDataAttributeCore _core = new CombinatorialDataAttributeCore();

	/// <inheritdoc/>
	public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
		=> _core.GetData(methodInfo);

	/// <inheritdoc/>
	public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
	{
		if (data is null)
			return methodInfo.Name;

		ParameterInfo[] parameters = methodInfo.GetParameters();

		if (parameters.Length != data.Length)
			throw new InvalidOperationException("The number of parameters does not match the number of data elements.");

		var sb = new StringBuilder();

		sb.Append(methodInfo.Name);
		sb.Append("(");

		for (int i = 0; i < data.Length; i++) {
			sb.Append(parameters[i].Name);
			sb.Append(": ");
			sb.Append(data[i]?.ToString() ?? "null");
			if (i < data.Length - 1)
				sb.Append(", ");
		}

		sb.Append(")");

		return sb.ToString();
	}
}