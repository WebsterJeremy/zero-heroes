using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem
{

    public Image image;
    public string itemName = "name";
    public string description= "description";
    public int cost;

    public ShopItem(Image _image, string _name, int _cost, string _description) {
        image = _image;
        itemName = _name;
        cost = _cost;
        description = _description;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
