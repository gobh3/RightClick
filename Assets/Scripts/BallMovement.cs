using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallMovement : MonoBehaviour
{
    public UnityEvent OnSuccess;
    public UnityEvent OnSuccessOnRight;
    public UnityEvent OnSuccessOnLeft;
    public UnityEvent OnFailure;
    public UnityEvent OnFailureOnRight;
    public UnityEvent OnFailureOnLeft;
    public UnityEvent OnMoveRight;
    public UnityEvent OnMoveLeftt;

    public GatePairData data;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootRight()
    {
        if (data.IsInversed)
            shootLeft();
        else
            shootRight();
    }

    private void shootRight()
    {
        rb.linearVelocity += Vector2.right * 5f;
        OnMoveRight?.Invoke();
    }

    public void ShootLeft()
    {
        if (data.IsInversed)
            shootRight();
        else
            shootLeft();
    }

    public void shootLeft()
    {
        rb.linearVelocity += Vector2.left * 5f;
        OnMoveLeftt?.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string gateName = collision.gameObject.name;
        data.SetIsMissed(false);
        if (keySideToName(data.Key) == gateName)
        {
            OnSuccess?.Invoke();
            
            if (keySideToName(KeySide.Right) == gateName)
            { 
                OnSuccessOnRight?.Invoke(); 
            }
            else 
            {
                OnSuccessOnLeft?.Invoke(); 
            }
        }

        else if (keySideToName(data.Other) == gateName)
        {
            OnFailure?.Invoke();
            if (keySideToName(KeySide.Right) == gateName)
            {
                OnFailureOnRight?.Invoke();
            }
            else
            {
                OnFailureOnLeft?.Invoke();
            }
        }
        //else OnMissed?.Invoke(this);
    }

    private string keySideToName(KeySide key)
    {
        switch (key)
        {
            case KeySide.Left:
                return "LeftGate";
            case KeySide.Right:
                return "RightGate";
        }
        return "BH";
    }
}


