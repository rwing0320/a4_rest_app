
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
using System.Collections;

namespace A4_Rest_App
{
    public class WordsController : Controller
    {

        [HttpGet]
        [Route("/api/drink")]
        [Obsolete]
        public string GetDotNetCountAsync(string drinkName)
        {
            string[] keyLookingFor = { "idDrink", "strDrink", "strCategory", "strAlcoholic","strGlass", "strInstructions", "strDrinkThumb", "strIngredient", "strMeasure"};

            // Dictionary<string, Dictionary<string, string>[]> newDictionary = new Dictionary<string, Dictionary<string, string>[]>();

            Dictionary<int, Dictionary<string, string>> newDictionary = new Dictionary<int, Dictionary<string, string>>();

            int counter = 1;


            var user = HttpContext.User;
            Boolean isSignedIn = user.Identity.IsAuthenticated;

            if(isSignedIn == true)
            {
                drinkName = drinkName.ToLower();
                //var client = new RestClient("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=margarita");
                string url = "https://www.thecocktaildb.com/api/json/v1/1/search.php?s=" + drinkName;

                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);
                //request.AddHeader("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");
                //request.AddHeader("key", "1");
                IRestResponse response = client.Execute(request);

                string data = response.Content;

                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                if (values.ContainsKey("drinks"))
                {
                    
                    foreach(var item in values)
                    {
                       
                        IEnumerable list = item.Value as IEnumerable;
                                       

                        foreach (Object element in list)
                        {                        

                            Dictionary<string, string> newVal = JsonConvert.DeserializeObject<Dictionary<string, string>>(element.ToString());

                            Dictionary<string, string> keyValuePairDict = new Dictionary<string, string>();

                            for (var i = 0; i < keyLookingFor.Length; i++)
                            {
                                if (i < 7)
                                {

                                    if (newVal.ContainsKey(keyLookingFor[i]))
                                    {
                                        if (newVal[keyLookingFor[i]] != null && newVal[keyLookingFor[i]] != "")
                                        {
                                            keyValuePairDict.Add(keyLookingFor[i], newVal[keyLookingFor[i]]);

                                            Console.WriteLine("THE ELEMENT IS: " + newVal[keyLookingFor[i]]);
                                        }
                                    }
                                }
                                else
                                {
                                    int newCount = 1;
                                  

                                    bool isNotFound = false;
                                    do
                                    {
                                        string keyName = keyLookingFor[i] + newCount;
                                        if (newVal.ContainsKey(keyName))
                                        {
                                            if (newVal[keyName] != null && newVal[keyName] != "")
                                            {
                                                keyValuePairDict.Add(keyName, newVal[keyName]);
                                                newCount++;
                                                Console.WriteLine("THE ELEMENT IS: " + newVal[keyName]);
                                            }
                                            else
                                            {
                                                isNotFound = true;
                                            }
                                        }
                                        else
                                        {
                                            isNotFound = true;
                                        }
                                    } while (isNotFound == false);

                                }
                            }

                            newDictionary.Add(counter, keyValuePairDict);
                            counter++;
                        }

                    }
                    return JsonConvert.SerializeObject(newDictionary);

                }
                else
                {
                    return "";
                }

                //Console.WriteLine("NEW DICTIONAIRY: " + JsonConvert.SerializeObject(newDictionary));

                //object test;
                //if (values.TryGetValue("drinks", out test)) // Returns true.
                //{
                //    //Console.WriteLine(test); // This is the value at key1.



                //    return JsonConvert.SerializeObject(test);
                //}
                //return "";
            }

            return "";

        }
    }
}

