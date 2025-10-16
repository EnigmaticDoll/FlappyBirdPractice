using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject pooledPrefab;
    [SerializeField] private uint defaultPersistentCapacity;
    [SerializeField] private uint maxPersistentCapacity;

    private GameObject _PooledInstanceInactiveParant;
    private Queue<GameObject> _Pool;

    private void OnEnable()
    {
        _PooledInstanceInactiveParant = new GameObject();
        _PooledInstanceInactiveParant.transform.SetParent(this.transform);
        _PooledInstanceInactiveParant.SetActive(false);
        _Pool = new Queue<GameObject>();
        pooledPrefab.SetActive(false);

        for (int i = 0; i < defaultPersistentCapacity; i++)
        {
            _Pool.Enqueue(CreateGameObject());
        }
    }

    public GameObject GetObject()
    {
        GameObject obj = null;

        //return pull
        while (_Pool.Count > 0)
        {
            obj = _Pool.Dequeue();
            if (obj != null) break; 
        }

        // create pull
        if (obj == null)
        {
            obj = CreateGameObject();
        }

        obj.transform.SetParent(null);
        obj.SetActive(true);
        return obj;
    }

    public GameObject GetObject(Transform parent)
    {
        GameObject obj = null;

        while (_Pool.Count > 0)
        {
            obj = _Pool.Dequeue();
            if (obj != null) break;
        }
        if (obj == null)
        {
            obj = CreateGameObject();
        }

        obj.transform.SetParent(parent);
        obj.SetActive(true);
        return obj;
    }

    public GameObject GetObject(GameObject parent)
    {
        GameObject obj = null;

        while (_Pool.Count > 0)
        {
            obj = _Pool.Dequeue();
            if (obj != null) break;
        }

        if (obj == null)
        {
            obj = CreateGameObject();
        }

        obj.transform.SetParent(parent.transform);
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null) return;  

        if (_Pool.Count >= maxPersistentCapacity)
            Destroy(obj);
        else
        {
            obj.SetActive(false);
            obj.transform.SetParent(_PooledInstanceInactiveParant.transform);
            _Pool.Enqueue(obj);
        }
    }

    private GameObject CreateGameObject()
    {
        GameObject obj = Instantiate(pooledPrefab, _PooledInstanceInactiveParant.transform);
        return obj;
    }
}