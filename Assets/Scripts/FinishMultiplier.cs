using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishMultiplier : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int multiplier = 1;
    [SerializeField] FinishCoinAmount finishCoinAmount;


    private void OnTriggerEnter(Collider other)
    {
        foreach (var collider in this.transform.parent.GetComponentsInChildren<BoxCollider>())
        {
            collider.enabled = false;
        }
        //Bıçağı saplama eklenecek.
        if (this.gameObject.tag != "Empty")
        {
            this.GetComponent<Animator>().enabled = true;
            text = this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            string str = text.text.ToString();
            str = str.Remove(str.Length - 1);
            Debug.Log(str);
            multiplier = int.Parse(str);
            finishCoinAmount.GetFinishMultiplier(multiplier);
        }
        GameManager.Instance.FinishLevel();
        GameManager.Instance.UpdateMoney(GameManager.Instance.Money * multiplier);
        this.transform.parent.GetChild(0).GetComponent<ParticleSystem>().Play();
        SoundManager.instance.Play("WinLevel");
        this.transform.parent.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(FinishUI());
    }
    IEnumerator FinishUI()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.FinishLevel();
    }
}
