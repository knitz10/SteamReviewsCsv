using System;
using System.Text.Json;
using CsvHelper;
namespace SteamReviewsCsv
{
    class Program
    {
        /*
        AppId* (the id of the app you want to get reviews for, you can get it from the url, right after /app/)
        --use-recommended-output: Whether to use recommended (by me) output or no (turned off by default)
        --custom-output: Your own custom output (comma separated) (example: "Author,ReviewText,TimestampCreated")
        --custom-filters: Your own custom filters for the query (comma separated) (not recommended) (use Steam docs for reference: https://partner.steamgames.com/doc/store/getreviews)
        Read README.md for more info
        *required */
        static async Task Main(string[] args)
        {
            int appId = 440;
            bool useRecommendedOutput = false;
            CustomOutput customOutput = new(new List<string>());
            string customFilters = ",,,,,,,";
            int[] AvailableArgs = [0, 1, 2, 3, 4, 5];

            if (args.Length < 1) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"You forgot to put in the AppID!"); Console.ResetColor(); Environment.Exit(1); }

            if (args.Contains("help")) { Console.WriteLine($"AppId* (the id of the app you want to get reviews for, you can get it from the url, right after /app/)\n--use-recommended-output: Whether to use recommended (by me) output or no (turned off by default)\n--custom-output: Your own custom output (comma separated) (example: \"Author,ReviewText,TimestampCreated\")\n--custom-filters: Your own custom filters for the query (comma separated) (not recommended)(use Steam docs for reference: {Misc.TerminalURL("Steam Docs", "https://partner.steamgames.com/doc/store/getreviews#:~:text=the%20parameters%20below.-,Parameters%3A,-GET%20store.steampowered")})\nRead README.md for more info\n*required"); Environment.Exit(0); }
            if (args.Contains("--use-recommended-output"))
            {
                useRecommendedOutput = true;
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--use-recommended-output"));
            }
            if (args.Contains("--custom-output"))
            {
                customOutput = CustomOutput.Parse(args[args.ToList().IndexOf("--custom-output") + 1]);
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--custom-output"));
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--custom-output") + 1);
            }
            if (args.Contains("--custom-filters"))
            {
                customFilters = args[args.ToList().IndexOf("--custom-filters") + 1];
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--custom-filters"));
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--custom-filters") + 1);
            }
            appId = int.Parse(args[AvailableArgs[0]]);


            HttpClient client = new();
            var response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={appId}");
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            string GameName =
            doc.RootElement
            .GetProperty(appId.ToString())
            .GetProperty("data")
            .GetProperty("name")
            .GetString()!;
            string filename = $"{GameName}";
            App app = new(appId);
            CustomFilters customFiltersClass = new("", "", "", "", "", 0, 0, 1);

            try
            {
                customFiltersClass = new CustomFilters(
                customFilters.Split(',')[0],
                customFilters.Split(',')[1],
                customFilters.Split(',')[2],
                customFilters.Split(',')[3],
                customFilters.Split(',')[4],
                int.Parse(customFilters.Split(',')[5]),
                int.Parse(customFilters.Split(',')[6]),
                int.Parse(customFilters.Split(',')[7])

                );
            }
            catch (FormatException)
            {
                customFiltersClass = new CustomFilters("", "", "", "", "", 0, 0, 1);
            }
            foreach (string field in customFilters.Split(','))
            {
                Console.WriteLine($"{field}");

            }


            var Reviews = await app.GetReviews(customFiltersClass);
            Csv.SaveCsv(Reviews, $"{GameName}", useRecommendedOutput, customOutput);
        }
    }


}