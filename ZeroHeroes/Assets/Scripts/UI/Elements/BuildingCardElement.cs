using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingCardElement : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textEco;
    public Image imgIcon;

    public Image panelCost;
    public Image panelEco;

    private BuildingAttributes attributes;
    private ColorBlock colorVar;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Setup(BuildingAttributes attributes)
    {
        if (attributes == null) return;

        textTitle.text = attributes.GetTitle();
        textCost.text = attributes.GetBuyPrice().ToString();
        textEco.text = attributes.GetBuyEcoPrice().ToString();
        imgIcon.sprite = attributes.GetIcon();
        this.attributes = attributes;

        CheckBuyable();
    }

    public void Select()
    {
        if (!CheckBuyable()) return;

        UIController.Instance.GetBuildMenu().Select(attributes.GetID());
    }

    public bool CheckBuyable()
    {
        button = GetComponent<Button>();
        ColorBlock colorVar = button.colors;

        if (GameController.Instance.GetMoney() < attributes.GetBuyPrice() || GameController.Instance.GetPoints() < attributes.GetBuyEcoPrice())
        {
            // Not buyable
            colorVar.pressedColor = new Color32(255,150,150,255);
            button.colors = colorVar;

            if (GameController.Instance.GetMoney() < attributes.GetBuyPrice()) panelCost.color = new Color32(255, 0, 0, 40); else panelCost.color = new Color32(0, 0, 0, 20);
            if (GameController.Instance.GetPoints() < attributes.GetBuyEcoPrice()) panelEco.color = new Color32(255, 0, 0, 40); else panelEco.color = new Color32(0, 0, 0, 20);

            return false;
        }

        // Buyable
        colorVar.pressedColor = new Color32(200, 200, 200, 255);
        button.colors = colorVar;

        panelCost.color = new Color32(0, 0 ,0, 20);
        panelEco.color = new Color32(0, 0, 0, 20);

        return true;
    }
}
