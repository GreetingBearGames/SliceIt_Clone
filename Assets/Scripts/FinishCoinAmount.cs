using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishCoinAmount : MonoBehaviour
{
    private int multiplier = 1;
    void Awake()
    {
        FinishCoinCalculator();
    }


    private void FinishCoinCalculator()
    {
        Debug.Log(multiplier);

        var money = GameManager.Instance.Money - GameManager.Instance.LevelStartScore;
        var multipliedMoney = money * multiplier;

        Debug.Log(money);
        Debug.Log(multipliedMoney);

        GetComponent<TextMeshProUGUI>().text = "$ " + multipliedMoney.ToString();
    }

    public void GetFinishMultiplier(int hitMultiplier)
    {
        multiplier = hitMultiplier;
    }
}
