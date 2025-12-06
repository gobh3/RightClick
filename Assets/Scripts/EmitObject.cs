using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmitObject : MonoBehaviour
{
    public GameObject Object;
    public Transform ParentOtherThanGlobal;
    public SortingLayer Layer;
    public float DestroyAfter = 10f;

    // Start is called before the first frame update
    public void InstaniateObject()
    {
        GameObject go = Instantiate(Object, gameObject.transform.position, Quaternion.identity);
        setParent(go);
        setDestroyAfter(go);
    }

    public void InstantinateObjectOnPos(Vector3 pos)
    {
        GameObject go = Instantiate(Object);
        setParent(go);
        setPosition(go, pos, Quaternion.identity);
        setDestroyAfter(go);

    }
    public void InstantinateObjectOnPos(Vector2 pos)
    {
        Vector3 v = new Vector3(pos.x, pos.y, -1);
        GameObject go = Instantiate(Object);
        setParent(go);
        setPosition(go, v, Quaternion.identity);
        setDestroyAfter(go);
    }

    public void InstantinateObjectWithZRot(float angle)
    {
        Quaternion rot = Quaternion.identity;
        rot.z = angle;
        GameObject go = Instantiate(Object);
        setParent(go);
        setPosition(go, transform.position, rot);
        setDestroyAfter(go);
    }

    private void setParent(GameObject go)
    {
        if (ParentOtherThanGlobal != null)
        {
            go.gameObject.transform.SetParent(ParentOtherThanGlobal);
            go.transform.localScale = ParentOtherThanGlobal.localScale;
        }
        setDestroyAfter(go);
    }

    private void setDestroyAfter(GameObject go)
    {
        if (DestroyAfter > 0)
            Destroy(go, DestroyAfter);
    }

    private void setPosition(GameObject go, Vector3 pos, Quaternion rot)
    {
        go.transform.position = pos;
        go.transform.rotation = rot;
    }


}
