using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; //Initialize from/in the inspector
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }

    public void Clear(int index)
    {
        foreach (GameObject item in pools[index])
        {
            item.SetActive(false);
        }

    }

    public void ClearAll()
    {
        for (int index = 0; index < pools.Length; index++)
        {
            foreach (GameObject item in pools[index])
            {
                item.SetActive(false);
            }
        }
    }
}