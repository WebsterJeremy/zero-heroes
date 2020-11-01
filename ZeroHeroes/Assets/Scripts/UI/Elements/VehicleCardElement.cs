using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VehicleCardElement : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textEco;
    public TextMeshProUGUI textTime;
    public Image imgIcon;

    public Image panelCost;
    public Image panelEco;

    public RectTransform rectGarageRequired;

    private ColorBlock colorVar;
    private Button button;

    public MapMenu.TransportMode transportMode;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Setup(MapMenu.TransportMode transportMode)
    {

        textTitle.text = transportMode.title;
        textCost.text = transportMode.price.ToString();
        textEco.text = transportMode.points.ToString();
        imgIcon.sprite = transportMode.icon;
        this.transportMode = transportMode;

        string timeStr;

        if (transportMode.time >= 3600) timeStr = Mathf.FloorToInt((transportMode.time) / 3600) + " Hours";
        else if (transportMode.time >= 60) timeStr = Mathf.FloorToInt((transportMode.time) / 60) + " Minutes";
        else timeStr = (int)(transportMode.time) + " Seconds";

        textTime.text = timeStr;

        CheckBuyable();
    }

    public void Select()
    {
        if (!CheckBuyable()) return;

        UIController.Instance.GetMapMenu().Select(this);
    }

    public bool CheckBuyable()
    {
        button = GetComponent<Button>();
        ColorBlock colorVar = button.colors;

        bool foundGarage = false;
        if (transportMode.requireGarage)
        {
            foundGarage = (GameController.Instance.GetBuilding("garage") != null);

            rectGarageRequired.gameObject.SetActive(!foundGarage);
        }

        if (GameController.Instance.GetMoney() < transportMode.price || GameController.Instance.GetPoints() < transportMode.points)
        {
            // Not buyable
            colorVar.pressedColor = new Color32(255,150,150,255);
            button.colors = colorVar;

            if (GameController.Instance.GetMoney() < transportMode.price) panelCost.color = new Color32(255, 0, 0, 40); else panelCost.color = new Color32(0, 0, 0, 20);
            if (GameController.Instance.GetPoints() < transportMode.points) panelEco.color = new Color32(255, 0, 0, 40); else panelEco.color = new Color32(0, 0, 0, 20);

            return false;
        }

        if (transportMode.requireGarage && !foundGarage) return false;

        // Buyable
        colorVar.pressedColor = new Color32(200, 200, 200, 255);
        button.colors = colorVar;

        panelCost.color = new Color32(0, 0 ,0, 20);
        panelEco.color = new Color32(0, 0, 0, 20);

        return true;
    }
}
