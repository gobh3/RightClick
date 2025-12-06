using UnityEngine;

public class InversedBall : ABall
{
    public override void ApplyInput(InputValue input)
    {
        switch (input)
        {
            case InputValue.Right:
                SetXVelocity(SideV * -1);
                break;
            case InputValue.Left:
                SetXVelocity(SideV);
                break;
            default:
                break;
        }
    }
}
