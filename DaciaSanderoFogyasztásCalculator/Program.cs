using AngleSharp;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DaciaSanderoFogyasztásCalculator
{
    internal class Program
    {
        private static readonly IConfiguration config = Configuration.Default.WithDefaultLoader();
        private static readonly IBrowsingContext context = BrowsingContext.New(config);

        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var benzin = await Get95Cost();
            Console.WriteLine($"Hány km az út?");
            var read = Console.ReadLine();
            bool parse = double.TryParse(read, NumberStyles.Any, CultureInfo.InvariantCulture, out double km);
            double fogyasztas = 0;
            if (parse)
                fogyasztas = (6.5 / 100.0) * km;
            int cost = (int)(fogyasztas * benzin);
            Console.WriteLine($"Ennyit eszik: {Math.Round(fogyasztas, 2)} liter");
            Console.WriteLine($"Ennyibe kerül: {cost} Ft");
            Console.WriteLine($"Ennyibe kerül oda-vissza: {cost * 2} Ft");
            Console.WriteLine($"95-ös benzin ára: {benzin} Ft");
            Console.ReadKey();
        }

        private static async Task<int> Get95Cost()
        {
            var document = await context.OpenAsync(Url.Create("https://www.auchan.hu/benzinkutak/auchan-debrecen"));
            var element = document.QuerySelectorAll(".petrolPrice .petrolTypeItems span").FirstOrDefault(dom => dom.InnerHtml == "95");
            var text = element.ParentElement.TextContent.Trim().Split(' ').First().Substring(2);
            return int.Parse(text);
        }
    }
}