using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    //Function for rotating the ships
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag == "PrefabTilePoint")
            {
                transform.GetChild(i).transform.position -= new Vector3(0,0,10);
            }
        }
    }
}
