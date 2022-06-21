using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScripableSingletonInterface<T> : ScriptableObject where T : ScripableSingletonInterface<T>
{
    private static T singleton;
    public static T Singleton
    {
        get
        {
            if (singleton == null)
            {
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("countnt find any singleton scriableobject in resources");
                }
                else
                {
                    if (assets.Length > 1)
                    {
                        Debug.LogWarning("multiple singleton of a scriableobject found in resource");
                    }
                }
                singleton = assets[0];
            }
            return singleton;
        }
    }
}
