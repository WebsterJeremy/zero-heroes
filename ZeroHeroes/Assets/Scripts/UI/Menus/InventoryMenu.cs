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
    [SerializeField] private Button buttonUse;
    [SerializeField] private Button buttonSell;

    [Header("Content")]
    [SerializeField] private RectTransform rectInventory;
    [SerializeField] private RectTransform rectSellPanel;
    [SerializeField] private GameObject prefabUIItem;
    [SerializeField] private InvSlotElement[] slotElements;

    [Header("Item Details")]
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Image imgIcon;


    #endregion
    #region PrivateVariables

    private List<GameObject> itemElements = new List<GameObject>();

    private InvItemElement selectedItem;
    private InvSlotElement selectedSlot;
    private Item selectedI;


    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();
    }


    #endregion
    #region Getters and Setters

    public InvItemElement GetSelectedItem()
    {
        return selectedItem;
    }

    public void SetSelectedItem(InvItemElement selection)
    {
        if (selectedSlot != null)
        {
            selectedSlot.GetComponent<Image>().color = new Color32(207, 206, 167, 255);
        }

        selectedItem = selection;
        selectedI = selectedItem.item;

        selectedSlot = selectedItem.slotElement;
        selectedSlot.GetComponent<Image>().color = new Color32(167, 166, 127, 255);

        UpdateItemDetails();
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
        buttonUse.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedItem != null && selectedItem.item != null) selectedItem.item.Use();
        });
        buttonSell.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedItem != null && selectedItem.item != null) selectedItem.item.Sell();
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
        Item[] items = GameController.Instance.GetInventory().GetItems();
        bool foundItem = false;

        selectedItem = null;

        if (selectedSlot != null)
        {
            selectedSlot.GetComponent<Image>().color = new Color32(207, 206, 167, 255);
        }

        if (itemElements != null && itemElements.Count > 0)
        {
            for (int i = 0; i < itemElements.Count; i++)
            {
                Destroy(itemElements[i]);
            }

            itemElements.Clear();
        }

        if (items != null && items.Length > 0)
        {
            for (int i = 0;i < items.Length;i++)
            {
                InvSlotElement slotElement = slotElements[items[i].GetSlot()];

                GameObject itemElement = Instantiate(prefabUIItem);
                itemElement.transform.SetParent(slotElement.transform, false);

                itemElement.GetComponent<InvItemElement>().item = items[i];
                itemElement.GetComponent<InvItemElement>().slotElement = slotElement;
                slotElement.itemElement = itemElement.GetComponent<InvItemElement>();

                itemElement.GetComponentInChildren<TextMeshProUGUI>().text = "x" + items[i].GetQuantity().ToString();
                itemElement.GetComponentsInChildren<Image>()[2].sprite = items[i].GetIcon();

                itemElements.Add(itemElement);

                if (selectedItem == null || items[i].GetSlot() == selectedSlot.slotNumber && !foundItem || items[i] == selectedI)
                {
                    SetSelectedItem(itemElement.GetComponent<InvItemElement>());

                    if (items[i] == selectedI) foundItem = true;
                }
            }
        }

        UpdateItemDetails();
    }

    public void UpdateDisplay()
    {
        if (!IsOpened()) return;

        Populate();
        UpdateItemDetails();
    }

    public void UpdateItemDetails()
    {

        if (selectedItem != null)
        {
            txtTitle.text = selectedItem.item.GetTitle();
            txtAmount.text = "x" + selectedItem.item.GetQuantity().ToString();
            txtDescription.text = selectedItem.item.GetDescription();
            imgIcon.sprite = selectedItem.item.GetIcon();

            if (selectedItem.item.GetSellPrice() > 0)
            {
                rectSellPanel.gameObject.SetActive(true);
                txtPrice.text = "Sell for $" + (selectedItem.item.GetSellPrice() * selectedItem.item.GetQuantity());
            }
            else
            {
                rectSellPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            txtTitle.text = "Select a Item!";
            txtAmount.text = "";
            txtDescription.text = "View all the important details of each item in your inventory just by selecting them!";
            imgIcon.sprite = null;

            rectSellPanel.gameObject.SetActive(false);
        }
    }

    #endregion
}