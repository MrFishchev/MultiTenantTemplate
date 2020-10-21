using System;

namespace MultiTenantTemplate.Services
{
    public class ApplicationSpecificTestService
    {
        public Guid Id { get; }

        public ApplicationSpecificTestService()
        {
            Id = new Guid();
        }
    }
}
