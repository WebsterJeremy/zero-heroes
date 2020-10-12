using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MenuBase
{
    #region AccessVariables


    [Header("Buttons")]
    [SerializeField] private Button buttonClose;

    [Header("Content")]
    [SerializeField] private RectTransform rectInventory;
    [SerializeField] private GameObject prefabUIItem;


    #endregion
    #region PrivateVariables

    private List<GameObject> itemElements = new List<GameObject>();

    private Item selectedItem;


    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();
    }


    #endregion
    #region Core


    protected override void AddButtonListeners()
    {
        buttonClose.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Close();
        });
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

        Populate();

        UIController.Instance.EnableBlur();
        rectMenu.gameObject.SetActive(true);
    }

    protected override IEnumerator _Close()
    {
        if (!IsOpened()) yield break;

        rectMenu.gameObject.SetActive(false);
    }

    private void Populate()
    {
        /*
         * Population Process
         * 1. See if we have either more slots or more items
         * 2. Use a for loop with the highest number
         * 3. If the item count is higher, when no more slots avaiable add another slot
         *    If the slot count is higher, when no more items destroy or set to inactive the slots
         *  -- Make sure the same item is selected in the details panel when menu is re-opened or default to first (also same scroll ??)
         */


        Item[] items = GameController.Instance.GetInventory().GetItems();

        if (items != null && items.Length > 0)
        {
            int toLoop = items.Length > itemElements.Count ? items.Length : itemElements.Count;

            for (int i = 0;i < toLoop;i++)
            {
                if (i >= items.Length)
                {
                    itemElements[i].SetActive(false);
                }
                else if (i >= itemElements.Count)
                {
                    // Create new item element

                    GameObject element = Instantiate(prefabUIItem);
                    element.transform.SetParent(rectInventory);

                    element.GetComponentInChildren<TextMeshProUGUI>().text = "x"+ items[i].GetAmount().ToString();
                    element.GetComponentsInChildren<Image>()[2].sprite = items[i].GetIcon();

                    itemElements.Add(element);
                }
                else
                {
                    // Replace old item element icon/amount

                    itemElements[i].GetComponentInChildren<TextMeshProUGUI>().text = "x"+ items[i].GetAmount().ToString();
                    itemElements[i].GetComponentsInChildren<Image>()[2].sprite = items[i].GetIcon();
                }
            }
        }
    }

    public void UpdateDisplay()
    {
        if (!IsOpened()) return;

        Populate();
    }

    public void GiveNewItem(string item_id)
    {
        GameController.Instance.GetInventory().GiveItem(item_id, 5);
    }


    #endregion
}
