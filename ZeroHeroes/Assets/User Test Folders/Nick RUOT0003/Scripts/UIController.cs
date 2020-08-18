/* UIController.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 18/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Canvas canvas;

    [Header("UI Panels")]
    public GameObject panelInteraction;
    public GameObject panelInventory;

    public GameObject interactionItem;


    private void Awake() {
        instance = this;
        CloseAllPanels();
    }

    public void CloseAllPanels() {
        ShowHideInteractionPanel(false);
        ShowHideInventoryPanel(false);
    }


    #region Interaction Panel

    public void SetInteractionPanelPosition(Vector3 mousePos) {
        RectTransform rTransform = panelInteraction.GetComponent<RectTransform>();

        float width  = rTransform.rect.width;
        float height = rTransform.rect.height;

        //                          offset so the toolbar is a litte off...
        Vector3 newPos = mousePos - new Vector3(width / 2, -16);

        newPos.z = 0f;

        //just know that this clamps the toolbar and dont mess with it
        if ((newPos.x + width * 2 * canvas.scaleFactor / 2) > Screen.width) {
            newPos.x = Screen.width - width;
        }

        if ((newPos.x * canvas.scaleFactor / 2) < 0) {
            newPos.x = 0;
        }

        if ((newPos.y + height * 2 * canvas.scaleFactor / 2) > Screen.height) {
            newPos.y = Screen.height;
        }

        if ((newPos.y * canvas.scaleFactor / 2) < 0) {
            newPos.y = height;
        }

        panelInteraction.transform.position = newPos;
    }
    public void ShowHideInteractionPanel(bool state) {
        panelInteraction.SetActive(state);
        //todo clear children
    }

    public void ClearInteractionPanelItems() {
        foreach (Transform child in panelInteraction.transform) {
            Destroy(child.gameObject);
        }
    }

    public void AddInteractionPanelItem(string _text, Action callback) {
        //spawn item
        GameObject item = Instantiate(interactionItem);
        item.transform.SetParent(panelInteraction.transform, false);

        //set text
        item.GetComponentInChildren<Text>().text = _text;

        //set callback
        item.GetComponent<Button>().onClick.AddListener(() => { callback.Invoke(); ShowHideInteractionPanel(false); });
    }


    #endregion

    public void ShowHideInventoryPanel(bool state) {
        panelInventory.SetActive(state);
    }


}
