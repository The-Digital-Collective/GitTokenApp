using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;


namespace GitTokenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitTokenAppController : ControllerBase
    {  
        [HttpGet]
        public async Task<IActionResult> GitTokenApp(string gitHubUri, string gitHubAccessToken)
        {            
            JsonResult errorContent;
            string strAuthHeader;            
            try
            {
                if (string.IsNullOrEmpty(gitHubUri))
                {
                    errorContent = new JsonResult("Please pass the GitHub raw file content URI (raw.githubusercontent.com) in the request URI string");
                    return NotFound(errorContent);
                }
                else if (string.IsNullOrEmpty(gitHubAccessToken))
                {
                    errorContent = new JsonResult("Please pass the GitHub personal access token in the request URI string");
                    return NotFound(errorContent);
                }
                else
                {
                    strAuthHeader = "token " + gitHubAccessToken;
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        client.DefaultRequestHeaders.Add("Authorization", strAuthHeader);
                        using (var response = await client.GetAsync(gitHubUri))
                        {
                            using (var content = response.Content)
                            {
                                var result = await content.ReadAsStringAsync();                              
                                return Ok(JsonConvert.DeserializeObject(result));
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                errorContent = new JsonResult("Error Occurred while processing the request :"+ ex.Message);
                return BadRequest(errorContent);
            }
        }
    }
}