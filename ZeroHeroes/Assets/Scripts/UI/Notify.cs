using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Notify : MonoBehaviour
{
    public string label;
    public int quantity = 1;
    public TextMeshProUGUI textQuantity;

    public void Setup(string label, int quantity, Sprite icon, Transform Transform, Vector3 offset)
    {
        this.label = label;
        this.quantity = quantity;

        textQuantity.text = "<size=%30>x</size>" + quantity;
        if (icon != null ) GetComponent<Image>().sprite = icon;
        GetComponent<UITween>().SetFollowTransform(Transform);
        GetComponent<UITween>().SetFollowOffset(offset);
    }

    public void SetLabel(string label)
    {
        this.label = label;
    }

    public string GetLabel()
    {
        return label;
    }

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
        textQuantity.text = "<size=%30>x</size>" + quantity;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void SetIcon(Sprite icon)
    {
        if (icon != null) GetComponent<Image>().sprite = icon;
    }

    public void SetPosition(Vector3 position)
    {
        GetComponent<UITween>().SetFollowVector(position);
    }
}
