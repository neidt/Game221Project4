using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region stats
    public float maxHealth = 100;
    public float currentHealth;
    public float attack = 10f;
    public float attackRate = 1f;
    #endregion

    private PlayerController pc;
    public bool playerInRange;
    private Color originalEnemyColor;
    // Use this for initialization
    void Start ()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentHealth = maxHealth;
        originalEnemyColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playerInRange)
        {
            
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = originalEnemyColor;
        }
	}

    private void AttackPlayer()
    {
        print("enemy attacking player");
        pc.TakeDamage(attack);
    }

    public void TakeDamage(float damageValue)
    {
       
        Color damageColor = Color.red;
        Color enemyColor = Color.Lerp(originalEnemyColor, damageColor, Mathf.PingPong(Time.time, 1));
        this.gameObject.GetComponent<MeshRenderer>().material.color = enemyColor;

        currentHealth -= damageValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player is in range
        //then attack player
        if(other.tag == "Player")
        {
            playerInRange = true;
            InvokeRepeating("AttackPlayer", 1, attackRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            CancelInvoke();
        }
    }
}
