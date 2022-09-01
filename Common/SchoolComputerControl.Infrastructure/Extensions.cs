namespace SchoolComputerControl.Infrastructure;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class RequiredMemberAttribute : Attribute
{
    public RequiredMemberAttribute() {}
}

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
public sealed class SetsRequiredMembersAttribute : Attribute
{
    public SetsRequiredMembersAttribute() {}
}