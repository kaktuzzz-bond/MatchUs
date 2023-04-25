using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class ObjectPool
{
    private readonly Transform _prefab;
    
    private readonly Queue<Transform> _queue;

    public ObjectPool(Transform prefab)
    {
        _prefab = prefab;
        _queue = new Queue<Transform>();
    }

    public Transform Get()
    {
        if (_queue.Count == 0)
        {
            Add();
        }

        return _queue.Dequeue();
    }

    private void Add(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Transform obj = Object.Instantiate(_prefab);

            obj.gameObject.SetActive(false);

            _queue.Enqueue(obj);
        }
    }

    public void Release(Transform obj)
    {
        obj.gameObject.SetActive(false);

        _queue.Enqueue(obj);
    }


    public void ReleaseAll()
    {
        foreach (Transform t in _queue)
        {
            Release(t);
        }
    }
}