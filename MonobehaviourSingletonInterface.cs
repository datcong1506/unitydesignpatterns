using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonobehaviourSingletonInterface<T> : MonoBehaviour where T : MonobehaviourSingletonInterface<T>
{
    private static T singleton;

    public static T Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton=GameObject.FindObjectOfType<T>();
            }
            return singleton;
        }
    }
    protected virtual void Awake()
    {
        if (singleton == null)
        {
            singleton = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
