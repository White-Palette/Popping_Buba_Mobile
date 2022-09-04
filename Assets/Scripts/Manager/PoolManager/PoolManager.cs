using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PoolManager<T> where T : MonoBehaviour, IPoolable
{
    private static GameObject _prefab = null;
    private static Dictionary<T, bool> _pooledDict = new Dictionary<T, bool>();
    private static Queue<GameObject> _objectQueue = new Queue<GameObject>();
    private static Transform _poolStore = null;

    public static int Count => _objectQueue.Count;
    public static int TotalCount => _pooledDict.Count;

    static PoolManager()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        _prefab = Resources.Load<GameObject>("Prefabs/" + typeof(T).Name);
        Debug.Log($"PoolManager<{typeof(T).Name}> loaded");
    }

    private static void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var poolStore = GameObject.Find("PoolStore");
        if (poolStore != null)
        {
            _poolStore = poolStore.transform;
        }
        else
        {
            GameObject gameObject = new GameObject("PoolStore");
            _poolStore = gameObject.transform;
        }
        _pooledDict.Clear();
        _objectQueue.Clear();
    }

    public static T Get(Transform parent, Vector3 position = new Vector3())
    {
        T pool = null;
        if (_objectQueue.Count > 0)
        {
            pool = _objectQueue.Dequeue().GetComponent<T>();
            pool.transform.SetParent(parent);
            pool.transform.localPosition = Vector3.zero;
        }
        else
        {
            GameObject gameObject = GameObject.Instantiate(_prefab, parent);
            gameObject.name = $"{typeof(T).Name} {TotalCount + 1}";
            pool = gameObject.GetComponent<T>();
        }
        _pooledDict[pool] = false;
        pool.gameObject.SetActive(true);
        if (position != Vector3.zero)
        {
            pool.transform.position = position;
        }
        pool.Initialize();
        return pool;
    }

    public static void Release(T pool, bool force = false)
    {
        bool isPooled = false;
        if (_pooledDict.TryGetValue(pool, out isPooled) || force)
        {
            if (isPooled)
            {
                Debug.LogError($"{pool.gameObject.name} is not valid object");
                return;
            }
            _pooledDict[pool] = true;
            pool.gameObject.SetActive(false);
            _objectQueue.Enqueue(pool.gameObject);
        }
        else
        {
            Debug.LogError($"{pool.gameObject.name} is not object in pool");
        }
    }

    public static T[] GetAllActive()
    {
        List<T> list = new List<T>();
        foreach (var pair in _pooledDict)
        {
            if (!pair.Value)
            {
                list.Add(pair.Key.GetComponent<T>());
            }
        }
        return list.ToArray();
    }
}

public static class PoolManager
{
    private static Dictionary<string, GameObject> _poolableObjects = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> _pooledObjects = new Dictionary<string, GameObject>();

    public static void CreatePool(string name, GameObject prefab)
    {
        _poolableObjects[name] = prefab;
    }

    public static T Get<T>(string name, Transform parent, Vector3 position = new Vector3()) where T : MonoBehaviour
    {
        GameObject prefab = _poolableObjects[name] as GameObject;
        if (prefab == null)
        {
            Debug.LogError($"{name} is not poolable object");
            return null;
        }

        T pool = null;
        if (_pooledObjects[name] != null)
        {
            pool = _pooledObjects[name].GetComponent<T>();
            pool.transform.SetParent(parent);
            pool.transform.localPosition = Vector3.zero;
        }
        else
        {
            GameObject gameObject = GameObject.Instantiate(prefab, parent);
            gameObject.name = $"{typeof(T).Name} {name} {_pooledObjects.Count + 1}";
            pool = gameObject.GetComponent<T>();
        }
        return pool;
    }

    public static void Release<T>(T pool, bool force = false) where T : MonoBehaviour
    {
        string name = pool.gameObject.name;
        if (_pooledObjects.ContainsKey(name) || force)
        {
            if (_pooledObjects.ContainsKey(name))
            {
                Debug.LogError($"{pool.gameObject.name} is not valid object");
                return;
            }
            _pooledObjects[name] = pool.gameObject;
            pool.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"{pool.gameObject.name} is not object in pool");
        }
    }
}