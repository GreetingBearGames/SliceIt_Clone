using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliceScore : MonoBehaviour
{
    private int _scoreAmount;
    private int _levelStartScore;


    void Start()
    {
        _scoreAmount = PlayerPrefs.GetInt("Score", 0);
        GetComponent<TextMeshProUGUI>().text = _scoreAmount.ToString();

        _levelStartScore = _scoreAmount;
    }


    public void IncreaseScore()
    {
        _scoreAmount++;
        PlayerPrefs.SetInt("Score", _scoreAmount);
        GetComponent<TextMeshProUGUI>().text = _scoreAmount.ToString();
    }
}
