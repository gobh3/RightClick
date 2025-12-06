using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGarbage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //p//rint("DestroyGarbage:  Destroying GatePair");
        GatePair ob = collision.transform.parent.GetComponent<GatePair>();
        //p//rint("ob.Destroy()");
    }
}
