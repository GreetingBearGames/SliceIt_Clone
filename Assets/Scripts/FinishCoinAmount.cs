using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishCoinAmount : MonoBehaviour
{
    void Awake()
    {
        FinishCoinCalculator();
    }


    private void FinishCoinCalculator()
    {
        GetComponent<TextMeshProUGUI>().text = "$ " + GameManager.Instance.LevelStartScore.ToString();
    }
}
