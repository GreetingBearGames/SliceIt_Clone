using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SliceCombiner : MonoBehaviour
{
    [SerializeField] private int _counter;
    [SerializeField] private TextMeshProUGUI _successTextObj;
    private float _lastSliceTime = 0;
    private float _combineDuration = 0.5f;


    public void CounterIncrease()
    {
        var newSliceTime = Time.time;

        //eğer yeni slice anı, önceki slice anından kısa bir süre sonra gerçekleşirse
        if (newSliceTime - _lastSliceTime < _combineDuration)
        {
            _counter++;
            _lastSliceTime = newSliceTime;
            SliceMultiplierTextDecision(_counter);
        }
        else    //eğer yeni slice anı, önceki slice anından uzun bir bir süre sonra gerçekleşirse
        {
            _counter = 1;
            _lastSliceTime = newSliceTime;
        }
    }


    private void SliceMultiplierTextDecision(int sliceCount)
    {
        if (sliceCount >= 3 && sliceCount < 5) StartCoroutine(CombineTextTimer("GOOD"));
        else if (sliceCount < 8) StartCoroutine(CombineTextTimer("GREAT"));
        else if (sliceCount < 10) StartCoroutine(CombineTextTimer("AWESOME"));
        else if (sliceCount < 15) StartCoroutine(CombineTextTimer("FANTASTIC"));
        else StartCoroutine(CombineTextTimer("PERFECT"));
    }


    private IEnumerator CombineTextTimer(string successText)
    {
        var lastTimerValue = _lastSliceTime;
        //yeni slice gelecek mi diye bir süre bekliyoruz.
        yield return new WaitForSeconds(_combineDuration + 0.2f);

        if (_lastSliceTime == lastTimerValue)       //yeni slice gelmedi. son slice anı hala aynı.
        {
            _successTextObj.text = successText;

            yield return new WaitForSeconds(1);
            _successTextObj.text = string.Empty;
        }
    }
}
