using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (GameController.Instance.CurrentGameState == GameController.GameState.PLAYING) {
            HandleWorldTouch();
        }

    }

   private void HandleWorldTouch() {
        //ignore touch handle if interacted with ui
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        /*
        lastPosition = new Position(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));

        MoveTileHover(lastPosition);

        if (Input.GetMouseButtonDown(0)) {
            //todo this works for touch screen too may have to resolve to Input.GetTouch,, etc..
            
            GameController.Instance.World.InteractWithPosition(mousePos, lastPosition);
        }
        */
    }

    public GameObject tileHover;

    /*
    private void MoveTileHover(Position _position) {
        tileHover.transform.position = _position.ToVector();
    }
    */
}
