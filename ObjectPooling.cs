using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling
{
    private List<GameObject> poolling;

    private GameObject ownerObject;
    private GameObject poolObject;
    private GameObject container;

    public ObjectPooling(GameObject owner, GameObject pool, int initialNumber = 20)
    {
        if (owner == null || pool == null || initialNumber < 1) return;
        ownerObject = owner;
        container = new GameObject(owner.name + "_pool_" + pool.name + "_container");
        poolObject = pool;


        poolling = new List<GameObject>();
        for (int i = 0; i < initialNumber; i++)
        {
            var newInstance = GameObject.Instantiate(poolObject, Vector3.zero, Quaternion.identity);
            newInstance.transform.parent = container.transform;
            newInstance.SetActive(false);
            poolling.Add(newInstance);
        }
    }

    public void Instantiate(Vector3 posision, Quaternion rotation)
    {
        for (int i = 0; i < poolling.Count; i++)
        {
            if (!poolling[i].activeInHierarchy)
            {
                poolling[i].SetActive(true);
                poolling[i].transform.position = posision;
                poolling[i].transform.rotation = rotation;

                
                poolling[i].transform.position = posision;

                return;
            }
        }
        var newInstance = GameObject.Instantiate(poolObject, posision, rotation);
        newInstance.transform.parent = container.transform;
        poolling.Add(newInstance);
    }

    public void DestroyPool()
    {
        GameObject.Destroy(container);
    }




}
