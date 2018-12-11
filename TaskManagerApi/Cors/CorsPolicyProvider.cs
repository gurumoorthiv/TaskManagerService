using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using System.Diagnostics.CodeAnalysis;
namespace ProjectManager.API.Security
{
    [ExcludeFromCodeCoverage]
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private CorsPolicy policy;
        private const string CORSKey = "CorsAccessDomains";

        public CorsPolicyProvider()
        {
            policy = new CorsPolicy() { AllowAnyMethod = true, AllowAnyHeader = true };
            policy.Methods.Add("GET");
            policy.Methods.Add("POST");
            policy.Methods.Add("PUT");
            policy.Methods.Add("DELETE");
            policy.Methods.Add("OPTIONS");



            if (System.Web.Configuration.WebConfigurationManager.AppSettings.AllKeys.Any(x => x == "CorsDomains"))
            {
                foreach(var x in System.Web.Configuration.WebConfigurationManager.AppSettings["CorsDomains"].Split('|'))
                {
                    policy.Origins.Add(x);
                }
            }

        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            return System.Threading.Tasks.Task.FromResult(policy);
        }
    }
}