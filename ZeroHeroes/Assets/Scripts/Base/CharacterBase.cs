using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterBase : MonoBehaviour
{
    #region AccessVariables


    [Header("Movement")]
    [SerializeField] private float speed = 1f;


    #endregion
    #region PrivateVariables


    protected bool isMoving = false;
    protected Vector2 destination;

    protected Animator animator;
    protected Rigidbody2D rigidbody;


    #endregion
    #region Initlization


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
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
