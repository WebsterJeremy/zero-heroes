using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    #region AccessVariables


    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f; //Unsure if health is required


    #endregion
    #region PrivateVariables


    private float health = 100f;


    #endregion
    #region Initlization


    protected override void Start()
    {
        base.Start();

        health = maxHealth;
    }


    #endregion
    #region Getters & Setters


    public float GetHealth()
    {
        return health;
    }


    #endregion
    #region Main


    public void OnDamaged(float dmg, GameObject inflictor)
    {
        if (dmg < 0) return;

        this.health = Mathf.Clamp(this.health - dmg, 0, maxHealth);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(
            Mathf.Lerp(0, Input.GetAxis("Horizontal") * GetSpeed(), 0.8f),
            Mathf.Lerp(0, Input.GetAxis("Vertical") * GetSpeed(), 0.8f));
    }


    #endregion
}
