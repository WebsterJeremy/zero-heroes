using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    void Update()
    {
        if(GameController.Instance.Player != null && GameController.Instance.CurrentGameState == GameController.GameState.PLAYING) {
            this.transform.position = new Vector3(GameController.Instance.Player.Entity.GameObject.transform.position.x,
                GameController.Instance.Player.Entity.GameObject.transform.position.y, this.transform.position.z);
        }
    }
}
