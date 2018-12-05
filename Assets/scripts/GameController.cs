using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public RoomGenerator roomGen;
    public PlayerController playerControl;

    public float levelTimeLimit = 90f;

    public int totalPoints;

    private Text timerText;
    private Text pointsCount;

    // Use this for initialization
    void Start()
    {
        roomGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<RoomGenerator>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        pointsCount = GameObject.FindGameObjectWithTag("PointsCounter").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelTimeLimit > 0)
        {
            //update timer
            timerText.text = levelTimeLimit.ToString();
            levelTimeLimit -= Time.deltaTime;

            //update points
            pointsCount.text = totalPoints.ToString();
        }
        if (levelTimeLimit < 0)
        {
            //game over
            timerText.text = "0.00";
            playerControl.enabled = false;
            GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Text>().text = "Game Over. Total Points: " + totalPoints;          
        }
    }
}
