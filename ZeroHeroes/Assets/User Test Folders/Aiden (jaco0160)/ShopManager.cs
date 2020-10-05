﻿using System.Collections;
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
    public Text itemDescription;
    public GameObject ScrollPanel;

    public GameObject ListItemPrefab;

    public Image defaultImage;
    
    private string[] itemDescriptions;

    private int selectedItem = -1;

    protected override void Start() {
        base.Start();

        ShopItem[] items = {
            new ShopItem(defaultImage, "Item 1", 20, "Item 1 has a description like this"),
            new ShopItem(defaultImage, "Item 2", 50, "Item 2 has a different description"),
            new ShopItem(defaultImage, "Item 3", 60, "Item 3 has a different description"),
            new ShopItem(defaultImage, "Item 4", 500, "Item 4 has a different description")
        };

        itemDescriptions = new string[items.Length];

        loadNewShop(defaultImage, "NPC NAME", items);
    }

    public void loadNewShop(Image _portrait, string _name, ShopItem[] _items) {

        textName.text = _name;
        imagePortrait = _portrait;

        //Button[] itemButtons = new Button[_items.Length];

        

        for (int i = 0; i < _items.Length; i++) {
            /*
            itemButtons[i] = new Button();

            itemButtons[i].transform.SetParent(ScrollPanel.transform, false);
            itemButtons[i].transform.localScale = Vector3.one;

            ThisButton.GetComponentInChildren(Text).text = "testing";
            */
            
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ShopListItemController controller = (ShopListItemController) newItem.GetComponent(typeof(ShopListItemController));

            //controller.image = _items[i].image;
            controller.button.GetComponentInChildren<Text>().text = _items[i].itemName+" $"+_items[i].cost;
            itemDescriptions[i] = _items[i].description;

            newItem.transform.SetParent(ScrollPanel.transform,false);

            float itemHeight = 50;

            int index = i;
            controller.button.onClick.AddListener(() => {
                setSelectedItem(index);
            });

            newItem.transform.position += new Vector3(0, (-i * itemHeight), 0);
        }
        
    }

    private void setSelectedItem(int index) {
        //Debug.Log("selected: " + index);
        selectedItem = index;
        itemDescription.text = itemDescriptions[index];
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
        yield return null;
    }

    protected override IEnumerator _Close() {
        yield return null;

        /*
        EffectController.TweenFade(rectMenu.GetComponent<CanvasGroup>(), 1f, 0f, 3f, () => {
            rectMenu.gameObject.SetActive(false);
        });

        GameController.Instance.StartGame();
        */
    }

    private void Update() {
        //AnimateBackgroundScroller();
    }
}