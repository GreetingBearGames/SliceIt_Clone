using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceUISpawner : MonoBehaviour
{
    [SerializeField] Vector3 _sliceUIOffset;
    [SerializeField] GameObject _gameScreenCanvas;
    [SerializeField] GameObject _sliceUI;



    public void SpawnUIText(Vector3 spawningPosition)
    {
        GameObject createdSliceUI = Instantiate(_sliceUI, this.transform);
        RectTransform createdRect = createdSliceUI.GetComponent<RectTransform>();
        createdRect.anchoredPosition = GetUIScreenPosition(spawningPosition, Camera.main, _gameScreenCanvas);
        Destroy(createdSliceUI, createdSliceUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 5);
        //animasyon bittiği an yok etme
    }

    private Vector2 GetUIScreenPosition(Vector3 obj3dPosition, Camera cam3d, GameObject canvas)
    {
        Vector2 anchoredPos;
        Vector2 screenPos = cam3d.WorldToScreenPoint(obj3dPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(),
                                                                screenPos, Camera.main, out anchoredPos);
        return anchoredPos;
    }
}
