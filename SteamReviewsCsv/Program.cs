using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
namespace SteamReviewsCsv
{
    class Program
    {
        /*
        AppId* (the id of the app you want to get reviews for, you can get it from the url, right after /app/. The full URL is also supported)
        --use-recommended-output: Whether to use recommended (by me) output or no (turned off by default)
        --custom-output: Your own custom output (comma separated) (example: "Author,ReviewText,TimestampCreated")
        --custom-filters: Your own custom filters for the query (comma separated) (not recommended) (use Steam docs for reference: https://partner.steamgames.com/doc/store/getreviews)
        --additional-output: Whether to use additional output (by me) or no (turned off by default)
        Read README.md for more info
        *required */
        public static bool debugMode = false;

        static async Task Main(string[] args)
        {
            int appId = 440;
            bool useRecommendedOutput = false;
            bool useAdditionalOutput = false;
            CustomOutput customOutput = new(new List<string>());
            string customFilters = ",,,,,,,";
            int[] AvailableArgs = [0, 1, 2, 3, 4, 5, 6, 7];

            if (args.Length < 1) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"You forgot to put in the AppID!"); Console.ResetColor(); Environment.Exit(1); }

            if (args.Contains("--help")) { Console.WriteLine($"AppId* (the id of the app you want to get reviews for, you can get it from the url, right after /app/. The full URL is also supported)\n--use-recommended-output: Whether to use recommended (by me) output or no (turned off by default)\n--additional-output: Whether to use additional output (by me) or no (turned off by default)\n--custom-output: Your own custom output (comma separated) (example: \"Author,ReviewText,TimestampCreated\")\n--custom-filters: Your own custom filters for the query (comma separated) (not recommended)(use Steam docs for reference: {Misc.TerminalURL("Steam Docs", "https://partner.steamgames.com/doc/store/getreviews#:~:text=the%20parameters%20below.-,Parameters%3A,-GET%20store.steampowered")})\nRead README.md for more info\n*required"); Environment.Exit(0); }
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
            if (args.Contains("--additional-output"))
            {
                useAdditionalOutput = true;
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--additional-output"));
            }
            if (args.Contains("--debug") || args.Contains("-d"))
            {
                debugMode = true;
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("-d"));
                AvailableArgs = Misc.RemoveAvailableArg(AvailableArgs, args.ToList().IndexOf("--debug"));
            }
            if (Regex.IsMatch(args[AvailableArgs[0]], @".*\/app\/\d+\/.*"))
            {
                string[] split = args[AvailableArgs[0]].Split('/');
                appId = int.Parse(split[split.ToList().IndexOf("app") + 1]);
            }
            else
            {
                appId = int.Parse(args[AvailableArgs[0]]);
            }

            // Get the game name from the Steam API
            HttpClient client = new();
            var response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={appId}");
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            string gameName =
            doc.RootElement
            .GetProperty(appId.ToString())
            .GetProperty("data")
            .GetProperty("name")
            .GetString()!;
            App app = new(appId);
            CustomFilters customFiltersClass = new("", "", "", "", "", 0, 0, 1);

            // Parse the custom filters into a CustomFilters object
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

            // Get the reviews and save them to a CSV file
            var Reviews = await app.GetReviews(customFiltersClass);
            Csv.SaveCsv(Reviews, gameName, useRecommendedOutput, customOutput, useAdditionalOutput);
        }
    }


}