using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region stats
    public float maxHealth = 100;
    public float currentHealth;
    public float attack = 10f;
    public float attackRate = 0.05f;
    #endregion

    private PlayerController pc;
    public bool playerInRange;

    // Use this for initialization
    void Start ()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerInRange)
        {
            InvokeRepeating("AttackPlayer", 1, attackRate);
        }
        else
        {
            CancelInvoke();
        }
	}

    private void AttackPlayer()
    {
        print("enemy attacking player");
        pc.TakeDamage(attack);
    }

    public void TakeDamage(float damageValue)
    {
        Color enemyColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
        Color damageColor = Color.red;
        enemyColor = Color.Lerp(enemyColor, damageColor, Mathf.PingPong(Time.time, 1));
        currentHealth -= damageValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player is in range
        //then attack player
        if(other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
