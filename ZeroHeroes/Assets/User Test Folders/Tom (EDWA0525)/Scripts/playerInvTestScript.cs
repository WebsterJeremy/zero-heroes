﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInvTestScript : MonoBehaviour
{
    public InventoryScript inventory;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();

        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Cont.Clear();
    }

}
