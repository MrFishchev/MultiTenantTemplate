using System;

namespace MultiTenantTemplate.Services
{
    public class TenantSpecificTestService
    {
        public Guid Id { get; }

        public TenantSpecificTestService()
        {
            Id = Guid.NewGuid();
        }
    }
}
