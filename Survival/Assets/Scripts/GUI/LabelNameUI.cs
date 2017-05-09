using UnityEngine;
using UnityEngine.UI;

public class LabelNameUI : MonoBehaviour
{
    public Transform followObject;

    private RectTransform CanvasRect;
    private RectTransform rt;
    void Awake()
    {
        CanvasRect = GetComponentInParent<RectTransform>();
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {

        if (followObject != null)
        {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            rt.anchoredPosition = WorldObject_ScreenPosition;

        }
    }
}
