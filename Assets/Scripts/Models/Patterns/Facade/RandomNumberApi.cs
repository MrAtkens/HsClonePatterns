using System;
using System.Net.Http;
using UnityEngine;

namespace Models.Patterns.Facade
{
    public class RandomNumberApi
    {
        public int GetOneNumber(int min, int max)
        {
            var number = 0;
            using var client = new HttpClient();
            var response = client.GetAsync("https://www.random.org/integers/?num=1&min="+ min +"&max= " + max + "&col=1&base=10&format=plain&rnd=new").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent.ReadAsStringAsync().Result;
                number = int.Parse(responseString);
            }
            return number;
        }
    }
}