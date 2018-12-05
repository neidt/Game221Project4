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
    public int healthPickupPointValue = 1;

    public float healthIncreaseValue = 5f;
    public int healthIncreasePointValue = 5;

    public float attackIncreaseValue = 1f;
    public int attackIncreasePointValue = 5;

    public int enemyPointValue = 10;

    public Slider healthSlider;
    public Text attackValueTextbox;
    public GameController gcontrol;
    #endregion

    private Color originalPlayerColor;
    private EnemyHealth enemyHealthScript;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        eyeMount = transform.Find("EyeMount");
        gcontrol = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        originalPlayerColor = this.gameObject.GetComponent<MeshRenderer>().material.color;

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
            float camRotation = Input.GetAxis("Mouse Y") * Time.deltaTime;
            float yAxis = Input.GetAxis("Mouse Y");
            float eulerAngleLimit = eyeMount.transform.eulerAngles.x;
            float maxView = 70f;
            float minView = -20f;

            if(eulerAngleLimit > 180)
            {
                eulerAngleLimit -= 360;
            }
            else if(eulerAngleLimit < -180)
            {
                eulerAngleLimit += 360;
            }
            float targetRotation = eulerAngleLimit + yAxis * -rotateFactor * Time.deltaTime;

            if(targetRotation < maxView && targetRotation > minView)
            {
                eyeMount.transform.eulerAngles += new Vector3(yAxis * -rotateFactor * Time.deltaTime, 0, 0);
            }

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
        Color damageColor = Color.red;
        Color playerColor = Color.Lerp(originalPlayerColor, damageColor, Mathf.PingPong(Time.time, 1));
        this.gameObject.GetComponent<MeshRenderer>().material.color = playerColor;
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
            gcontrol.totalPoints += healthPickupPointValue;
        }

        //if its a health increase,
        //max health increases by that amount
        if (other.tag == "HealthIncrease")
        {
            maxHealth += healthIncreaseValue;
            Destroy(other.gameObject);
            gcontrol.totalPoints += healthIncreasePointValue;
        }

        //if its an attack increase, 
        //increases attack( therfore damage)
        if (other.tag == "AttackIncrease")
        {
            currentAttack += attackIncreaseValue;
            Destroy(other.gameObject);
            gcontrol.totalPoints += attackIncreasePointValue;
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
                Attack(other.gameObject);

                //if enemy health hits 0
                //destroy it and add points to player's points
                if (enemyHealthScript.currentHealth <= 0)
                {
                    Destroy(other.gameObject);
                    gcontrol.totalPoints += enemyPointValue;
                }
            }
        }
    }
}
