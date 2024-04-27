using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;
    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / Screen.height;
        if(ratio>2.1f){
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;
            leftBottom.x = 0;
            rightTop.x = 0;
            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
    }
    public virtual void Setup()
    {
        // Override this method to setup the UI
    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDrirectly), time);
    }
    public virtual void CloseDrirectly()
    {
        gameObject.SetActive(false);
    }
}
