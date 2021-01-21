using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class-helper for pooling objects. Main pool logic described here.
/// </summary>
public abstract class ObjectsPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T prefab;

    public static ObjectsPool<T> Instance { get; private set; }
    private Queue<T> objects = new Queue<T>();

    private void Awake()
    {
        Instance = this;
    }

    public T Get()
    {
        if (objects.Count == 0)
            AddObjects(1);

        return objects.Dequeue();
    }

    public T Get(Transform parent, bool resetTransform = false)
    {
        var pooledObject = Get();
        pooledObject.gameObject.SetActive(true);
        pooledObject.transform.SetParent(parent);

        if (resetTransform)
        {
            pooledObject.transform.localPosition = Vector3.zero;
            pooledObject.transform.localRotation = Quaternion.identity;
        }

        return pooledObject;
    }

    public T Get(Transform parent, Vector3 relativePosition, Quaternion relativeRotation)
    {
        var pooledObject = Get();
        pooledObject.gameObject.SetActive(true);
        pooledObject.transform.SetParent(parent);

        pooledObject.transform.localPosition = relativePosition;
        pooledObject.transform.localRotation = relativeRotation;

        return pooledObject;
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }

    private void AddObjects(int count)
    {
        var newObject = Instantiate(prefab);
        newObject.gameObject.SetActive(false);
        objects.Enqueue(newObject);
    }
}
