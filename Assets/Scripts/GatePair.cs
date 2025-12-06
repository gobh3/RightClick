using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class GatePair : MonoBehaviour
{
    public UnityEvent OnShootRight;
    public UnityEvent OnShootLeft;
    public Animator lAnimator;
    public Animator rAnimator;
    //public IObjectPool<GatePair> pool;
    public GatePairData data;
    public Rigidbody2D ball; 
    public Transform ballBlur;
    public Rigidbody2D rGp;
    public Rigidbody2D lGp;
    public SpriteRenderer rGpSp;
    public SpriteRenderer lGpSp; 
    public GatePairStyle style;
    private Vector2 pos = new Vector2(0,0f);
    private Vector2 rPos = new Vector2(5f, 0f);
    private Vector2 lPos = new Vector2(-5f, 0f);
    
    public void Release(IObjectPool<GatePair> pool)
    {
        lAnimator.SetTrigger("BackToNormal");
        rAnimator.SetTrigger("BackToNormal");
        //in order to prepare gatePair to recycaling
        //we need to reset it.
        //reset key gate:
        data.Initialize();
        //reset gates style accordding to data:
        style.Initialize();
        //reset ball postions:
        SetYVelocity(0);
        ball.transform.localPosition = pos;
        rGp.transform.localPosition = rPos; 
        lGp.transform.localPosition = lPos;
        lGpSp.enabled = true;
        rGpSp.enabled = true;
        // Return to the pool
        pool.Release(this);

    }
    /*public void Destroy()
    {
        foreach (BoxCollider2D c in gameObject.GetComponentsInChildren<BoxCollider2D>())
            c.enabled = false; // Destroying is not immediate so we need to prevent additional
                               // collision
                               //animator.SetTrigger("Die");
                               //StartCoroutine(animate());
        Destroy(gameObject);

    }*/

    //IEnumerator animate()
    //{
    //    // waits for filling animation
    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    //}

    public void AddVelocity(Vector2 velocity)
    {
        setVelocity(ball.linearVelocity + velocity);
        /*foreach (Rigidbody2D rb in transform.GetComponentsInChildren<Rigidbody2D>())
            rb.linearVelocity += velocity;*/
    }
    public void SetYVelocity(float yVelocity)
    {
        setYVelocity(yVelocity);
        /*foreach (Rigidbody2D rb in transform.GetComponentsInChildren<Rigidbody2D>())
        {
            float x = rb.linearVelocity.x;
            Vector2 v = new Vector2(x, yVelocity);
            rb.linearVelocity = v;
            x = x;
        }*/
    }

    private void setVelocity(Vector2 v)
    {
        ball.linearVelocity = v;
        rGp.linearVelocity = v;
        lGp.linearVelocity = v;
    }

    private void setYVelocity(float v)
    {
        ball.linearVelocityY = v;
        rGp.linearVelocityY = v;
        lGp.linearVelocityY = v;
    }

}