using System;
using System.Collections.Generic;

namespace MultiTenantTemplate.Model.Core
{
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; }

        public Dictionary<string, object> Items { get; private set; } = new Dictionary<string, object>();
    }
}
