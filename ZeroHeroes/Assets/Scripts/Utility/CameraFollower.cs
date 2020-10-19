using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollower : MonoBehaviour
{
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    private CinemachineVirtualCamera vcam;
    private CinemachineCameraOffset camOffset;
    private CinemachinePixelPerfect camPixel;
    private Transform followTarget;

    private Vector3 touchStart;
    private float curScroll = 0f;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        camOffset = GetComponent<CinemachineCameraOffset>();
        camPixel = GetComponent<CinemachinePixelPerfect>();
    }

    void Update()
    {
        if(GameController.PLAYING()) {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.touchCount == 2 || curScroll != Input.GetAxis("Mouse ScrollWheel"))
            {
                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    Zoom(InputPinch() * 0.01f);
                }
                else
                {
                    Zoom(Input.GetAxis("Mouse ScrollWheel") * 2);
                    curScroll = Input.GetAxis("Mouse ScrollWheel");
                }
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 dir = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                camOffset.m_Offset += (dir/2);
            }

//            followTarget = GameController.Instance.Player.transform;

            vcam.LookAt = followTarget;
            vcam.Follow = followTarget;
        }

    }

    private float InputPinch()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
        Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

        float prevMagnitude = (touch0Prev - touch1Prev).magnitude;
        float curMagnitude = (touch0.position - touch1.position).magnitude;

        float difference = curMagnitude - prevMagnitude;

        return difference;
    }

    private void Zoom(float increment)
    {
        camPixel.enabled = false;
        vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
