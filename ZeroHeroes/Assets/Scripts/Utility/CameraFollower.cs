using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollower : MonoBehaviour
{
    public static bool ENABLED = true;
    public static bool PANNING_ENABLED = true;

    public float panSpeed = 20f;
    public float zoomSpeedTouch = 0.1f;
    public float zoomSpeedMouse = 0.5f;

    public Vector2 boundsX = new Vector2(-10f, 5f);
    public Vector2 boundsY = new Vector2(-18f, 4f);
    public Vector2 zoomBounds = new Vector2(10f, 85f);

    private CinemachineVirtualCamera vcam;

    private Vector3 lastPanPosition;
    private int panFingerId;

    private bool wasZoomingLastFrame;
    private Vector2[] lastZoomPositions;

    IEnumerator Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();

        yield return new WaitForSeconds(0.1f);

        GetComponent<CinemachinePixelPerfect>().enabled = false;
    }

    void Update()
    {
        if (GameController.PLAYING() && ENABLED)
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
        }

    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                if (!PANNING_ENABLED) return;
                wasZoomingLastFrame = false;

                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, zoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, zoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        if (!PANNING_ENABLED) return;
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, offset.y * panSpeed, 0);

        transform.Translate(move, Space.World);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, boundsX[0], boundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, boundsY[0], boundsY[1]);
        pos.z = -10;
        transform.position = pos;

        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize - (offset * speed), zoomBounds[0], zoomBounds[1]);
    }
}
