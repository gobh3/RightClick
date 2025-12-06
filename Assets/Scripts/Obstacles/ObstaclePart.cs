using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ObstaclePart : MonoBehaviour
{
    //public UnityEvent OnGoodCollision;
    //public UnityEvent OnBadCollision;
    public SpriteRenderer sp;
    public Animator animator;
    public ParticleSystem boomParticlesEffect;
    public string triggerOnBadCol;
    public bool isGood;
    /// <summary>
    /// states whether a ball already collided with this part.
    /// </summary>
    public bool isLive = true;

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent<ABall>(out ABall ball);
        if (ball != null && ball.isLive == true)
        {
            Kill();
        }
    }
    */
    public void KillOnGame()
    {
        //if (isLive)
        //{
        if (isGood)
        {
            boomParticlesEffect.Play();
            sp.enabled = false;
        }
        else
        {
            animator.SetTrigger(triggerOnBadCol);
        }
        //}
    }

    public void SetIsLive(bool l)
    {
        isLive = l;
    }
}
