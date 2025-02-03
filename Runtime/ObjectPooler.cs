using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Utilities;

public class ObjectPooler : GenericSingletonClass<ObjectPooler>
{
    [SerializeField] private PoolKey[] basePoolKeys;
    private Dictionary<string, Pool> pools;

    [Serializable]
    public struct Pool
    {
        public GameObject prefab;
        public Queue<GameObject> queue;
        public int baseCount;
    }

    [Serializable]
    public struct PoolKey
    {
        public Pool pool;
        public string poolName;
    }


    #region Init Functions

    public override void Awake()
    {
        base.Awake();

        InitDictionary();
        PopulatePools();
    }

    private void InitDictionary()
    {
        foreach (PoolKey currentPoolKey in basePoolKeys)
        {
            pools.Add(currentPoolKey.poolName, currentPoolKey.pool);
        }
    }

    private void PopulatePools()
    {
        foreach (PoolKey currentPoolKey in basePoolKeys)
        {
            for (int i = 0; i < currentPoolKey.pool.baseCount; i++)
            {
                AddInstance(currentPoolKey.pool);
            }
        }
    }

    private void AddInstance(Pool pool)
    {
        GameObject obj = Instantiate(pool.prefab, transform);
        obj.SetActive(false);

        pool.queue.Enqueue(obj);
    }

    #endregion



    public GameObject UnpoolObject(string poolName, float autoPoolDuration = 0)
    {
        if (pools[poolName].queue.Count == 0)
        {
            AddInstance(pools[poolName]);
        }

        GameObject obj = pools[poolName].queue.Dequeue();
        obj.SetActive(true);

        if(autoPoolDuration > 0)
        {
            StartCoroutine(AutoPoolCoroutine(poolName, obj, autoPoolDuration));
        }

        return obj;
    }

    public void PoolObject(string poolName, GameObject go)
    {
        pools[poolName].queue.Enqueue(go);

        go.transform.parent = transform;
        go.SetActive(false);
    }

    private IEnumerator AutoPoolCoroutine(string poolName, GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);

        PoolObject(poolName, go);
    }
}
