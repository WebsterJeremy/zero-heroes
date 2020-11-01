﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    #region AccessVariables


    [Header("Reader")]
    [SerializeField] private float scrollSpeed = 0.05f;
    [SerializeField] private float delayBetweenLines = 0.4f;
    [SerializeField] private float transistionDuration = 0.04f;
    [SerializeField] private bool autoNextLine = true;

    [Header("Setup")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Image imgAvatar;
    [SerializeField] private TextMeshProUGUI textNpc;

    #endregion
    #region PrivateVariables


    private string[] lines;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI txt;
    private bool shown = false;
    private int currentLine = 0;


    #endregion
    #region Initlization


    private static Dialog instance;
    public static Dialog Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Dialog>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("Dialog");
                    instance = instanceContainer.AddComponent<Dialog>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }


    #endregion
    #region Getters & Setters


    #endregion
    #region Main

    public void StartReading(string message, NpcAttributes npc) { gameObject.SetActive(true); StartCoroutine(_StartReading(message, '.', npc)); }
    public void StartReading(string message, char delimitter) { gameObject.SetActive(true); StartCoroutine(_StartReading(message, delimitter, null)); }
    public IEnumerator _StartReading(string message, char delimitter, NpcAttributes npc)
    {
        dialogueBox.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        txt = GetComponentInChildren<TextMeshProUGUI>();

        canvasGroup.alpha = 0f;
        shown = true;

        if (npc != null)
        {
            textNpc.text = npc.GetTitle();
            imgAvatar.sprite = npc.GetIcon();
        }

        UIController.Instance.EnableBlur();

        lines = message.Split(delimitter);
        txt.text = "";

        yield return _AlphaFade(0f, 1f);

        for (int i = 0; i < lines.Length; i++) // Instead of an loop, when read line is finished call the next line (So we can pause it)
        {
            yield return ReadLine(lines[i], i + 1);
        }

        yield return _AlphaFade(1f, 0f);

        dialogueBox.SetActive(false);
        gameObject.SetActive(false);
        shown = false;

        UIController.Instance.DisableBlur();
    }

    private IEnumerator _AlphaFade(float start, float end)
    {
        float tranCounter = 0f;
        while (tranCounter < transistionDuration)
        {
            tranCounter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, tranCounter / transistionDuration);

            yield return null;
        }
    }

    public void StartReading(string message)
    {
        if (shown) return;

        StartReading(message, '.');
    }

    public void PauseReading()
    {
        // Stop Timer & Pause Loop
    }

    public void StopReading()
    {
        // Stop Timer
        // Hide Dialog Box
    }

    private IEnumerator ReadLine(string line, int nextLine)
    {
        if (line.Length < 4) yield return null;

        txt.maxVisibleCharacters = 0;
        txt.SetText(line.Trim() + (nextLine < lines.Length ? "." : ""));

        yield return new WaitForSeconds(0.01f);
        int totalVisibleCharacters = txt.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            counter++;

            txt.maxVisibleCharacters = counter;

            if (counter >= totalVisibleCharacters)
            {
                yield return new WaitForSeconds(delayBetweenLines);
                break;
            }

            yield return new WaitForSeconds(scrollSpeed);
        }
    }


    #endregion
}
