using UnityEngine;

public class ClassicBall : ABall
{
    public override void ApplyInput(InputValue input)
    {
        switch (input)
        {
            case InputValue.Right:
                SetXVelocity(SideV);
                break;
            case InputValue.Left:
                SetXVelocity(SideV * -1);
                break;
            default:
                break;
        }
    }
}
