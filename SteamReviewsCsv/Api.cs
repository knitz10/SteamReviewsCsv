using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

public class App
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
        Console.WriteLine($"Requesting: {requestUrl}");
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
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            allReviews[i].Author.ReviewID = i + 1;
    }
    
    return allReviews;
}
}

public class CustomFilters
{
    public string filter = "recent";
    public string language = "all";
    public string reviewType = "all";
    public string purchaseType = "all";
    public string dayRange = "";
    public int numPerPage = 100;
    public int startOffset = 0;
    public int filterOfftopicActivity = 0;
    public CustomFilters(string filter, string language, string reviewType, string purchaseType, string dayRange, int numPerPage, int startOffset, int filterOfftopicActivity) {
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


public class SteamResponse
{
[JsonPropertyName("success")]
public  int? Success { get; set; }


[JsonPropertyName("cursor")]
public  string? Cursor { get; set; }

[JsonPropertyName("reviews")]
public  List<Review>? Reviews { get; set; }
}

public class Review
{
public int? ID { get; set; }
[JsonPropertyName("recommendationid")]
public  string? RecommendationId { get; set; }


[JsonPropertyName("author")]
public  Author? Author { get; set; }

public string? RecommendationUrl =>
    $"https://steamcommunity.com/profiles/{Author.SteamId}/recommended/{App.AppIdStatic}/";

[JsonPropertyName("language")]
public  string? Language { get; set; }

[JsonPropertyName("review")]
public  string? ReviewText { get; set; }

[JsonPropertyName("timestamp_created")]
public  long TimestampCreated { get; set; }

public DateTime DateCreated =>
    DateTimeOffset.FromUnixTimeSeconds(TimestampCreated).LocalDateTime;

[JsonPropertyName("timestamp_updated")]
public  long TimestampUpdated { get; set; }

public DateTime DateUpdated =>
    DateTimeOffset.FromUnixTimeSeconds(TimestampUpdated).LocalDateTime;

[JsonPropertyName("voted_up")]
public  bool VotedUp { get; set; }

public string? Vote =>
    VotedUp ? "positive" : "negative";

[JsonPropertyName("votes_up")]
public  int? VotesUp { get; set; }

[JsonPropertyName("votes_funny")]
public  int? VotesFunny { get; set; }

[JsonPropertyName("weighted_vote_score")]
[Ignore]
public  JsonElement RawWeightedVoteScore { get; set; }
public double? WeightedVoteScore {
    get
    {
        return RawWeightedVoteScore.ValueKind switch
        {
            JsonValueKind.Number => RawWeightedVoteScore.GetDouble(),

            JsonValueKind.String => double.Parse(
                RawWeightedVoteScore.GetString()!,
                System.Globalization.CultureInfo.InvariantCulture),

            _ => 0
        };
    }
    set => RawWeightedVoteScore = JsonSerializer.Deserialize<JsonElement>(value.ToString());}

[JsonPropertyName("comment_count")]
public  int? CommentCount { get; set; }

[JsonPropertyName("steam_purchase")]
public  bool? SteamPurchase { get; set; }

[JsonPropertyName("received_for_free")]
public  bool? ReceivedForFree { get; set; }

[JsonPropertyName("refunded")]
public  bool? Refunded { get; set; }

[JsonPropertyName("written_during_early_access")]
public  bool? WrittenDuringEarlyAccess { get; set; }

[JsonPropertyName("primarily_steam_deck")]
public  bool? PrimarilySteamDeck { get; set; }

[JsonPropertyName("app_release_date")]
public  string? AppReleaseDate { get; set; }

public DateTime AppReleaseDateTime =>
    DateTimeOffset.FromUnixTimeSeconds(long.Parse(AppReleaseDate)).LocalDateTime;

[JsonPropertyName("reactions")]
public  List<Reaction> Reactions { get; set; }

[JsonPropertyName("hardware")]
public Hardware? Hardware { get; set; }
}
public class Hardware
{
    public int? ReviewID { get; set; }
    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("dx_video_card")]
    public string? DxVideoCard { get; set; }
    [JsonPropertyName("dx_vendorid")]
    public int? DxVendorId { get; set; }
    [JsonPropertyName("dx_deviceid")]
    public int? DxDeviceId { get; set; }
    [JsonPropertyName("num_gpu")]
    public int? NumGpu { get; set; }
    [JsonPropertyName("system_ram")]
    public string? SystemRam { get; set; }
    [JsonPropertyName("os")]
    public string? Os { get; set; }
    [JsonPropertyName("cpu_vendor")]
    public string? CpuVendor { get; set; }
    [JsonPropertyName("cpu_name")]
    public string? CpuName { get; set; }
    [JsonPropertyName("gaming_device_type")]
    public int? GamingDeviceType { get; set; }
    [JsonPropertyName("dx_driver_version")]
    public string? DxDriverVersion { get; set; }
    [JsonPropertyName("adapter_description")]
    public string? AdapterDescription { get; set; }
    [JsonPropertyName("driver_version")]
    public string? DriverVersion { get; set; }
    [JsonPropertyName("driver_date")]
    public string? DriverDateRaw { get; set; } = "";
    public DateOnly? DriverDate =>
    string.IsNullOrWhiteSpace(DriverDateRaw) || DriverDateRaw == "-1--1--1"
        ? null
        : DateOnly.Parse(DriverDateRaw);
    [JsonPropertyName("vram_size")]
    public int? VramSize { get; set; }
    [JsonPropertyName("screen_width")]
    public int? ScreenWidth { get; set; }
    [JsonPropertyName("screen_height")]
    public int? ScreenHeight { get; set; }
    [JsonPropertyName("precise_frame_rate")]
    public int? PreciseFrameRate { get; set; }
}

public class Author
{
public int? ReviewID { get; set; }

[JsonPropertyName("steamid")]
public  string? SteamId { get; set; }


[JsonPropertyName("personaname")]
public  string? PersonaName { get; set; }

[JsonPropertyName("persona_status")]
public  string? PersonaStatus { get; set; }

[JsonPropertyName("profile_url")]
public  string? ProfileUrl { get; set; }

[JsonPropertyName("num_games_owned")]
public  int? NumGamesOwned { get; set; }

[JsonPropertyName("num_reviews")]
public  int? NumReviews { get; set; }

[JsonPropertyName("playtime_forever")]
public  int? PlaytimeForever { get; set; }

[JsonPropertyName("playtime_last_two_weeks")]
public  int? PlaytimeLastTwoWeeks { get; set; }

// [JsonPropertyName("playtime_at_review")]
public int? playtime_at_review { get; set; }
public  int? PlaytimeAtReview { get {return playtime_at_review;} set { playtime_at_review = value;} }

[JsonPropertyName("last_played")]
public  long LastPlayed { get; set; }

public DateTime LastPlayedDateTime =>
    DateTimeOffset.FromUnixTimeSeconds(LastPlayed).LocalDateTime;

[JsonPropertyName("avatar")]
public  string? Avatar { get; set; }

public string? FullAvatar =>
    $"https://avatars.akamai.steamstatic.com/{Avatar}_full.jpg";


}

public class Reaction
{
[JsonPropertyName("reaction_type")]
public  int? ReactionType { get; set; }


[JsonPropertyName("count")]
public  int? Count { get; set; }


}

public class StatusErrorException : Exception
{
}

class Misc
{
public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
{
return DateTimeOffset
.FromUnixTimeSeconds(unixTimeStamp)
.LocalDateTime;
}
}
