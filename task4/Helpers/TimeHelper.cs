namespace task4.Helpers;

public static class TimeHelper
{
    public static string ToRelative(DateTime date)
    {
        if (date == default)
            return "-";

        var span = DateTime.Now - date;

        if (span.TotalSeconds < 60)
            return "just now";

        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes} minutes ago";

        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours} hours ago";

        if (span.TotalDays < 7)
            return $"{(int)span.TotalDays} days ago";

        return date.ToString("dd.MM.yyyy HH:mm");
    }
}