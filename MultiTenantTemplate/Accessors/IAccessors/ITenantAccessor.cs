using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Accessors.IAccessors
{
    /// <summary>
    /// Instead of using IHttpAccessor (easier way)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface ITenantAccessor<T> where T : Tenant
    {
        T Tenant { get; }
    }
}
