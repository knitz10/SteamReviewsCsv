using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace SteamReviewsCsv
{

    internal class SteamResponse
    {
        [JsonPropertyName("success")]
        public int? Success { get; set; }

        [JsonPropertyName("cursor")]
        public string? Cursor { get; set; }

        [JsonPropertyName("reviews")]
        public List<Review>? Reviews { get; set; }
    }

    internal class Review
    {
        public int? ID { get; set; }
        [JsonPropertyName("recommendationid")]
        [Name("Recommendation ID")]
        public string? RecommendationId { get; set; }

        [JsonPropertyName("author")]
        [Name("Author")]
        public Author? Author { get; set; }

        [Name("Recommendation URL")]
        public string? RecommendationUrl =>
        $"https://steamcommunity.com/profiles/{Author.SteamId}/recommended/{App.AppIdStatic}/";

        [JsonPropertyName("language")]
        [Name("Language")]
        public string? Language { get; set; }

        [JsonPropertyName("review")]
        [Name("Review Text")]
        public string? ReviewText { get; set; }

        [JsonPropertyName("timestamp_created")]
        [Ignore]
        public long TimestampCreated { get; set; }
        [Name("Date Created")]
        public DateTime DateCreated =>
        DateTimeOffset.FromUnixTimeSeconds(TimestampCreated).LocalDateTime;

        [JsonPropertyName("timestamp_updated")]
        [Ignore]
        public long TimestampUpdated { get; set; }
        [Name("Date Updated")]
        public DateTime DateUpdated =>
        DateTimeOffset.FromUnixTimeSeconds(TimestampUpdated).LocalDateTime;

        [JsonPropertyName("voted_up")]
        [Ignore]
        public bool VotedUp { get; set; }
        [Name("Vote")]
        public string? Vote =>
        VotedUp ? "positive" : "negative";

        [JsonPropertyName("votes_up")]
        [Name("Votes Up")]
        public int? VotesUp { get; set; }

        [JsonPropertyName("votes_funny")]
        [Name("Votes Funny")]
        public int? VotesFunny { get; set; }

        [JsonPropertyName("weighted_vote_score")]
        [Ignore]
        public JsonElement RawWeightedVoteScore { get; set; }
        [Name("Weighted Vote Score")]
        public double? WeightedVoteScore
        {
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
            set => RawWeightedVoteScore = JsonSerializer.Deserialize<JsonElement>(value.ToString());
        }

        [JsonPropertyName("comment_count")]
        [Name("Comment Count")]
        public int? CommentCount { get; set; }

        [JsonPropertyName("steam_purchase")]
        [Name("Steam Purchase")]
        public bool? SteamPurchase { get; set; }

        [JsonPropertyName("received_for_free")]
        [Name("Received for Free")]
        public bool? ReceivedForFree { get; set; }

        [JsonPropertyName("refunded")]
        [Name("Refunded")]
        public bool? Refunded { get; set; }

        [JsonPropertyName("written_during_early_access")]
        [Name("Written During Early Access")]
        public bool? WrittenDuringEarlyAccess { get; set; }

        [JsonPropertyName("primarily_steam_deck")]
        [Name("Primarily Steam Deck")]
        public bool? PrimarilySteamDeck { get; set; }

        [JsonPropertyName("app_release_date")]
        [Ignore]
        public string? AppReleaseDate { get; set; }
        [Name("App Release Date")]
        [Ignore] // For now I have set it to "ignore" because there is quite literally no reason to see it, it's completely and utterly useless for reviews. If enough people want to get it back, either rebuild the project and delete this line or make a PR, if it gets enough votes then I will bring this back. Weirdo...
        public DateTime AppReleaseDateTime =>
        DateTimeOffset.FromUnixTimeSeconds(long.Parse(AppReleaseDate)).LocalDateTime;

        [JsonPropertyName("reactions")]
        [Ignore]
        public List<Reaction>? Reactions { get; set; }
        [Name("Reactions")]
        public static string? ReactionsString = null; // I'm too lazy to make it properly :P
        public NamedReactions? NamedReactions { get; set; }

        [JsonPropertyName("hardware")]
        [Name("Hardware")]
        public Hardware? Hardware { get; set; }
    }

    internal class NamedReactions
    {
        [Name("Clever")]
        public int Clever { get; set; }
        [Name("Warm Blanket")]
        public int WarmBlanket { get; set; }
        [Name("Saucy")]
        public int Saucy { get; set; }
        [Name("Slow Clap")]
        public int SlowClap { get; set; }
        [Name("Take My Points")]
        public int TakeMyPoints { get; set; }
        [Name("Wholesome")]
        public int Wholesome { get; set; }
        [Name("Jester")]
        public int Jester { get; set; }
        [Name("Fancy Pants")]
        public int FancyPants { get; set; }
        [Name("Whoa")]
        public int Whoa { get; set; }
        [Name("Super Star")]
        public int SuperStar { get; set; }
        [Name("Wild")]
        public int Wild { get; set; }
        [Name("Winner")]
        public int Winner { get; set; }
        [Name("Beautiful")]
        public int Beautiful { get; set; }
        [Name("Helpful")]
        public int Helpful { get; set; }
        [Name("Fire")]
        public int Fire { get; set; }
        [Name("Funny")]
        public int Funny { get; set; }
        [Name("One Hundred")]
        public int OneHundred { get; set; }
        [Name("Life Saver")]
        public int LifeSaver { get; set; }
        [Name("Perfect")]
        public int Perfect { get; set; }
        [Name("Plus One")]
        public int PlusOne { get; set; }
        [Name("Smart")]
        public int Smart { get; set; }
        [Name("Pure Gold")]
        public int PureGold { get; set; }
    }
    internal class Hardware
    {
        public int? ReviewID { get; set; }
        [JsonPropertyName("manufacturer")]
        [Name("Manufacturer")]
        public string? Manufacturer { get; set; }

        [JsonPropertyName("model")]
        [Name("Model")]
        public string? Model { get; set; }

        [JsonPropertyName("dx_video_card")]
        [Name("DX Video Card")]
        public string? DxVideoCard { get; set; }

        [JsonPropertyName("dx_vendorid")]
        [Name("DX Vendor ID")]
        public int? DxVendorId { get; set; }

        [JsonPropertyName("dx_deviceid")]
        [Name("DX Device ID")]
        public int? DxDeviceId { get; set; }

        [JsonPropertyName("num_gpu")]
        [Name("Num GPU")]
        public int? NumGpu { get; set; }

        [JsonPropertyName("system_ram")]
        [Name("System RAM")]
        public string? SystemRam { get; set; }

        [JsonPropertyName("os")]
        [Name("Operating System")]
        public string? Os { get; set; }

        [JsonPropertyName("cpu_vendor")]
        [Name("CPU Vendor")]
        public string? CpuVendor { get; set; }

        [JsonPropertyName("cpu_name")]
        [Name("CPU Name")]
        public string? CpuName { get; set; }

        [JsonPropertyName("gaming_device_type")]
        [Name("Gaming Device Type")]
        public int? GamingDeviceType { get; set; }

        [JsonPropertyName("dx_driver_version")]
        [Name("DX Driver Version")]
        public string? DxDriverVersion { get; set; }

        [JsonPropertyName("adapter_description")]
        [Name("Adapter Description")]
        public string? AdapterDescription { get; set; }

        [JsonPropertyName("driver_version")]
        [Name("Driver Version")]
        public string? DriverVersion { get; set; }

        [JsonPropertyName("driver_date")]
        [Ignore]
        public string? DriverDateRaw { get; set; } = "";
        [Name("Driver Date")] // Unsure what this is exactly, and why it sometimes returns -1--1--1
        public DateOnly? DriverDate =>
        string.IsNullOrWhiteSpace(DriverDateRaw) || DriverDateRaw == "-1--1--1"
        ? null
        : DateOnly.Parse(DriverDateRaw);

        [JsonPropertyName("vram_size")]
        [Name("VRAM Size")]
        public int? VramSize { get; set; }

        [JsonPropertyName("screen_width")]
        [Name("Screen Width")]
        public int? ScreenWidth { get; set; }

        [JsonPropertyName("screen_height")]
        [Name("Screen Height")]
        public int? ScreenHeight { get; set; }

        [JsonPropertyName("precise_frame_rate")]
        [Name("Precise Frame Rate")]
        public int? PreciseFrameRate { get; set; }
    }

    internal class Author
    {
        public int? ReviewID { get; set; }

        [JsonPropertyName("steamid")]
        [Name("Steam ID")]
        public string? SteamId { get; set; }

        [JsonPropertyName("personaname")]
        [Name("Persona Name")]
        public string? PersonaName { get; set; }

        [JsonPropertyName("persona_status")]
        [Name("Persona Status")]
        public string? PersonaStatus { get; set; }

        [JsonPropertyName("profile_url")]
        [Name("Profile URL")]
        public string? ProfileUrl { get; set; }

        [JsonPropertyName("num_games_owned")]
        [Name("Number of Games Owned")]
        public int? NumGamesOwned { get; set; }

        [JsonPropertyName("num_reviews")]
        [Name("Number of Reviews")]
        public int? NumReviews { get; set; }

        [JsonPropertyName("playtime_forever")]
        [Name("Playtime Forever")]
        public int? PlaytimeForever { get; set; }

        [JsonPropertyName("playtime_last_two_weeks")]
        [Name("Playtime Last Two Weeks")]
        public int? PlaytimeLastTwoWeeks { get; set; }

        [JsonPropertyName("playtime_at_review")]
        [Ignore]
        public int? playtime_at_review { get; set; }
        [Name("Playtime at Review")]
        public int? PlaytimeAtReview { get { return playtime_at_review; } set { playtime_at_review = value; } }

        [JsonPropertyName("last_played")]
        [Ignore]
        public long LastPlayed { get; set; }
        [Name("Last Played")]
        public DateTime LastPlayedDateTime =>
        DateTimeOffset.FromUnixTimeSeconds(LastPlayed).LocalDateTime;

        [JsonPropertyName("avatar")]
        [Ignore]
        public string? AvatarRaw { get; set; }
        [Name("Avatar URL")]
        public string? Avatar { get { return AvatarRaw; } set { AvatarRaw = $"https://avatars.akamai.steamstatic.com/{value}.jpg"; } }

        [Name("Full Avatar Url")]
        public string? FullAvatar =>
        $"https://avatars.akamai.steamstatic.com/{Avatar}_full.jpg";
    }

    internal class Reaction
    {
        private static readonly Dictionary<int, string> ReactionTypes = new()
        {
            { 13, "Clever" },
            { 14, "Warm Blanket" },
            { 15, "Saucy" },
            { 16, "Slow Clap" },
            { 17, "Take My Points" },
            { 18, "Wholesome" },
            { 19, "Jester" },
            { 20, "Fancy Pants" },
            { 21, "Whoa" },
            { 22, "Super Star" },
            { 23, "Wild" },
            { 24, "Winner"},
            { 25, "Beautiful" },
            { 26, "Helpful" },
            { 27, "Fire" },
            { 28, "Funny" },
            { 29, "One Hundred"},
            { 30, "Life Saver"},
            { 31, "Perfect" },
            { 32, "Plus One" },
            { 33, "Smart" },
            { 34, "Pure Gold" },
            { 35, "Wholesome" }
        };

        [JsonPropertyName("reaction_type")]
        [Ignore]
        public int? ReactionTypeRaw { get; set; }
        [Name("Reaction Type")]
        public string? ReactionType
        {
            get { return ReactionTypeRaw != null && ReactionTypes.ContainsKey(ReactionTypeRaw.Value) ? ReactionTypes[ReactionTypeRaw.Value] : null; }
            set { ReactionTypeRaw = ReactionTypes.FirstOrDefault(x => x.Value == value).Key; }
        }

        [JsonPropertyName("count")]
        [Name("Reaction Count")]
        public int? Count { get; set; }
    }

    internal class StatusErrorException : Exception { }

}