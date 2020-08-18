/* PlayerController.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using Assets.Scripts;
using Assets.Scripts.world;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    private void Update() {
        if (GameController.instance.isPlaying) {
            DetectInGamePlayerInput();
        }


        //other functionality 
    }

    private void DetectInGamePlayerInput() {
        //test input...
        if (Input.GetKeyDown(KeyCode.Return)) {
            GameController.instance.Player.AttemptMoveTo(new Position(1, 8));
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            GameController.instance.Player.AttemptMoveTo(new Position(9, 0));
        }
        
        
        if (Input.GetKeyDown(KeyCode.E)) {
            GameController.instance.Player.Inventory.DropItem("0", 1);
        }

        HandleWorldTouch();
    }

    private void HandleWorldTouch() {
        //ignore touch handle if interacted with ui
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
        //todo this works for touch screen too may have to resolve to Input.GetTouch,, etc..
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        Position pos = new Position(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));

        GameController.instance.Player.InteractWithPosition(mousePos, pos);
    }
        
    }
}
