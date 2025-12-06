using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtil
{
    private const float RoundProgressFraction = 0.01f;
    public static float? Lerp(float? from, float? to, float by)
    {
        float? result = (to - from) * by + from;
        if (by + RoundProgressFraction >= 1f)
            return to;
        return result;
    }

    public static bool GetRandomBoolean()
    {
        return UnityEngine.Random.value > 0.5f;
    }

    public static List<int> GetUniqueRandomNumbers(int from, int to, int count)
    {
        List<int> numbers = new List<int>();
        List<int> result = new List<int>();

        // Add all numbers from 'from' to 'to' into the 'numbers' list
        for (int i = from; i <= to; i++)
        {
            numbers.Add(i);
        }

        // Check if there are enough numbers in the range
        if (count > numbers.Count)
        {
            Debug.LogError("Count exceeds the number of available unique values in the range!");
            return result;
        }

        // Generate unique random numbers
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, numbers.Count); // Get a random index
            result.Add(numbers[randomIndex]); // Add the selected number to the result list
            numbers.RemoveAt(randomIndex); // Remove it from the list to avoid duplicates
        }

        return result;
    }

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T)); // Get all enum values
        int randomIndex = UnityEngine.Random.Range(0, values.Length); // Pick a random index
        return (T)values.GetValue(randomIndex); // Return the enum value at the random index
    }

}
