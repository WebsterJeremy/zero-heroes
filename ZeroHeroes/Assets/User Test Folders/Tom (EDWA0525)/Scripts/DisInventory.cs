using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisInventory : MonoBehaviour
{
    public InventoryScript inv;


    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int COL_NUM;
    public int Y_SPACE_BETWEEN_ITEMS;

    Dictionary<InvSlot, GameObject> itemsDisplayed = new Dictionary<InvSlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inv.Cont.Count; i++)
        {
            var obj = Instantiate(inv.Cont[i].item.preFab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPos(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inv.Cont[i].val.ToString("n0");
            itemsDisplayed.Add(inv.Cont[i], obj);
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inv.Cont.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inv.Cont[i]))
            {
                itemsDisplayed[inv.Cont[i]].GetComponentInChildren<TextMeshProUGUI>().text = inv.Cont[i].val.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inv.Cont[i].item.preFab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPos(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inv.Cont[i].val.ToString("n0");
                itemsDisplayed.Add(inv.Cont[i], obj);
            }
        }
        
    }

    public Vector3 GetPos(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % COL_NUM)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i/COL_NUM)), 0f) ;
    }
}
