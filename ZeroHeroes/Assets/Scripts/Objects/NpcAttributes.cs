using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Npc", menuName = "Gameplay/Npc")]
public class NpcAttributes : ScriptableObject
{
    #region AccessVariables


    [Header("Npc")]
    [SerializeField] private string title = "New Npc";
    [SerializeField] private Sprite icon;


    #endregion
    #region PrivateVariables


    #endregion
    #region Initlization


    private void Awake()
    {

    }


    #endregion
    #region Getters and Setters


    public string GetTitle()
    {
        return title;
    }

    public Sprite GetIcon()
    {
        return icon;
    }


    #endregion
    #region Core



    #endregion
}
