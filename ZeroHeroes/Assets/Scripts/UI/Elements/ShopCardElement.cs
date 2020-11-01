using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopCardElement : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textQuantity;
    public Image imgIcon;

    public Image panelCost;

    private Button button;

    public ItemAttributes item;

    private void Start()
    {
        button = GetComponent<Button>();

        AddButtonListeners();
    }

    public void Setup(ItemAttributes attr)
    {

        textTitle.text = attr.GetTitle();
        textQuantity.text = attr.GetBuyQuantity() > 1 ? "x"+ attr.GetBuyQuantity() : "";
        textCost.text = (attr.GetBuyPrice() * attr.GetBuyQuantity()).ToString();
        imgIcon.sprite = attr.GetIcon();

        this.item = attr;

        CheckBuyable();
    }

    private void AddButtonListeners()
    {
        button.onClick.AddListener(() =>
        {
            SoundController.PlaySound("button");

            Select();
        });
    }

    public void Select()
    {
        if (!CheckBuyable()) return;

        UIController.Instance.GetInventoryMenu().SetSelectedProduct(this);
    }

    public bool CheckBuyable()
    {
        button = GetComponent<Button>();
        ColorBlock colorVar = button.colors;

        if (GameController.Instance.GetMoney() < (item.GetBuyPrice() * item.GetBuyQuantity()))
        {
            // Not buyable
            colorVar.pressedColor = new Color32(255,150,150,255);
            button.colors = colorVar;

            panelCost.color = new Color32(255, 220, 220, 255);

            return false;
        }

        // Buyable
        colorVar.pressedColor = new Color32(200, 200, 200, 255);
        button.colors = colorVar;

        panelCost.color = new Color32(245, 245, 245, 255);

        return true;
    }
}
