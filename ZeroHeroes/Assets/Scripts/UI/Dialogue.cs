using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    #region AccessVariables


    [Header("Reader")]
    [SerializeField] private float scrollSpeed = 0.05f;
    [SerializeField] private float delayBetweenLines = 0.4f;
    [SerializeField] private float transistionDuration = 0.04f;
    [SerializeField] private bool autoNextLine = true;

    [Header("Setup")]
    [SerializeField] private GameObject dialogueBox;

    #endregion
    #region PrivateVariables


    private string[] lines;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI txt;
    private bool shown = false;
    private int currentLine = 0;


    #endregion
    #region Initlization


    private static Dialogue instance;
    public static Dialogue Instance // Assign Singlton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Dialogue>();
                if (Instance == null)
                {
                    var instanceContainer = new GameObject("Dialogue");
                    instance = instanceContainer.AddComponent<Dialogue>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }


    #endregion
    #region Getters & Setters


    #endregion
    #region Main


    public void StartReading(string message, char delimitter) { StartCoroutine(_StartReading(message, delimitter)); }
    public IEnumerator _StartReading(string message, char delimitter)
    {
        dialogueBox.SetActive(true);
        canvasGroup.alpha = 0f;
        shown = true;

        lines = message.Split(delimitter);
        txt.text = "";

        yield return _AlphaFade(0f, 1f);

        for (int i = 0; i < lines.Length; i++) // Instead of an loop, when read line is finished call the next line (So we can pause it)
        {
            yield return ReadLine(lines[i], i + 1);
        }

        yield return _AlphaFade(1f, 0f);

        dialogueBox.SetActive(false);
        shown = false;
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
    #region

    public class QuestLine
    {
        public string title;
//        public Task[] tasks;
    }

    public class Objectivite
    {
        public string text;
    }

    public class Condition
    {
        public bool passed = false;
    }

    public class Reward
    {
        
    }

    private class Task
    {
        public int id = -1;
        public string npc;
        public Objectivite[] objectivites;
        public QuestLine questLine;
        public string dialog;
        public Condition[] conditions;
        public Reward[] rewards;

        // Check done previous quest

        public Task(int id, string npc, Objectivite[] objectivites, QuestLine questLine, string dialog, Condition[] conditions, Reward[] rewards)
        {
            this.id = id;
            this.objectivites = objectivites;
            this.questLine = questLine;
            this.dialog = dialog;
            this.conditions = conditions;
            this.rewards = rewards;
        }

        public string GetDialog()
        {
            return dialog;
        }
    }

    Task[] tasks =
    {
        new Task(0,"npc_mayor",null,null,@"The litter on the land is <b>harmful</b> to local wildlife. 
It also increases water and soil pollution and destroys animal habitats.
Will you help clean it up?",null,null),
        new Task(1,"npc_birdwatcher_0",null,null,@"Native birds have varying habitat needs. 
Will you plant some tall and medium trees, and some shrubs to help the conservation of native bird life?",null,null),
        new Task(2,"npc_gardner_0",null,null,@"Rainwater tanks can store water for many uses and reduces the need for infrastructure such as dams and desalination plants. 
Could you install one for me?",null,null)
    };

    public void ReceiveTask(int id)
    {
        StartReading(tasks[id].GetDialog());
        // Check if want to accept task?
        // GiveTask(ID);
    }

    #endregion
}
