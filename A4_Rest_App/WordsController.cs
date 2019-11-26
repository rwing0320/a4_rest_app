
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Serialization.Json;

namespace A4_Rest_App
{
    public class WordsController : Controller
    {

        [HttpGet]
        [Route("/api/route")]
        [Obsolete]
        public string GetDotNetCountAsync()
        {
            var user = HttpContext.User;
            Boolean isSignedIn = user.Identity.IsAuthenticated;

            if(isSignedIn == true)
            {
                var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=margarita");
                var request = new RestRequest(Method.GET);
                //request.AddHeader("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");
                //request.AddHeader("key", "1");
                IRestResponse response = client.Execute(request);

                string data = response.Content;

                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                object test;
                if (values.TryGetValue("drinks", out test)) // Returns true.
                {
                    Console.WriteLine(test); // This is the value at key1.



                    return JsonConvert.SerializeObject(test);
                }
                return "";
            }

            return "";

        }
    }
}

