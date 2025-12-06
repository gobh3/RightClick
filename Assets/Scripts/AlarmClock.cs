using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    private static AlarmClock instance;

    private List<TimeRecord> records = new List<TimeRecord>();

    public void Awake()
    {
        instance = this;
    }

    public static AlarmClock GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        // Use a reverse for loop or copy to avoid modifying collection while iterating
        for (int i = records.Count - 1; i >= 0; i--)
        {
            TimeRecord record = records[i];
            if (record == null || record.client == null)
            {
                records.RemoveAt(i);
                continue;
            }

            if (record.timeElapsed > record.totalTime)
            {
                // record.client.Timeout(); // Uncomment if needed and add null check
                records.RemoveAt(i);
            }
            else
            {
                record.timeElapsed += Time.deltaTime;
                record.client.DuringTimer(record.timeElapsed);
            }
        }
    }

    public void RegisterAndReplace(float duration, ITimerClient client)
    {
        if (client == null)
        {
            Debug.LogWarning("Attempted to register null client.");
            return;
        }
        records.RemoveAll(record => record.client == client);
        records.Add(new TimeRecord(duration, client));
    }

    public void RemoveYourself(ITimerClient client)
    {
        if (client == null) return;
        records.RemoveAll(record => record.client == client);
    }
}
