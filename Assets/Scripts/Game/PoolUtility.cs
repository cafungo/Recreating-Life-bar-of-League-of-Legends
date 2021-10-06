using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolUtility
{
    List<GameObject> poolList = new List<GameObject>();
    public List<GameObject> PoolList => poolList;
    public GameObject prefab;
    

    public GameObject GetObject(Transform parent=null)
    {
        foreach (var obj in poolList)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        var newObj = GameObject.Instantiate(prefab);
        if (parent != null)
            newObj.transform.SetParent(parent,false);
        newObj.SetActive(true);
        poolList.Add(newObj);
        return newObj;
    }

    
}
