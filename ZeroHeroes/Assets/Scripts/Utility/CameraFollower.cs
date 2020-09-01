using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollower : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private Transform followTarget;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if(GameController.Instance.Player != null && GameController.PLAYING()) {
            followTarget = GameController.Instance.Player.Entity.GameObject.transform;

            vcam.LookAt = followTarget;
            vcam.Follow = followTarget;

//            this.transform.position = new Vector3(GameController.Instance.Player.Entity.GameObject.transform.position.x,
//                GameController.Instance.Player.Entity.GameObject.transform.position.y, this.transform.position.z);
        }
    }
}
