using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

//CODED BY: MIKE ROSOL

namespace Olo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //GET REST CALL
            using (HttpResponseMessage response = await new HttpClient().GetAsync($"http://files.olo.com/pizzas.json"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var pizzaOrders = JsonConvert.DeserializeObject<List<PizzaOrderModel>>(await response.Content.ReadAsStringAsync());

                    //COMBINATIONS
                    var uniqueCombinations = new List<string>();

                    foreach (var pizzaOrder in pizzaOrders)
                    {
                        pizzaOrder.Toppings.Sort();
                        uniqueCombinations.Add(string.Join(" ", pizzaOrder.Toppings));                                                
                    }

                    //PROCESS
                    var mostFrequentlyOrderedPizzaCombinations = uniqueCombinations
                        .GroupBy(x => x)
                        .Select(x => new
                        {
                            Toppings = x.Key,
                            OrderCount = x.Count()
                        })
                        .OrderByDescending(x => x.OrderCount)
                        .Take(20);


                    //OUTPUT
                    int rank = 1;

                    foreach (var topRankingPizzaCombination in mostFrequentlyOrderedPizzaCombinations)
                    {
                        Console.WriteLine($"Rank: {rank++}  ---> {topRankingPizzaCombination}");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Could not get json data from Olo.");
                }
            }
        }
    }
}