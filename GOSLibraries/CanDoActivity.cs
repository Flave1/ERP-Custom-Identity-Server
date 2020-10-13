using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GOSLibraries
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GOSActivitAttribute : Attribute, IAsyncActionFilter
    {
        public int Activity { get; set; }
        public UserActions Action { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
               
                var userId = context.HttpContext.User?.FindFirst("userId").Value ?? string.Empty;

                IEnumerable<string> thisUserRoleIds = new List<string>();
                IEnumerable<string> thisUserRoleNames = new List<string>();
                IEnumerable<string> roleActivities = new List<string>();

                var gosGatewayClient = new HttpClient();

                gosGatewayClient.BaseAddress = new Uri("https://localhost:44362/");

                //gosGatewayClient.BaseAddress = new Uri("http://104.238.103.48:70/");
                gosGatewayClient.DefaultRequestHeaders.Accept.Clear();

                var data = new UserPermission
                {
                    UserId = userId, 
                };
            
                var jsonContent1 = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(jsonContent1);
                var byteContent = new ByteArrayContent(buffer);

                gosGatewayClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await gosGatewayClient.PostAsync("/identity/permitDetails", byteContent);

                var accountInfo =  result.Content.ReadAsStringAsync();

                var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(accountInfo);

                var buffer2 = Encoding.UTF8.GetBytes(jsonContent); 

            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
    }
}
