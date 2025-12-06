using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallsDetector : MonoBehaviour
{
    public UnityEvent<GatePair> OnMissed;
    public UnityEvent<GatePair> OnNotMissed;
    public ObstaclesController obstaclesController;
    public void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //p//rint("FallsDetector collide with: "+collision.gameObject.name);
        /*
        collision.transform.parent.TryGetComponent<GatePair>(out GatePair gp);
        if (gp != null)
        {
            if (gp.data.GetIsMissed())
                OnMissed?.Invoke(gp);
            else OnNotMissed?.Invoke(gp);
        }*/
    }
}
