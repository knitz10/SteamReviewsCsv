using System;
using System.Text.Json;
using CsvHelper;

class Program
{
    /* 
    Argument 1: AppId* (the id of the app you want to get reviews for, you can get it from the url, right after /app/)
    Argument 2: Whether to use recommended (by me) output or no (false by default)
    Argument 3: Your own custom output (comma separated) (example: "Author,ReviewText,TimestampCreated")
    Argument 4: Your own custom filters for the query (comma separated) (not recommended)
    Read README.md for more info
    *required */
    static async Task Main(string[] args)
    {
        int appId = 440;
        bool useRecommendedOutput = false;
        CustomOutput customOutput = new CustomOutput(new List<string>());
        string customFilters = ",,,,,,,";
        try {
        if (args[0] == "help") { Console.WriteLine($"Argument 1: AppId* (the id of the app you want to get reviews for, you can get it from the url, right after /app/)\nArgument 2: Whether to use recommended (by me) output or no (false by default)\nArgument 3: Your own custom output (comma separated) (example: \"Author,ReviewText,TimestampCreated\")\nArgument 4: Your own custom filters for the query (comma separated) (not recommended)\nRead README.md for more info\n*required"); Environment.Exit(0); }
        if (args[0] != null) { appId = int.Parse(args[0]); } // else { Console.WriteLine("No AppId provided"); Environment.Exit(1); }
        if (args[1] != null) { useRecommendedOutput = bool.Parse(args[1]); }
        if (args[2] != null) { customOutput = CustomOutput.Parse(args[2]); }
        if (args[3] != null) { customFilters = args[4]; }
        } catch (IndexOutOfRangeException) {if (args.Length < 1) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"You forgot to put in the AppID!"); Console.ResetColor(); Environment.Exit(1); } }

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
        App app = new App(appId);
        CustomFilters customFiltersClass = new CustomFilters("","","","","",0,0,1);

        try {customFiltersClass = new CustomFilters(
            customFilters.Split(',')[0],
            customFilters.Split(',')[1],
            customFilters.Split(',')[2],
            customFilters.Split(',')[3],
            customFilters.Split(',')[4],
            int.Parse(customFilters.Split(',')[5]),
            int.Parse(customFilters.Split(',')[6]),
            int.Parse(customFilters.Split(',')[7])

        );} catch (FormatException)
        {
            customFiltersClass =  new CustomFilters("","","","","",0,0,1);
        }
        foreach (string field in customFilters.Split(',')) {
            Console.WriteLine($"{field}");
            
        }


        var Reviews = await app.GetReviews(customFiltersClass);
        Csv.SaveCsv(Reviews,$"{GameName}", useRecommendedOutput, customOutput);
    }
}


