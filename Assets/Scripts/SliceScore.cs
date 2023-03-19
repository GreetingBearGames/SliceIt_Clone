using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliceScore : MonoBehaviour
{
    private int _scoreAmount;


    void Start()
    {
        _scoreAmount = GameManager.Instance.Money;
        GetComponent<TextMeshProUGUI>().text = _scoreAmount.ToString();

        GameManager.Instance.LevelStartScore = _scoreAmount;
    }


    public void IncreaseScore()
    {
        GameManager.Instance.UpdateMoney(1);
        GetComponent<TextMeshProUGUI>().text = GameManager.Instance.Money.ToString();
    }
}
