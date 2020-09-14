using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInvTestScript : MonoBehaviour
{
    public InventoryScriptTE inventory;

    public void OnTriggerEnter(Collider other)
    {
        var item1 = other.GetComponent<GroundItem>();

        if (item1)
        {
            invItemTE _item = new invItemTE(item1.item);
            Debug.Log(_item.Id);
            inventory.AddItem(_item, 1);
            Destroy(other.gameObject);

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            inventory.Load();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Cont.Items = new InvSlotTE[16];
    }

}
