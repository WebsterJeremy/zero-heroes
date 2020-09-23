using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowWorld : MonoBehaviour
{
    public Transform pos;
    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void OnGUI()
    {

        /*        Vector2 viewPos = Camera.main.WorldToViewportPoint(pos.position);
                Vector2 screenPos = new Vector2(
                ((viewPos.x * rect.sizeDelta.x) - (rect.sizeDelta.x * 0.5f)),
                ((viewPos.y * rect.sizeDelta.y) - (rect.sizeDelta.y * 0.5f)));

                rect.anchoredPosition = screenPos; */
        rect.position = Camera.main.WorldToScreenPoint(pos.position);
    }
}
