using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

namespace Models.Patterns.Facade
{
    public class RandomNumberSequence
    {
        public List<int> GetSequence(int min, int max)
        {

            var list = new List<int>();
            using var client = new HttpClient();
            var response = client.GetAsync("https://www.random.org/sequences/?min=" + min + "&max=" + max + "&col=1&format=plain&rnd=new").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent.ReadAsStringAsync().Result;
                var spliter = responseString.Split('\n');
                var segments = spliter.Take(spliter.Count() - 1).ToArray();
                
                // Переводим все сегменты страницы полученные от api в int
                foreach (var segment in segments)
                {
                    list.Add(int.Parse(segment));
                }
                
            }
            return list;
        }
    }
}