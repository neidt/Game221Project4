using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public LevelData menuLevelData = new LevelData();
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplayHighScores()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

        menuLevelData = LevelData.LoadFromFile("High Scores");

    }
    private void OnGUI()
    {
        if (SceneManager.GetActiveScene().name == "HighScores")
        {
            for (int i = 0; i < menuLevelData.scoreData.Count; i++)
            {
                GUI.Label(new Rect(380, 50 + i * 10, 100, 20), menuLevelData.scoreData[i].ToString());
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
