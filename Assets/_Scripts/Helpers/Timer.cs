using UnityEngine;

public static class Timer
{
    public static string FormatTime(float timeCounter)
    {
        int days = Mathf.FloorToInt(timeCounter / (24f * 60f * 60f));

        float daysRemained = Mathf.Floor(timeCounter % (24f * 60f * 60f));

        int hours = Mathf.FloorToInt(daysRemained / (60f * 60f));

        float hoursRemained = Mathf.Floor(daysRemained % (60f * 60f));

        int minutes = Mathf.FloorToInt(hoursRemained / 60);

        int seconds = Mathf.FloorToInt(timeCounter % 60);

        if (days != 0)
            return $"{days:D1}d {hours:D2}:{minutes:D2}:{seconds:D2}";

        return timeCounter < 60 * 60
                ? $"{minutes:D2}:{seconds:D2}"
                : $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}