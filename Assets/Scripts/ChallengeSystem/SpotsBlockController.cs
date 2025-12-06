using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotsBlockController : MonoBehaviour
{
    private Spot[] spotsList;

    public void Awake()
    {
        spotsList = GetComponentsInChildren<Spot>();
        if (spotsList == null)
        {
            spotsList = new Spot[0]; // avoid null ref later
        }
    }

    public void TurnOnSpot(int spotIndex)
    {
        if (spotsList == null || spotIndex < 0 || spotIndex >= spotsList.Length)
        {
            Debug.LogWarning($"Can't turn on spot {spotIndex}. Spots count: {spotsList?.Length ?? 0}.");
            return;
        }

        if (spotsList[spotIndex] != null)
            spotsList[spotIndex].TurnOn();
        else
            Debug.LogWarning($"Spot at index {spotIndex} is null.");
    }

    public void TurnOffSpot(int spotIndex)
    {
        if (spotsList == null || spotIndex < 0 || spotIndex >= spotsList.Length)
        {
            Debug.LogWarning($"Can't turn off spot {spotIndex}. Spots count: {spotsList?.Length ?? 0}.");
            return;
        }

        if (spotsList[spotIndex] != null)
            spotsList[spotIndex].TurnOff();
        else
            Debug.LogWarning($"Spot at index {spotIndex} is null.");
    }

    public void TurnOffAll()
    {
        if (spotsList == null)
            return;

        for (int i = 0; i < spotsList.Length; i++)
        {
            if (spotsList[i] != null)
                spotsList[i].TurnOff();
        }
    }

    public int GetSpotsAmount()
    {
        return spotsList?.Length ?? 0;
    }
}
