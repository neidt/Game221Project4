using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public RoomGenerator roomGen;
    public PlayerController playerControl;
    public MainMenuController menuControl;
    

    public float levelTimeLimit = 90f;

    public int totalPoints;

    private Text timerText;
    private Text pointsCount;

    public LevelData levelData;

    // Use this for initialization
    void Start()
    {
        roomGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<RoomGenerator>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        pointsCount = GameObject.FindGameObjectWithTag("PointsCounter").GetComponent<Text>();
        menuControl = GameObject.Find("UIControl").GetComponent<MainMenuController>();
        levelData = menuControl.menuLevelData;
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
            
            GameObject.FindGameObjectWithTag("GameOverText").GetComponent<Text>().text = "Game Over. Total Points: " + totalPoints + "\nPress Escape to Quit";
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SaveScores();
                Destroy(menuControl.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }

    private void SaveScores()
    {
        levelData.scoreData.Add(totalPoints);
        levelData.SaveToFile("High Scores");
    }
}

//other classes


[Serializable]
public class LevelData
{
    public List<int> scoreData = new List<int>();
    private static string saveDir = Directory.GetCurrentDirectory() + "\\HighScores\\";

    public static LevelData LoadFromFile(string fileName)
    {
        string newPath = System.IO.Path.Combine(saveDir, fileName);
        return JsonUtility.FromJson<LevelData>(System.IO.File.ReadAllText(newPath));
    }

    public void SaveToFile(string fileName)
    {
        string newPath = System.IO.Path.Combine(saveDir, fileName);
        System.IO.File.WriteAllText(newPath, JsonUtility.ToJson(this, true));
    }
}
