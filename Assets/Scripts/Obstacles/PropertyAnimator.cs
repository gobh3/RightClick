using UnityEngine;
using System.Collections.Generic;

public class PropertyAnimator : MonoBehaviour
{
    public enum AnimatableProperty
    {
        PositionX, PositionY, PositionZ,
        RotationX, RotationY, RotationZ,
        ScaleX, ScaleY, ScaleZ
    }

    [System.Serializable]
    public class PropertyAnimation
    {
        public AnimatableProperty property;
        public float startValue;
        public float endValue;
        public float duration;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        [HideInInspector] public float progress;
    }

    public List<PropertyAnimation> animations = new List<PropertyAnimation>();

    private Transform cachedTransform;
    private Vector3 cachedPosition;
    private Vector3 cachedRotation;
    private Vector3 cachedScale;

    void Awake()
    {
        cachedTransform = transform;
        cachedPosition = cachedTransform.position;
        cachedRotation = cachedTransform.eulerAngles;
        cachedScale = cachedTransform.localScale;
    }

    void Update()
    {
        bool positionChanged = false;
        bool rotationChanged = false;
        bool scaleChanged = false;

        for (int i = 0; i < animations.Count; i++)
        {
            var animation = animations[i];
            if (animation.progress < 1f)
            {
                animation.progress += Time.deltaTime / animation.duration;
                animation.progress = Mathf.Clamp01(animation.progress);

                float curveValue = animation.curve.Evaluate(animation.progress);
                float currentValue = Mathf.Lerp(animation.startValue, animation.endValue, curveValue);

                switch (animation.property)
                {
                    case AnimatableProperty.PositionX:
                        cachedPosition.x = currentValue;
                        positionChanged = true;
                        break;
                    case AnimatableProperty.PositionY:
                        cachedPosition.y = currentValue;
                        positionChanged = true;
                        break;
                    case AnimatableProperty.PositionZ:
                        cachedPosition.z = currentValue;
                        positionChanged = true;
                        break;
                    case AnimatableProperty.RotationX:
                        cachedRotation.x = currentValue;
                        rotationChanged = true;
                        break;
                    case AnimatableProperty.RotationY:
                        cachedRotation.y = currentValue;
                        rotationChanged = true;
                        break;
                    case AnimatableProperty.RotationZ:
                        cachedRotation.z = currentValue;
                        rotationChanged = true;
                        break;
                    case AnimatableProperty.ScaleX:
                        cachedScale.x = currentValue;
                        scaleChanged = true;
                        break;
                    case AnimatableProperty.ScaleY:
                        cachedScale.y = currentValue;
                        scaleChanged = true;
                        break;
                    case AnimatableProperty.ScaleZ:
                        cachedScale.z = currentValue;
                        scaleChanged = true;
                        break;
                }
            }
        }

        if (positionChanged) cachedTransform.position = cachedPosition;
        if (rotationChanged) cachedTransform.eulerAngles = cachedRotation;
        if (scaleChanged) cachedTransform.localScale = cachedScale;
    }
}