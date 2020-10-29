using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SnapScrollView : ScrollRect
{
    public int page = 1;

    private List<Transform> pages;

    private float scroll_to = 0f, scroll_distance = 0f, last_move = 0f;
    private float[] pos;

    protected override void Start()
    {
        base.Start();



        if (pages != null && pages.Count > 0)
        {
            ChangePage(page, true);
        }
    }

    public void GetPages()
    {
        if (content.childCount <= 0) return;
        pages.Clear();

        for (int i = 0;i < content.childCount;i++)
        {
            pages.Add(content.GetChild(i));
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //ShowPages();

        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        pos = new float[pages != null ? pages.Count : 1];
        scroll_distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = scroll_distance * i;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (horizontalNormalizedPosition < pos[i] + (scroll_distance / 2) && horizontalNormalizedPosition > pos[i] - (scroll_distance / 2))
            {
                ChangePage(i + 1);

            }
        }
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
            last_move = Time.time + 0.5f;

        if (!Input.GetMouseButton(0) && horizontalNormalizedPosition != scroll_to)
        {
            horizontalNormalizedPosition = Mathf.Lerp(horizontalNormalizedPosition, scroll_to, 0.1f);
            if (Mathf.Abs(horizontalNormalizedPosition - scroll_to) < 0.0001)
                horizontalNormalizedPosition = scroll_to;

            last_move = Time.time + 0.5f;
        }

        if (last_move != 0f && Time.time >= last_move)
        {
            //HidePages();
            last_move = 0f;
        }
    }

    public void ShowPages()
    {
        for (int i = 1; i <= pages.Count; i++)
        {
            pages[i - 1].GetChild(0).gameObject.SetActive(true);
        }
    }

    private void HidePages()
    {
        for (int i = 1; i <= pages.Count; i++)
        {
            pages[i - 1].GetChild(0).gameObject.SetActive(i == page);
        }
    }

    public void ChangePage(int page, bool instant)
    {
        float t = ((float)page - 1f) / (pages.Count - 1);

        if (instant)
        {
            horizontalNormalizedPosition = t;
            //HidePages();
        }
        else
        {
            //ShowPages();
        }

        scroll_to = t;
        this.page = page;
    }
    public void ChangePage(int page) { ChangePage(page, false); }
}