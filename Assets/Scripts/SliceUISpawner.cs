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
        GameObject createdSliceUI = Instantiate(_sliceUI);
        RectTransform createdRect = createdSliceUI.GetComponent<RectTransform>();
        createdSliceUI.transform.SetParent(this.transform);
        createdRect.anchoredPosition = GetUIScreenPosition(spawningPosition, Camera.main,
                                                            createdRect.anchorMin, _gameScreenCanvas);
        Destroy(createdSliceUI, createdSliceUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        //animasyon bittiği an yok etme
    }

    private Vector2 GetUIScreenPosition(Vector3 obj3dPosition, Camera cam3d, Vector2 anchor, GameObject canvas)
    {
        Vector2 rootScreen = canvas.GetComponent<RectTransform>().sizeDelta;
        Vector3 screenPos = cam3d.WorldToViewportPoint(obj3dPosition);
        return (rootScreen * screenPos) - (rootScreen * anchor);
    }
}
