using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstaclesPool : MonoBehaviour
{
    public GatePair PrefGatePair;
    public enum PoolType
    {
        Stack,
        LinkedList
    }

    public PoolType poolType;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize;
    public int initializeWith;
    IObjectPool<GatePair> m_Pool;

    public IObjectPool<GatePair> Pool
    {
        get
        {
            return m_Pool;
        }
    }

    public void Awake()
    {
        if (m_Pool == null)
        {
            if (poolType == PoolType.Stack)
                m_Pool = new ObjectPool<GatePair>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            else
                m_Pool = new LinkedPool<GatePair>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
        }
        GatePair[] l = new GatePair[initializeWith];
        for (int i = 0; i < initializeWith; i++)
        {
            l[i] = m_Pool.Get();
        }
        for (int i = 0; i < initializeWith; i++)
        {
            m_Pool.Release(l[i]);
        }
    }

    GatePair CreatePooledItem()
    {
        GatePair gp = Instantiate(PrefGatePair);
        /*
        // This is used to return GatePair to the pool when they have stopped.
        var returnToPool = go.AddComponent<ReturnToPool>();
        returnToPool.pool = Pool;
        returnToPool.gp = gp;*/

        return gp;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GatePair system)
    {
        system.gameObject.SetActive(false);
        foreach (Transform t in system.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GatePair system)
    {
        system.gameObject.SetActive(true);
        foreach (Transform t in system.transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GatePair system)
    {
        Destroy(system.gameObject);
    }

}
