using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649

public class CharacterBase : MonoBehaviour
{
    #region AccessVariables


    [Header("Settings")]
    [SerializeField] private string displayName = "Character";
    [SerializeField] private float speed = 3f;


    #endregion
    #region PrivateVariables


    protected bool isMoving = false;
    protected Vector2 destination;

 //   protected Animator animator;


    #endregion
    #region Initlization


    protected virtual void Start()
    {
//        animator = GetComponent<Animator>();
    }


    #endregion
    #region Getters & Setters


    public float GetSpeed()
    {
        return speed;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Vector2 GetDestination()
    {
        return destination;
    }

    #endregion
    #region Main
       


    #endregion
    #region AI


    public void MoveTo(Vector2 pos)
    {
        Debug.Log("Walking to " + pos);
        destination = pos;
    }

    public void MoveTo(float x, float y)
    {
        MoveTo(new Vector2(x, y));
    }


    #endregion
}
