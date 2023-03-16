using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;

    void Start()
    {

    }


    void Update()
    {

    }

    public void GameScore(int knifeScore)
    {
        score += knifeScore;
        scoreText.text = score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //next level will be added
    }
    

}
