using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePairStyle : MonoBehaviour
{
    public GatePairData data;
    public SpriteRenderer leftGate;
    public SpriteRenderer rightGate;
    public SpriteRenderer ball;
    public SpriteRenderer blurBall;
    public Sprite inversedBallSprite;
    public Sprite inversedBallBlurSprite;
    public Sprite regBallSprite;
    public Sprite regBallBlurSprite;

    [Range(0, 100)]
    public int OtherGateA;

    public Material ShinyTrnsparentMaterial;
    public Material ShinyMaterial;
    public void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        float a = (float)OtherGateA / 100;
        if (data.Key == KeySide.Left)
        {
            rightGate.material = ShinyTrnsparentMaterial;
            leftGate.material = ShinyMaterial;
        }
        else
        {
            leftGate.material = ShinyTrnsparentMaterial;
            rightGate.material = ShinyMaterial;
        }

        if (data.IsInversed)
        {
            ball.sprite = inversedBallSprite;
            blurBall.sprite = inversedBallBlurSprite;
        }
        else
        {
            ball.sprite = regBallSprite;
            blurBall.sprite = regBallBlurSprite;
        }
    }

   
}