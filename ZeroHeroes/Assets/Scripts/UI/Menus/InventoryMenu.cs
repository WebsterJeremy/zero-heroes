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
    [SerializeField] private Button buttonBuy;
    [SerializeField] private Button buttonTab1;
    [SerializeField] private Button buttonTab2;

    [Header("Content")]
    [SerializeField] private RectTransform rectInventory;
    [SerializeField] private RectTransform rectStore;
    [SerializeField] private RectTransform rectSellPanel;
    [SerializeField] private RectTransform rectPage1;
    [SerializeField] private RectTransform rectPage2;
    [SerializeField] private GameObject prefabUIItem;
    [SerializeField] private GameObject prefabUIProduct;
    [SerializeField] private InvSlotElement[] slotElements;
    [SerializeField] private TextMeshProUGUI txtHeader;

    [Header("Item Details")]
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Sprite imgMask;

    [Header("Sell Details")]
    [SerializeField] private Image imgCurrency;
    [SerializeField] private Sprite spriteDollars;
    [SerializeField] private Sprite spritePoints;

    [Header("Store")]
    [SerializeField] private string[] storeItems;

    #endregion
    #region PrivateVariables

    private List<GameObject> itemElements = new List<GameObject>();

    private InvItemElement selectedItem;
    private InvSlotElement selectedSlot;
    private Item selectedI;

    private int currentPage = -1;

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
        buttonBuy.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            if (selectedProduct != null) BuyProduct();
        });
        buttonTab1.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            OpenPage(0);
        });
        buttonTab2.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            OpenPage(1);
        });
    }

    public void Open(int page)
    {
        base.Open();

        OpenPage(page);
    }

    protected override IEnumerator _Open()
    {
        if (IsOpened()) yield break;

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
        GameController.Instance.CheckObjectivies(TaskAttributes.ObjectiveType.ITEM);
        if (!IsOpened()) return;

        if (currentPage == 0)
        {
            Populate();
            UpdateItemDetails();
        }
        else if (currentPage == 1)
        {
            PopulateStore();
            UpdateProductDetails();
        }
    }

    public void UpdateItemDetails()
    {

        if (selectedItem != null)
        {
            txtTitle.text = selectedItem.item.GetTitle();
            txtAmount.text = "x" + selectedItem.item.GetQuantity().ToString();
            txtDescription.text = selectedItem.item.GetDescription();
            imgIcon.sprite = selectedItem.item.GetIcon();

            if (selectedItem.item.GetEcoSellPrice() > 0)
            {
                rectSellPanel.gameObject.SetActive(true);
                imgCurrency.sprite = spritePoints;
                txtPrice.text = "Sell for " + (selectedItem.item.GetEcoSellPrice() * selectedItem.item.GetQuantity());
            }
            else if (selectedItem.item.GetSellPrice() > 0)
            {
                rectSellPanel.gameObject.SetActive(true);
                imgCurrency.sprite = spriteDollars;
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
            imgIcon.sprite = imgMask;

            rectSellPanel.gameObject.SetActive(false);
        }
    }

    public void OpenPage(int page)
    {
        currentPage = page;

        rectPage1.gameObject.SetActive(page == 0);
        rectPage2.gameObject.SetActive(page == 1);

        buttonTab1.GetComponent<Image>().color = (page == 0 ? new Color32(229, 228, 194, 255) : new Color32(212, 211, 174,255));
        buttonTab2.GetComponent<Image>().color = (page == 1 ? new Color32(229, 228, 194, 255) : new Color32(212, 211, 174,255));

        if (page == 0)
        {
            txtHeader.text = "Inventory";

            Populate();
        }
        else if (page == 1)
        {
            txtHeader.text = "Marketplace";

            PopulateStore();
        }

        buttonUse.gameObject.SetActive(page == 0);
        buttonSell.gameObject.SetActive(page == 0);
        buttonBuy.gameObject.SetActive(page == 1);
    }

    #endregion
    #region Store

    ShopCardElement selectedProduct;
    List<ShopCardElement> productElements = new List<ShopCardElement>();

    private void BuyProduct()
    {
        if (selectedProduct == null || selectedProduct.item == null) return;

        if (GameController.Instance.GetMoney() >= (selectedProduct.item.GetBuyPrice() * selectedProduct.item.GetBuyQuantity()))
        {
            GameController.Instance.GetInventory().GiveItem(selectedProduct.item.GetID(), selectedProduct.item.GetBuyQuantity(), -1);
            GameController.Instance.GiveMoney(-(int)(selectedProduct.item.GetBuyPrice() * selectedProduct.item.GetBuyQuantity()));

            PopulateStore();
            UpdateProductDetails();

            SoundController.PlaySound("sell_buy_item");
        }
    }

    public void SetSelectedProduct(ShopCardElement selection)
    {
        if (selectedProduct != null)
        {
            selectedProduct.GetComponent<Image>().color = new Color32(245, 245, 245, 255);
        }

        selectedProduct = selection;

        selectedProduct.GetComponent<Image>().color = new Color32(225, 225, 225, 255);

        UpdateProductDetails();
    }

    public void PopulateStore()
    {
        if (storeItems == null || storeItems.Length < 1) return;

        string oldSelected = null;
        if (selectedProduct != null) oldSelected = selectedProduct.textTitle.text;

        selectedProduct = null;


        if (selectedProduct != null)
        {
            selectedProduct.GetComponent<Image>().color = new Color32(245, 245, 245, 255);
        }

        if (productElements != null && productElements.Count > 0)
        {
            for (int i = 0; i < productElements.Count; i++)
            {
                Destroy(productElements[i].gameObject);
            }

            productElements.Clear();
        }

        for (int i = 0; i < storeItems.Length; i++)
        {
            GameObject shopElement = Instantiate(prefabUIProduct);
            shopElement.transform.SetParent(rectStore.transform, false);

            ShopCardElement shopCardElement = shopElement.GetComponent<ShopCardElement>();
            ItemAttributes attr = Item.FindItemAttributes(storeItems[i]);
            if (attr == null) continue;

            shopCardElement.Setup(attr);

            productElements.Add(shopCardElement);

            if (selectedProduct == null || attr.GetTitle() == oldSelected)
            {
                SetSelectedProduct(shopCardElement);
            }
        }
    }

    public void UpdateProductDetails()
    {

        if (selectedProduct != null)
        {
            txtTitle.text = selectedProduct.item.GetTitle();
            txtAmount.text = "";
            txtDescription.text = selectedProduct.item.GetDescription();
            imgIcon.sprite = selectedProduct.item.GetIcon();

            if (selectedProduct.item.GetBuyPrice() > 0)
            {
                rectSellPanel.gameObject.SetActive(true);
                imgCurrency.sprite = spriteDollars;

                txtPrice.text = "Buy "+ (selectedProduct.item.GetBuyQuantity() > 1 ?(selectedProduct.item.GetBuyQuantity() + " for $" ) : " $") + (selectedProduct.item.GetBuyPrice() * selectedProduct.item.GetBuyQuantity());
            }
            else
            {
                rectSellPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            txtTitle.text = "Select a Product!";
            txtAmount.text = "";
            txtDescription.text = "View all the important details of each product in the store just by selecting them!";
            imgIcon.sprite = imgMask;

            rectSellPanel.gameObject.SetActive(false);
        }
    }

    #endregion
}