using System.Net.Http;
using System.Web.Http.Cors;
using System.Diagnostics.CodeAnalysis;
namespace ProjectManager.API.Security
{
    [ExcludeFromCodeCoverage]
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        private CorsPolicyProvider policyProvider;

        public CorsPolicyFactory()
        {
            policyProvider = new CorsPolicyProvider();
        }

        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return policyProvider;
        }
    }
}