using MultiTenantTemplate.Model.Core;

namespace MultiTenantTemplate.Core.Accessors.IAccessors
{
    /// <summary>
    /// Instead of using IHttpAccessor (easier way)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenantAccessor<T> where T : Tenant
    {
        T Tenant { get; }
    }
}
