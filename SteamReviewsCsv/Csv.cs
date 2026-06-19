using Microsoft.VisualBasic;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
namespace SteamReviewsCsv
{
    internal class Csv
    {
        public static void SaveCsv(List<Review> reviews, string filename, bool useRecommendedOutput, CustomOutput customOutput)
        {
            foreach (var review in reviews)
            {
                {
                    int Clever = 0;
                    int WarmBlanket = 0;
                    int Saucy = 0;
                    int SlowClap = 0;
                    int TakeMyPoints = 0;
                    int Wholesome = 0;
                    int Jester = 0;
                    int FancyPants = 0;
                    int Whoa = 0;
                    int SuperStar = 0;
                    int Wild = 0;
                    int Winner = 0;
                    int Beautiful = 0;
                    int Helpful = 0;
                    int Fire = 0;
                    int Funny = 0;
                    int OneHundred = 0;
                    int LifeSaver = 0;
                    int Perfect = 0;
                    int PlusOne = 0;
                    int Smart = 0;
                    int PureGold = 0;
                    if (review.Reactions != null)
                    {
                        foreach (var reaction in review.Reactions)
                        {
                            switch (reaction.ReactionType)
                            {
                                case "Clever":
                                    Clever = reaction.Count ?? 0;
                                    break;
                                case "Warm Blanket":
                                    WarmBlanket = reaction.Count ?? 0;
                                    break;
                                case "Saucy":
                                    Saucy = reaction.Count ?? 0;
                                    break;
                                case "Slow Clap":
                                    SlowClap = reaction.Count ?? 0;
                                    break;
                                case "Take My Points":
                                    TakeMyPoints = reaction.Count ?? 0;
                                    break;
                                case "Wholesome":
                                    Wholesome = reaction.Count ?? 0;
                                    break;
                                case "Jester":
                                    Jester = reaction.Count ?? 0;
                                    break;
                                case "Fancy Pants":
                                    FancyPants = reaction.Count ?? 0;
                                    break;
                                case "Whoa":
                                    Whoa = reaction.Count ?? 0;
                                    break;
                                case "Super Star":
                                    SuperStar = reaction.Count ?? 0;
                                    break;
                                case "Wild":
                                    Wild = reaction.Count ?? 0;
                                    break;
                                case "Winner":
                                    Winner = reaction.Count ?? 0;
                                    break;
                                case "Beautiful":
                                    Beautiful = reaction.Count ?? 0;
                                    break;
                                case "Helpful":
                                    Helpful = reaction.Count ?? 0;
                                    break;
                                case "Fire":
                                    Fire = reaction.Count ?? 0;
                                    break;
                                case "Funny":
                                    Funny = reaction.Count ?? 0;
                                    break;
                                case "One Hundred":
                                    OneHundred = reaction.Count ?? 0;
                                    break;
                                case "Life Saver":
                                    LifeSaver = reaction.Count ?? 0;
                                    break;
                                case "Perfect":
                                    Perfect = reaction.Count ?? 0;
                                    break;
                                case "Plus One":
                                    PlusOne = reaction.Count ?? 0;
                                    break;
                                case "Smart":
                                    Smart = reaction.Count ?? 0;
                                    break;
                                case "Pure Gold":
                                    PureGold = reaction.Count ?? 0;
                                    break;
                            }
                        }
                        review.NamedReactions = new NamedReactions
                        {
                            Clever = Clever,
                            WarmBlanket = WarmBlanket,
                            Saucy = Saucy,
                            SlowClap = SlowClap,
                            TakeMyPoints = TakeMyPoints,
                            Wholesome = Wholesome,
                            Jester = Jester,
                            FancyPants = FancyPants,
                            Whoa = Whoa,
                            SuperStar = SuperStar,
                            Wild = Wild,
                            Winner = Winner,
                            Beautiful = Beautiful,
                            Helpful = Helpful,
                            Fire = Fire,
                            Funny = Funny,
                            OneHundred = OneHundred,
                            LifeSaver = LifeSaver,
                            Perfect = Perfect,
                            PlusOne = PlusOne,
                            Smart = Smart,
                            PureGold = PureGold
                        };
                    }
                }
            }

            var MainRecords = new List<Review>(reviews);
            var HardwareRecords = new List<Hardware>();



            foreach (var review in reviews)
            {
                if (review.Hardware != null)
                {
                    HardwareRecords.Add(review.Hardware);
                }
            }

            var AuthorRecords = new List<Author>();

            foreach (var review in reviews)
            {
                AuthorRecords.Add(review.Author);
            }

            // Main Records
            using (var writer = new StreamWriter($"{filename}_reviews_MainRecords.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(MainRecords);
            }

            // Author Records
            using (var writer = new StreamWriter($"{filename}_reviews_AuthorRecords.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(AuthorRecords);
            }

            // Hardware Records
            using (var writer = new StreamWriter($"{filename}_reviews_HardwareRecords.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(HardwareRecords);
            }

            // Custom Output
            if (customOutput.Fields.Count > 0)
            {
                using (var writer = new StreamWriter($"{filename}_reviews_CustomOutput.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    using (var reader = new StreamReader($"{filename}_reviews_MainRecords.csv"))
                    using (var fullFile = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = fullFile.GetRecords<ReviewCsv>();
                        foreach (var field in customOutput.Fields)
                        {
                            csv.WriteField(field);
                        }
                        csv.NextRecord();
                        foreach (var record in records)
                        {
                            foreach (var field in customOutput.Fields)
                            {
                                csv.WriteField(
                                record.GetType()
                                    .GetProperty(field)?
                                    .GetValue(record)?
                                    .ToString() ?? ""
                            );
                            }

                            csv.NextRecord();
                        }
                    }
                }
            }

            // Recommended Output
            if (useRecommendedOutput)
            {
                using (var writer = new StreamWriter($"{filename}_reviews_RecommendedOutput.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    using (var reader = new StreamReader($"{filename}_reviews_MainRecords.csv"))
                    using (var fullFile = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = fullFile.GetRecords<ReviewCsv>();
                        CustomOutput RecommendedOutput = CustomOutput.Parse("ID,PersonaName,Language,DateCreated,PlaytimeAtReview,PlaytimeForever,ReviewText,Vote,VotesUp,VotesFunny,WeightedVoteScore,SteamPurchase,ReceivedForFree,WrittenDuringEarlyAccess");
                        foreach (var field in RecommendedOutput.Fields)
                        {
                            csv.WriteField(field);
                        }
                        csv.NextRecord();
                        foreach (var record in records)
                        {
                            foreach (var field in RecommendedOutput.Fields)
                            {
                                csv.WriteField(
                                record.GetType()
                                    .GetProperty(field)?
                                    .GetValue(record)?
                                    .ToString() ?? ""
                            );
                            }

                            csv.NextRecord();
                        }
                    }
                }
            }
        }
    }

    internal class CustomOutput
    {
        public List<string> Fields { get; set; }
        public CustomOutput(List<string> fields) { this.Fields = fields; }

        public static CustomOutput Parse(string input)
        {
            var fields = input.Split(',').ToList();
            return new CustomOutput(fields);
        }
    }

    internal class ReviewCsv
    {
        // Review
        [Name("ID")]
        public int ID { get; set; }

        [Name("Recommendation ID")]
        public string? RecommendationId { get; set; }
        [Name("Recommendation URL")]
        public string? RecommendationUrl { get; set; }

        // Author
        [Ignore]
        public int ReviewID { get; set; }
        [Name("Steam ID")]
        public string? SteamId { get; set; }
        [Name("Persona Name")]
        public string? PersonaName { get; set; }
        [Name("Persona Status")]
        public string? PersonaStatus { get; set; }
        [Name("Profile URL")]
        public string? ProfileUrl { get; set; }
        [Name("Number of Games Owned")]
        public int NumGamesOwned { get; set; }
        [Name("Number of Reviews")]
        public int NumReviews { get; set; }
        [Name("Playtime Forever")]
        public int PlaytimeForever { get; set; }
        [Name("Playtime Last Two Weeks")]
        public int PlaytimeLastTwoWeeks { get; set; }
        [Name("Playtime at Review")]
        public int PlaytimeAtReview { get; set; }
        [Ignore]
        public long LastPlayed { get; set; }
        [Name("Last Played")]
        public DateTime LastPlayedDateTime { get; set; }
        [Name("Avatar URL")]
        public string? Avatar { get; set; }
        [Name("Full Avatar Url")]
        public string? FullAvatar { get; set; }

        // Review content
        [Name("Language")]
        public string? Language { get; set; }
        [Name("Review Text")]
        public string? ReviewText { get; set; }

        [Ignore]
        public long TimestampCreated { get; set; }
        [Name("Date Created")]
        public DateTime DateCreated { get; set; }

        [Ignore]
        public long TimestampUpdated { get; set; }
        [Name("Date Updated")]
        public DateTime DateUpdated { get; set; }

        [Ignore]
        public bool VotedUp { get; set; }
        [Name("Vote")]
        public string? Vote { get; set; }

        [Name("Votes Up")]
        public int VotesUp { get; set; }
        [Name("Votes Funny")]
        public int VotesFunny { get; set; }

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

        [Name("Weighted Vote Score")]
        public double WeightedVoteScore { get; set; }

        [Name("Comment Count")]
        public int CommentCount { get; set; }

        [Name("Steam Purchase")]
        public bool SteamPurchase { get; set; }
        [Name("Received for Free")]
        public bool ReceivedForFree { get; set; }
        [Name("Refunded")]
        public bool Refunded { get; set; }
        [Name("Written During Early Access")]
        public bool WrittenDuringEarlyAccess { get; set; }
        [Name("Primarily Steam Deck")]
        public bool PrimarilySteamDeck { get; set; }

        [Ignore]
        public string? AppReleaseDate { get; set; }
        [Name("App Release Date")]
        [Ignore]
        public DateTime? AppReleaseDateTime { get; set; }

        // Hardware
        [Name("Manufacturer")]
        public string? Manufacturer { get; set; }
        [Name("Model")]
        public string? Model { get; set; }
        [Name("DX Video Card")]
        public string? DxVideoCard { get; set; }
        [Name("DX Vendor ID")]
        public int? DxVendorId { get; set; }
        [Name("DX Device ID")]
        public int? DxDeviceId { get; set; }
        [Name("Num GPU")]
        public int? NumGpu { get; set; }
        [Name("System RAM")]
        public string? SystemRam { get; set; }
        [Name("Operating System")]
        public string? Os { get; set; }
        [Name("CPU Vendor")]
        public string? CpuVendor { get; set; }
        [Name("CPU Name")]
        public string? CpuName { get; set; }
        [Name("Gaming Device Type")]
        public int? GamingDeviceType { get; set; }
        [Name("DX Driver Version")]
        public string? DxDriverVersion { get; set; }
        [Name("Adapter Description")]
        public string? AdapterDescription { get; set; }
        [Name("Driver Version")]
        public string? DriverVersion { get; set; }
        [Ignore]
        public string? DriverDateRaw { get; set; }
        [Name("Driver Date")]
        public DateOnly? DriverDate { get; set; }
        [Name("VRAM Size")]
        public int? VramSize { get; set; }
        [Name("Screen Width")]
        public int? ScreenWidth { get; set; }
        [Name("Screen Height")]
        public int? ScreenHeight { get; set; }
        [Name("Precise Frame Rate")]
        public int? PreciseFrameRate { get; set; }
    }
}