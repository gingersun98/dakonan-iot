using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject prefab;
    public Queue<GameObject> pool = new Queue<GameObject>();
    public int maxAvailable;
    public bool destroyInstead; // instead of enqueing, it will destroy
    int currentlyAvailable;
    public int prebuildObject = 0;
    List<GameObject> prebuilds;

    private void Start()
    {
        if (prebuildObject <= 0)
        {
            return;
        }

        prebuilds = new List<GameObject>();
        for (int i = 0; i < prebuildObject; i++)
        {
            GameObject prebuilt = GetObject();
            prebuilt.transform.SetParent(transform, false);
            prebuilds.Add(prebuilt);
        }
        foreach (GameObject obj in prebuilds)
        {
            ReturnObject(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        } else
        {
            if (maxAvailable > 0)
            {
                if (currentlyAvailable >= maxAvailable)
                {
                    if (transform.parent.childCount > 0)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        return transform.GetChild(0).gameObject;
                    } else
                    {
                        print("There's nothing to use here!");
                        return null;
                    }
                }
                else
                {
                    currentlyAvailable++;
                    return Instantiate(prefab);
                }
            }
            else
            {
                currentlyAvailable++;
                return Instantiate(prefab);
            }
        }
    }

    public void ReturnObject(GameObject obj)
    {
        if (!destroyInstead)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        } else
        {
            Destroy(obj);
        }
    }
}
