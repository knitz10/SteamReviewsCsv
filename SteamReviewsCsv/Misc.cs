class Misc
{
    public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        return DateTimeOffset
        .FromUnixTimeSeconds(unixTimeStamp)
        .LocalDateTime;
    }

    public static string TerminalURL(string caption, string url) => $"\u001B]8;;{url}\a{caption}\u001B]8;;\a"; // thanks to PhonicUK on StackOverflow for this one https://stackoverflow.com/a/71833021

    public static int[] RemoveAvailableArg(int[] AvailableArguments, int numToRemove) // thanks meetjaydeep! https://stackoverflow.com/a/497005
    {
        return AvailableArguments = [.. AvailableArguments.Where(val => val != numToRemove)];
    }
}
