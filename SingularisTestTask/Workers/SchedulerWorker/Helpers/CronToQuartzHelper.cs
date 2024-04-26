namespace SingularisTestTask.Workers.SchedulerWorker.Helpers;

public static class CronToQuartzHelper
{
    /// <summary>
    /// Converts unix-like cron expression to Quartz-like
    /// </summary>
    /// <param name="crontab"></param>
    /// <returns></returns>
    public static string GetQuartz(string crontab)
    {
        // Check for cron magic entries
        var data = ParseCronMagics(crontab) ?? ParseCronSyntax(crontab.Split(' '));

        return string.Join(" ", data);
    }

    private static string[] ParseCronSyntax(string[] crontabEntry)
    {
        var quartzEntry = new string[7];
        quartzEntry[0] = "0"; // Seconds
        var toggleQuartzCompat = false; // Toggle for Quartz compatibility

        if (crontabEntry.Length == 6)
        {
            crontabEntry = crontabEntry.Skip(1).ToArray();
        }

        for (int index = 0; index < crontabEntry.Length; index++)
        {
            if (index == 0 || index == 1)
            {
                quartzEntry[index + 1] = crontabEntry[index]; // Minutes and Hours
            }
            else if (index == 2)
            {
                if (crontabEntry[index] != "*")
                {
                    toggleQuartzCompat = true;
                }

                quartzEntry[index + 1] = crontabEntry[index] == "*" ? "?" : crontabEntry[index]; // Day of Month
            }
            else if (index == 3)
            {
                quartzEntry[index + 1] = crontabEntry[index]; // Month
            }
            else if (index == 4)
            {
                var itemAbbreviated = AdvanceNumber(crontabEntry[index]); // Day of Week
                quartzEntry[index + 1] = toggleQuartzCompat ? "?" : itemAbbreviated;
            }
            else
            {
                break; // Beyond index 4, we don't care and exit the loop
            }
        }

        quartzEntry[6] = "*"; // Year (Quartz expects a last 7th parameter for scheduling yearly recurrence)

        return quartzEntry;
    }

    private static string[]? ParseCronMagics(string crontab)
    {
        string[]? quartzEntry = null;

        if (crontab.StartsWith("@hourly"))
        {
            quartzEntry = new[] { "0", "0", "*", "*", "*", "?", "*" };
        }
        else if (crontab.StartsWith("@daily") || crontab.StartsWith("@midnight"))
        {
            quartzEntry = new[] { "0", "0", "0", "*", "*", "?", "*" };
        }
        else if (crontab.StartsWith("@weekly"))
        {
            quartzEntry = new[] { "0", "0", "0", "?", "*", "1", "*" };
        }
        else if (crontab.StartsWith("@monthly"))
        {
            quartzEntry = new[] { "0", "0", "0", "1", "*", "?", "*" };
        }
        else if (crontab.StartsWith("@yearly") || crontab.StartsWith("@annually"))
        {
            quartzEntry = new[] { "0", "0", "0", "1", "1", "?", "*" };
        }

        return quartzEntry;
    }
    
    private static string AdvanceNumber(string str)
    {
        var quartzCompatibleStr = "";
        foreach (var chr in str)
        {
            if (int.TryParse(chr.ToString(), out int num))
            {
                // Char is an actual number
                if (num is >= 0 and <= 7)
                {
                    quartzCompatibleStr += (num + 1).ToString();
                }
                else
                {
                    // Otherwise default to 1, beginning of the week
                    quartzCompatibleStr = "1";
                }
            }
            else
            {
                quartzCompatibleStr += chr;
            }
        }

        return quartzCompatibleStr;
    }
}