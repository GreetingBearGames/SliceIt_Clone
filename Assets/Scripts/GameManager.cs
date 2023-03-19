using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //[SerializeField] private Canvas gameOverCanvas;
    private float _money;
    private static GameManager _instance;   //Create instance and make it static to be sure that only one instance exist in scene.

    public static GameManager Instance {     //To access GameManager, we use GameManager.Instance
        get {
            if (_instance == null) {
                Debug.LogError("Game Manager is null");
            }
            return _instance;
        }
    }
    public float Money {       //Money property. You can get money from outside this script, but you can only set in this script.
        get => _money;
        private set => _money = value;
    }

    private void Awake() {
        _instance = this;
    }

    public void UpdateMoney(float updateAmount) {     //To update money.Use positive value to increment. Use negative value to decrement.
        _money += updateAmount;
        if (_money < 0) {                           //To make sure that money is above 0.
            _money = 0;
        }
    }


    public void NextLevel() {

    }
    public void LoseLevel() {               //Call this when player can't finish the game successfully.
        //gameOverCanvas.gameObject.SetActive(true);
    }
    public void RetryLevel() {

    }
}
