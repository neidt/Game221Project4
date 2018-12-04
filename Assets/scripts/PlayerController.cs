using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement stuff
    public float speed;
    public float runMultiplier;
    private bool isRunning = false;
    private float MAXSPEED;
    private float BASESPEED;
    public float rotateFactor = 500.0f;
    public float pitchFactor = 500.0f;

    private Vector3 playerVec;
    private Vector3 targetVec;

    private Transform eyeMount;
    private CharacterController characterController;
    //private Camera mainCam;
    #endregion

    #region playerStats
    //player values
    public float maxHealth = 1000f;
    public float currentHealth;
    public float attack = 20f;
    public float currentAttack;
    public float playerAttackRate = .2f;

    //pickups
    public float healthPickupValue = 10f;
    public float healthIncreaseValue = 5f;
    public float attackIncreaseValue = 1f;

    public Slider healthSlider;
    public Text attackValueTextbox;
    #endregion

    private EnemyHealth enemyHealthScript;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        eyeMount = transform.Find("EyeMount");

        speed = 10;

        #region stats
        currentHealth = maxHealth;
        currentAttack = attack;
        #endregion
    }

    void Update()
    {
        #region movement
        //movement
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += -transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;
        }

        transform.Rotate(Vector3.up, rotateFactor * (Input.GetAxis("Mouse X") * Time.deltaTime));
        if (eyeMount != null)
        {
            eyeMount.Rotate(Vector3.right, -rotateFactor * (Input.GetAxis("Mouse Y") * Time.deltaTime));
        }

        characterController.SimpleMove(moveDirection.normalized * speed);
        #endregion

        #region stat updates
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        attackValueTextbox.text = currentAttack.ToString();
        #endregion
    }

    private void Attack(GameObject enemy)
    {
        enemy.GetComponent<EnemyHealth>().TakeDamage(attack);
    }

    public void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;
        Color playerColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
        Color damageColor = Color.red;
        playerColor = Color.Lerp(damageColor, playerColor, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if hitting a health pickup, 
        //destroy it and add to max health
        if (other.tag == "HealthPickup")
        {
            currentHealth += healthPickupValue;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            Destroy(other.gameObject);
        }

        //if its a health increase,
        //max health increases by that amount
        if (other.tag == "HealthIncrease")
        {
            maxHealth += healthIncreaseValue;
            Destroy(other.gameObject);
        }

        //if its an attack increase, 
        //increases attack( therfore damage)
        if (other.tag == "AttackIncrease")
        {
            currentAttack += attackIncreaseValue;
            Destroy(other.gameObject);
        }

        //if its an enemy, 
        //disable movment and do combat
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.LogWarning("player is hitting enemy");
            enemyHealthScript = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealthScript.currentHealth > 0)
            {
                //this.enabled = false;

                Attack(other.gameObject);

                //if enemy health hits 0
                //destroy it
                if (enemyHealthScript.currentHealth <= 0)
                {
                    Destroy(other.gameObject);
                }
            }

        }
    }
}
