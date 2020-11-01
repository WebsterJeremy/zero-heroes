using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingMarker : MonoBehaviour
{
    public SpriteRenderer icon;
    public SpriteRenderer outline;
    public Transform tileContainer;
    public GameObject tileHover;
    public UITween markerHUD;

    private Vector2 size;

    private bool isDragging;
    private Vector2 mousePosition;
    private float overUI = 0;
    private string building_id;

    private GameObject tiledIcon;

    private List<GameObject> tiles;

    public void Setup(BuildingAttributes buildingAttributes)
    {
        icon.sprite = !buildingAttributes.GetTiled() ? buildingAttributes.GetIcon() : null;
        building_id = buildingAttributes.GetID();
        size = buildingAttributes.GetSize();

        if (tiledIcon != null) Destroy(tiledIcon);

        float max = size.x > size.y ? size.y : size.x;
        outline.transform.localScale = new Vector3(max, max, 1);
        outline.transform.localPosition = new Vector3(size.x * 0.5f - 1f, size.y * 0.5f - 1f, 0);

        markerHUD.SetFollowOffset(new Vector3(size.x * 0.5f - 1f, size.y * 0.5f + (size.y / 2) + 2f, 0));

        for (int i = 0; i < tileContainer.childCount; i++) Destroy(tileContainer.GetChild(i).gameObject);

        for (int y = 0;y < size.y;y++)
        {
            for (int x = 0;x < size.x;x++)
            {
                GameObject tile = Instantiate(tileHover);
                tile.transform.SetParent(tileContainer);
                tile.transform.localPosition = new Vector3(x - 1,y - 1,0);
            }
        }

        if (buildingAttributes.GetTiled())
        {
            tiledIcon = Instantiate(buildingAttributes.GetPrefab());
            tiledIcon.transform.SetParent(icon.transform);
            tiledIcon.GetComponent<Entity>().enabled = false;
            tiledIcon.GetComponent<BoxCollider2D>().enabled = false;
            tiledIcon.transform.localPosition = new Vector3(0, 0, 0);

            // Position
        }
    }

    public void StartDragging()
    {
        isDragging = true;
        CameraFollower.PANNING_ENABLED = false;
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() && Time.time < overUI) return;

        StartDragging();
    }

    public void OnMouseUp()
    {
        isDragging = false;
        CameraFollower.PANNING_ENABLED = true;
    }

    private void Update()
    {
        if (isDragging)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            transform.Translate(mousePosition);

            SnapPosition();
        }

        if (EventSystem.current.IsPointerOverGameObject()) overUI = Time.time + 0.2f;
    }

    private void SnapPosition()
    {
        float snappedX = (float)Math.Round(transform.position.x, MidpointRounding.AwayFromZero);
        float snappedY = (float)Math.Round(transform.position.y, MidpointRounding.AwayFromZero);

        transform.position = new Vector3(snappedX, snappedY, 0);

        if (Building.IsBlocked(new Vector2(transform.position.x, transform.position.y), size))
        {
            outline.color = new Color32(255, 0, 0, 40);
        }
        else
        {
            outline.color = new Color32(0, 0, 0, 20);
        }
    }
}
