using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnable : MonoBehaviour
{
    public GameObject[] objectsToEnable;
   
    public void EnableObjects()
    {
        for (int i = 0; i < objectsToEnable.Length; i++)
        {
            objectsToEnable[i].SetActive(true);
        }
    }
}
