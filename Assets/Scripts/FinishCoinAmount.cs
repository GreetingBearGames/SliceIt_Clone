using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishCoinAmount : MonoBehaviour
{
    void Start()
    {
        FinishCoinCalculator();
    }


    private void FinishCoinCalculator()
    {
        //levelstartscore gamemanager'dan alınacak.
        //GetComponent<TextMeshProUGUI>().text = "$ " + GameManager.instance.GetLevelStartScore().ToString();
    }
}
