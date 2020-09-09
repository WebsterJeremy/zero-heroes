using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MenuBase {



    public Button buttonBuy;
    public Button buttonBack;
    public Image imagePortrait;
    public Text textName;
    public GameObject ScrollPanel;

    public GameObject ListItemPrefab;

    public Image defaultImage;
    
    private string[] itemDescriptions;
    
    protected override void Start() {
        base.Start();

        ShopItem[] items = {
            new ShopItem(defaultImage, "Item 1", 20, "Item 1 has a description like this"),
            new ShopItem(defaultImage, "Item 2", 50, "Item 2 has a different description"),
            new ShopItem(defaultImage, "Item 3", 50, "Item 2 has a different description"),
            new ShopItem(defaultImage, "Item 4", 50, "Item 2 has a different description")
        };

        itemDescriptions = new string[items.Length];

        loadNewShop(defaultImage, "NPC NAME", items);
    }

    public void loadNewShop(Image _portrait, string _name, ShopItem[] _items) {

        textName.text = _name;
        imagePortrait = _portrait;

        for (int i = 0; i < _items.Length; i++) {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ShopListItemController controller = (ShopListItemController) newItem.GetComponent(typeof(ShopListItemController));

            controller.image = _items[i].image;
            controller.itemName.text = _items[i].itemName+" $"+_items[i].cost;
            itemDescriptions[i] = _items[i].description;

            newItem.transform.SetParent(ScrollPanel.transform,false);
            newItem.transform.localScale = Vector3.one;
        }
        
    }


    protected override void AddButtonListeners() {
        buttonBuy.onClick.AddListener(() => {
            SoundController.PlaySound("button");

            //buttonPlay.interactable = false;
            
            //Run Button Code
        });

        buttonBack.onClick.AddListener(() => {
            SoundController.PlaySound("button");
            
            //Run Button Code
        });
    }

    protected override IEnumerator _Open() {
        rectMenu.gameObject.SetActive(true);

        return null;
    }

    protected override IEnumerator _Close() {
        /*
        EffectController.TweenFade(rectMenu.GetComponent<CanvasGroup>(), 1f, 0f, 3f, () => {
            rectMenu.gameObject.SetActive(false);
        });

        GameController.Instance.StartGame();
        */
        return null;
    }

    private void Update() {
        //AnimateBackgroundScroller();
    }
}