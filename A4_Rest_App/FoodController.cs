using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace A4_Rest_App
{
    public class SuperheroController : Controller
    {

        [HttpGet]
        [Route("/api/food/random")]
        [Obsolete]
        public string GetRandomMealAsync(string foodName)
        {

            Dictionary<int, Dictionary<string, string>> newDictionary = new Dictionary<int, Dictionary<string, string>>();

            int counter = 1;


            var user = HttpContext.User;
            Boolean isSignedIn = user.Identity.IsAuthenticated;

            if (isSignedIn == true)
            {
                string url = "https://www.themealdb.com/api/json/v1/1/random.php";
              
                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                string data = response.Content;

                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                if (values.ContainsKey("meals"))
                {
                    string myFood = getFoodInfo(values);
                    return myFood;

                }
                else
                {
                    return "";
                }

            }

            return "";

        }


        [HttpGet]
        [Route("/api/food/letterlist")]
        [Obsolete]
        public string GetFoodByLetter(string letter)
        {
            int counter = 1;


            var user = HttpContext.User;
            Boolean isSignedIn = user.Identity.IsAuthenticated;

            if (isSignedIn == true)
            {
                string url = "https://www.themealdb.com/api/json/v1/1/search.php?f=" + letter;

                var client = new RestClient(url);

                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                string data = response.Content;

                Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                try
                {
                    if (values.ContainsKey("meals"))
                    {
                        string myFood = getFoodInfo(values);
                        return myFood;

                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception e)
                {
                    return "";
                }

            }

            return "";
        }

        public string getFoodInfo(Dictionary<string, object> values)
        {
            string[] keyLookingFor = { "idMeal", "strMeal", "strCategory", "strArea", "strInstructions", "strTags", "strIngredient" };

            Dictionary<int, Dictionary<string, string>> newDictionary = new Dictionary<int, Dictionary<string, string>>();
            int counter = 1;

            try
            {
                foreach (var item in values)
                {

                    IEnumerable list = item.Value as IEnumerable;


                    foreach (Object element in list)
                    {

                        Dictionary<string, string> newVal = JsonConvert.DeserializeObject<Dictionary<string, string>>(element.ToString());

                        Dictionary<string, string> keyValuePairDict = new Dictionary<string, string>();

                        for (var i = 0; i < keyLookingFor.Length; i++)
                        {
                            if (i < 6)
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
            }
            catch (Exception e)
            {
                return "";
            }
            return JsonConvert.SerializeObject(newDictionary);


        }



        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
