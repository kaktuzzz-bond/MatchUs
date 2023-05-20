using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class ObjectPool<T> where T: MonoBehaviour
{
    private readonly T _prefab;

    private readonly Queue<T> _queue;


    public ObjectPool(T prefab)
    {
        _prefab = prefab;
        _queue = new Queue<T>();
    }


    public T Get()
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
            T obj = Object.Instantiate(_prefab);

            obj.gameObject.SetActive(false);

            _queue.Enqueue(obj);
        }
    }


    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);

        _queue.Enqueue(obj);
    }


    public void ReleaseAll()
    {
        foreach (T t in _queue)
        {
            Release(t);
        }
    }
}