using Microsoft.VisualBasic;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;

public class Csv
{
    public static void SaveCsv (List<Review> reviews, string filename, bool useRecommendedOutput, CustomOutput customOutput) {
        var MainRecords = new List<Review>(reviews);
        var HardwareRecords = new List<Hardware>();
        foreach (var review in reviews) {
            if (review.Hardware != null) {
                HardwareRecords.Add(review.Hardware);
            }
        }
        var AuthorRecords = new List<Author>();
        foreach (var review in reviews) {
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
        if (customOutput.Fields.Count > 0) {
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
        if (useRecommendedOutput) {
            using (var writer = new StreamWriter($"{filename}_reviews_RecommendedOutput.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            using (var reader = new StreamReader($"{filename}_reviews_MainRecords.csv"))
            using (var fullFile = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
            var records = fullFile.GetRecords<ReviewCsv>();
            CustomOutput RecommendedOutput = CustomOutput.Parse("ID,PersonaName,Language,TimestampCreated,PlaytimeAtReview,PlaytimeForever,ReviewText,Vote,VotesUp,VotesFunny,WeightedVoteScore,SteamPurchase,ReceivedForFree,WrittenDuringEarlyAccess");
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

public class CustomOutput
{
    public List<string> Fields { get; set; }
    public CustomOutput(List<string> fields) { this.Fields = fields; }

    public static CustomOutput Parse(string input) {
        var fields = input.Split(',').ToList();
        return new CustomOutput(fields);
    }
}
public class ReviewCsv
{
    // Review
    public int ID { get; set; }
    public string? RecommendationId { get; set; }
    public string? RecommendationUrl { get; set; }

    // Author
    public int ReviewID { get; set; }
    public string? SteamId { get; set; }
    public string? PersonaName { get; set; }
    public string? PersonaStatus { get; set; }
    public string? ProfileUrl { get; set; }
    public int NumGamesOwned { get; set; }
    public int NumReviews { get; set; }
    public int PlaytimeForever { get; set; }
    public int PlaytimeLastTwoWeeks { get; set; }
    public int PlaytimeAtReview { get; set; }
    public long LastPlayed { get; set; }
    public DateTime LastPlayedDateTime { get; set; }
    public string? Avatar { get; set; }
    public string? FullAvatar { get; set; }

    // Review content
    public string? Language { get; set; }
    public string? ReviewText { get; set; }

    public long TimestampCreated { get; set; }
    public DateTime DateCreated { get; set; }

    public long TimestampUpdated { get; set; }
    public DateTime DateUpdated { get; set; }

    public bool VotedUp { get; set; }
    public string? Vote { get; set; }

    public int VotesUp { get; set; }
    public int VotesFunny { get; set; }

    public double WeightedVoteScore { get; set; }

    public int CommentCount { get; set; }

    public bool SteamPurchase { get; set; }
    public bool ReceivedForFree { get; set; }
    public bool Refunded { get; set; }
    public bool WrittenDuringEarlyAccess { get; set; }
    public bool PrimarilySteamDeck { get; set; }

    public string? AppReleaseDate { get; set; }
    public DateTime AppReleaseDateTime { get; set; }

    // Hardware
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? DxVideoCard { get; set; }
    public int? DxVendorId { get; set; }
    public int? DxDeviceId { get; set; }
    public int? NumGpu { get; set; }
    public string? SystemRam { get; set; }
    public string? Os { get; set; }
    public string? CpuVendor { get; set; }
    public string? CpuName { get; set; }
    public int? GamingDeviceType { get; set; }
    public string? DxDriverVersion { get; set; }
    public string? AdapterDescription { get; set; }
    public string? DriverVersion { get; set; }
    public string? DriverDateRaw { get; set; }
    public DateOnly? DriverDate { get; set; }
    public int? VramSize { get; set; }
    public int? ScreenWidth { get; set; }
    public int? ScreenHeight { get; set; }
    public int? PreciseFrameRate { get; set; }
}
