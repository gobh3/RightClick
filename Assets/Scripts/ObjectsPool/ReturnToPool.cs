using UnityEngine;
using UnityEngine.Pool;

public class ReturnToPool : MonoBehaviour
{
    public GatePair gp;
    public IObjectPool<GatePair> pool;
    public GatePairData data;
    public Transform ball;
    public GatePairStyle style;
}
