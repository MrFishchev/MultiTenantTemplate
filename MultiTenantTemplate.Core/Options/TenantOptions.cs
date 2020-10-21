using Microsoft.Extensions.Options;

namespace MultiTenantTemplate.Core.Options
{
    /// <summary>
    /// Make IOptions tenant aware
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class TenantOptions<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions>
        where TOptions : class, new()
    {
        #region Fields

        private readonly IOptionsFactory<TOptions> _factory;
        private readonly IOptionsMonitorCache<TOptions> _cache;

        #endregion

        #region Properties

        public TOptions Value => Get(Microsoft.Extensions.Options.Options.DefaultName);

        #endregion

        #region Constructor

        public TenantOptions(IOptionsFactory<TOptions> factory, IOptionsMonitorCache<TOptions> cache)
        {
            _factory = factory;
            _cache = cache;
        }

        #endregion

        #region Public Methods

        public TOptions Get(string name)
        {
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }

        #endregion
    }
}
