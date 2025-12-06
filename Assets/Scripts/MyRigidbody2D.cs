using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidbody2D : MonoBehaviour
{
    private Vector2 velocity;
    private float prevFactor;
    private float factor;
    private SpeedController speedController;

    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            Vector2 prevV = velocity;
            velocity = value;
            setActualVelocity(prevV);
        }
    }

    private void Awake()
    {
        speedController = SpeedController.GetInstance();
        speedController.RegisterToOnSpeedChanged(updateFactor);
        factor = speedController.GetCurrentSpeed().Value;
        prevFactor = factor;
    }

    private void updateFactor(float? f)
    {
        prevFactor = factor;
        factor = f.Value;
        foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
        {
            Vector2 baseV = rb.linearVelocity / prevFactor;
            rb.linearVelocity = baseV * factor;
        }
    }

    void setActualVelocity(Vector2 prevV)
    {
        foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
        {
            Vector2 baseV = rb.linearVelocity / prevFactor;
            baseV -= prevV;
            baseV += velocity;
            rb.linearVelocity = baseV * factor;
        }
    }
}
