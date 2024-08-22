namespace SolidCode.CombinatorialTests;

using System.Reflection;

/// <summary>Represents an attribute that provides combinatorial values for a property.</summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class CombinatorialMemberValuesAttribute : CombinatorialValuesBaseAttribute
{
	private readonly Lazy<object?[]> _objects;

	internal Type? SourceType { get; set; }

	private readonly string _memberName;

	/// <inheritdoc />
	public override object?[] Values => _objects.Value;

	/// <summary>Initializes a new instance of the <see cref="CombinatorialMemberValuesAttribute"/> class.</summary>
	/// <param name="sourceType">The type that contains the <paramref name="memberName"/>.</param>
	/// <param name="memberName">The name of the property or method that will provide values.</param>
	public CombinatorialMemberValuesAttribute(Type sourceType, string memberName)
	{
		SourceType = sourceType;
		_memberName = memberName;
		_objects = new Lazy<object?[]>(() => GetValues());
	}

	/// <summary>Initializes a new instance of the <see cref="CombinatorialMemberValuesAttribute"/> class.</summary>
	/// <param name="memberName">The name of the property or method that will provide values.</param>
	public CombinatorialMemberValuesAttribute(string memberName)
	{
		SourceType = null;
		_memberName = memberName;
		_objects = new Lazy<object?[]>(() => GetValues());
	}

	private object?[] GetValues()
	{
		if (SourceType is null)
			throw new InvalidOperationException("The source type must be provided.");

		MemberInfo member = SourceType.GetMember(_memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
								 .FirstOrDefault()
							 ?? throw new InvalidOperationException($"The member '{_memberName}' was not found on type '{SourceType.FullName}'.");

		if (member is PropertyInfo propertyInfo) {
			if (propertyInfo.GetMethod is null)
				throw new InvalidOperationException($"The property '{_memberName}' on type '{SourceType.FullName}' does not have a getter.");

			object target = propertyInfo.GetMethod.IsStatic
					? SourceType
					: Activator.CreateInstance(SourceType)
					  ?? throw new InvalidOperationException($"Could not create an instance of type '{SourceType.FullName}'.");

			object?[]? values = propertyInfo.GetValue(target) as object?[];
			if (values is null || values.Length < 1)
				throw new InvalidOperationException($"The property '{_memberName}' on type '{SourceType.FullName}' did not return an array.");

			return values;
		}

		if (member is MethodInfo methodInfo) {
			object target = methodInfo.IsStatic
				? SourceType
				: Activator.CreateInstance(SourceType)
				  ?? throw new InvalidOperationException($"Could not create an instance of type '{SourceType.FullName}'.");

			object?[]? values = (object?[]?)methodInfo.Invoke(target, []);
			if (values is null || values.Length < 1)
				throw new InvalidOperationException($"The method '{_memberName}' on type '{SourceType.FullName}' did not return an array.");

			return values;
		}

		if (member is FieldInfo fieldInfo) {
			object target = fieldInfo.IsStatic
				? SourceType
				: Activator.CreateInstance(SourceType)
				  ?? throw new InvalidOperationException($"Could not create an instance of type '{SourceType.FullName}'.");

			object?[]? values = fieldInfo.GetValue(target) as object?[];
			if (values is null || values.Length < 1)
				throw new InvalidOperationException($"The field '{_memberName}' on type '{SourceType.FullName}' did not return an array.");

			return values;
		}

		throw new InvalidOperationException($"The member '{_memberName}' on type '{SourceType.FullName}' is not a property, method, or field.");
	}
}