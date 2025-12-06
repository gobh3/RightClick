using UnityEngine;
using UnityEngine.Events;
public abstract class ABall : MonoBehaviour
{
    public UnityEvent OnBallHasGoodCollision;
    public UnityEvent OnChangingXVelocity;
    //public UnityEvent OnBallHasBadCollision; 
    static public float SideV = 5f;
    public Rigidbody2D rigidbody2;
    [HideInInspector]
    //public ObstaclesController obstaclesController;
    public SpriteRenderer spriteRenderer;
    public EchoEffect echoEffect;
    public Animator animator;
    public string triggerOnBadCol;
    protected AObstacle father;
    /// <summary>
    /// states wether this ball already colllided with any part.
    /// </summary>
    public bool isLive = true;
    public void SetFather(AObstacle ob)
    {
        father = ob;
        echoEffect.fatherOtherThanThisObject = father.gameObject;
    }

    public abstract void ApplyInput(InputValue input);
    public void SetXVelocity(float velocity)
    {
        rigidbody2.linearVelocityX = velocity;
        OnChangingXVelocity?.Invoke();
    }

    public void SetYVelocity(float velocity)
    {
        rigidbody2.linearVelocityY = velocity;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        bool isGoodCollision;
        collision.TryGetComponent<ObstaclePart>(out ObstaclePart part);
        if (part != null)
        {
            if (isLive)
            {
                part.KillOnGame();
                if (part.isLive)
                {
                    if (part.isGood)
                    {
                        OnBallHasGoodCollision?.Invoke();
                        isGoodCollision = true;
                    }
                    else
                    {
                        isGoodCollision = false;
                    }
                    KillOnGame(isGoodCollision);
                    father.OnBallCollision(part, isGoodCollision);
                    isLive = false;
                }
                part.SetIsLive(false);
            }
        }
    }
    public void KillOnGame(bool isGoodCollision)
    {
        // HERE.
        //if (isLive)
        //{
        if (isGoodCollision)
        {
            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;
        }
        else
        {
            animator.SetTrigger(triggerOnBadCol);
            echoEffect.StopEmitting();
        }
        // }
    }
}
