using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace SteamReviewsCsv
{
    internal class App
    {
        public static int AppIdStatic = 440;


        public App(int appId)
        {
            AppIdStatic = appId;
        }

        public async Task<List<Review>> GetReviews(CustomFilters customFilters)
        {
            if (customFilters.filter == "all") { throw new Exception("Filter cannot be set to \"all\". If you really need to use \"all\", type \"all-doesnt-work\" instead of \"all\""); }
            if (customFilters.filter == "all-doesnt-work") { customFilters.filter = "all"; }
            string filter = "recent";
            string language = "all";
            string reviewType = "all";
            string purchaseType = "all";
            int numPerPage = 100;
            int startOffset = 0;
            string dayRangeParam =
            string.IsNullOrWhiteSpace(customFilters.dayRange)
            ? ""
            : $"&day_range={customFilters.dayRange}";

            string cursor = "*";

            HttpClient client = new();

            List<Review> allReviews = [];

            while (true)
            {
                string requestUrl =
                $"https://store.steampowered.com/appreviews/{AppIdStatic}" +
                $"?json=1" +
                $"&filter={(customFilters.filter != null && customFilters.filter != "" ? customFilters.filter : filter)}" +
                $"&language={(customFilters.language != null && customFilters.language != "" ? customFilters.language : language)}" +
                $"&review_type={(customFilters.reviewType != null && customFilters.reviewType != "" ? customFilters.reviewType : reviewType)}" +
                $"&purchase_type={(customFilters.purchaseType != null && customFilters.purchaseType != "" ? customFilters.purchaseType : purchaseType)}" +
                $"&num_per_page={(customFilters.numPerPage > 0 ? customFilters.numPerPage : numPerPage)}" +
                $"{dayRangeParam}" +
                $"&start_offset={(customFilters.startOffset > 0 ? customFilters.startOffset : startOffset)}" +
                $"&filter_offtopic_activity={(customFilters.filterOfftopicActivity != 1 && customFilters.filterOfftopicActivity != 0 ? 1 : customFilters.filterOfftopicActivity)}" +
                $"&cursor={cursor}";
                // Only prints the request URL if the debugger is attached, to avoid cluttering the console in normal use
                if (Program.debugMode) { Console.WriteLine($"Requesting: {requestUrl}"); }
                var response = await client.GetAsync(requestUrl);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Steam returned {response.StatusCode} which was not OK");
                    Console.ResetColor();
                    Environment.Exit(1);
                }

                var json = await response.Content.ReadAsStringAsync();

                var steamResponse = JsonSerializer.Deserialize<SteamResponse>(json);

                if (steamResponse?.Reviews == null ||
                steamResponse.Reviews.Count == 0)
                {
                    break;
                }

                allReviews.AddRange(steamResponse.Reviews);
                if (steamResponse.Reviews.Count < numPerPage)
                {
                    break;
                }
                cursor = Uri.EscapeDataString(steamResponse.Cursor);
            }
            for (int i = 0; i < allReviews.Count; i++)
            {
                allReviews[i].ID = i + 1;
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (allReviews[i].Hardware != null) { allReviews[i].Hardware.ReviewID = i + 1; }
                allReviews[i].Author.ReviewID = i + 1;
                #pragma warning restore CS8602 // Dereference of a possibly null reference.

            }

            return allReviews;
        }
    }

    internal class CustomFilters
    {
        public string filter = "recent";
        public string language = "all";
        public string reviewType = "all";
        public string purchaseType = "all";
        public string dayRange = "";
        public int numPerPage = 100;
        public int startOffset = 0;
        public int filterOfftopicActivity = 0;
        public CustomFilters(string filter, string language, string reviewType, string purchaseType, string dayRange, int numPerPage, int startOffset, int filterOfftopicActivity)
        {
            this.filter = filter;
            this.language = language;
            this.reviewType = reviewType;
            this.purchaseType = purchaseType;
            this.dayRange = dayRange;
            this.numPerPage = numPerPage;
            this.startOffset = startOffset;
            this.filterOfftopicActivity = filterOfftopicActivity;
        }
    }

}