using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    public GameObject echo;
    public float timeBetweenSpawns;
    public float destroySpawnAfter;
    public GameObject fatherOtherThanThisObject;
    public Vector2 scale;
    private bool ok = true;
    public bool SartOnAwake;
    public void Awake()
    {
        if (SartOnAwake)
        {
            StartEmitting();
        }
    }
    public void StartEmitting()
    {
        StartCoroutine(startEmitting());
    }

    private IEnumerator startEmitting() {
        ok = true;
        GameObject go; 
        while (ok)
        {
            if(fatherOtherThanThisObject != null) 
               go = Instantiate(echo, transform.position, Quaternion.identity, fatherOtherThanThisObject.transform);
            else go = Instantiate(echo, transform.position, Quaternion.identity, gameObject.transform);
            go.transform.localScale = scale;
            Destroy(go, destroySpawnAfter);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void StopEmitting()
    {
        ok = false;
    }

}
