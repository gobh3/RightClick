using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class ShabbatDetector : MonoBehaviour
{
    [Header("Shabbat Settings")]
    [Tooltip("Hour when Shabbat starts on Friday (24-hour format)")]
    [Range(0, 23)]
    public int shabbatStartHour = 14; // Default: 2 PM

    [Tooltip("Minute when Shabbat starts on Friday")]
    [Range(0, 59)]
    public int shabbatStartMinute = 0;

    [Tooltip("Hour when Shabbat ends on Saturday (24-hour format)")]
    [Range(0, 23)]
    public int shabbatEndHour = 22; // Default: 10 PM

    [Tooltip("Minute when Shabbat ends on Saturday")]
    [Range(0, 59)]
    public int shabbatEndMinute = 0;

    [Header("Debugging")]
    [Tooltip("Enable to make the detector to detect Shabat regardless of the actual time")]
    public bool debugForceShabbat = false;

    [Header("Events")]
    public UnityEvent onShabbatStart;
    public UnityEvent onShabbatEnd;

    private bool _isShabbat = false;
    private const float CHECK_INTERVAL_SECONDS = 60f; // Check every minute

    public bool IsShabbat => _isShabbat;

    private void Start()
    {
        // Initial check
        _isShabbat = IsCurrentlyShabbat();

        // Start periodic checking
        StartCoroutine(CheckShabbatStatusRoutine());
    }

    private IEnumerator CheckShabbatStatusRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CHECK_INTERVAL_SECONDS);

            bool currentlyShabbat = IsCurrentlyShabbat();

            // If status changed from non-Shabbat to Shabbat
            if (!_isShabbat && currentlyShabbat)
            {
                _isShabbat = true;
                onShabbatStart?.Invoke();
                Debug.Log("Shabbat has started");
            }
            // If status changed from Shabbat to non-Shabbat
            else if (_isShabbat && !currentlyShabbat)
            {
                _isShabbat = false;
                onShabbatEnd?.Invoke();
                Debug.Log("Shabbat has ended");
            }
        }
    }

    public bool IsCurrentlyShabbat()
    {
        DateTime now = DateTime.Now;
        DayOfWeek today = now.DayOfWeek;

        // If debug mode is enabled, force Shabbat status
        if (debugForceShabbat)
        {
            return true;
        }

        // Check if it's Friday after start time
        if (today == DayOfWeek.Friday)
        {
            return now.Hour > shabbatStartHour ||
                  (now.Hour == shabbatStartHour && now.Minute >= shabbatStartMinute);
        }
        // Check if it's Saturday before end time
        else if (today == DayOfWeek.Saturday)
        {
            return now.Hour < shabbatEndHour ||
                  (now.Hour == shabbatEndHour && now.Minute < shabbatEndMinute);
        }

        // Not Friday or Saturday
        return false;
    }

    // Optional: Public methods to manually check next Shabbat times
    public DateTime GetNextShabbatStart()
    {
        DateTime now = DateTime.Now;
        DayOfWeek today = now.DayOfWeek;

        int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today + 7) % 7;

        // If today is Friday but already past start time, get next Friday
        if (today == DayOfWeek.Friday &&
            (now.Hour > shabbatStartHour || (now.Hour == shabbatStartHour && now.Minute >= shabbatStartMinute)))
        {
            daysUntilFriday = 7;
        }

        return now.Date.AddDays(daysUntilFriday)
            .AddHours(shabbatStartHour).AddMinutes(shabbatStartMinute);
    }

    public DateTime GetNextShabbatEnd()
    {
        DateTime now = DateTime.Now;
        DayOfWeek today = now.DayOfWeek;

        int daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)today + 7) % 7;

        // If today is Saturday but already past end time, get next Saturday
        if (today == DayOfWeek.Saturday &&
            (now.Hour > shabbatEndHour || (now.Hour == shabbatEndHour && now.Minute >= shabbatEndMinute)))
        {
            daysUntilSaturday = 7;
        }

        return now.Date.AddDays(daysUntilSaturday)
            .AddHours(shabbatEndHour).AddMinutes(shabbatEndMinute);
    }
}