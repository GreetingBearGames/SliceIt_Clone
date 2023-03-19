using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelInfoText;

    void Start()
    {
        var levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        _levelInfoText.text = "LEVEL " + levelIndex.ToString();
    }

    private void StartGame()
    {
        //GamaManagerda level start verilecek.
    }

}
