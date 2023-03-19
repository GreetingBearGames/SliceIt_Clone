using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelNumberText : MonoBehaviour
{
    void Start()
    {
        var levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        GetComponent<TextMeshProUGUI>().text = "LEVEL " + levelIndex.ToString();
    }
}
