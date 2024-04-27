using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform panrent;
    private void Awake()
    {
        UICanvas[] Prefab = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < Prefab.Length; i++)
        {
            canvasPrefabs.Add(Prefab[i].GetType(), Prefab[i]);
        }
    }
    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();
        canvas.Setup();
        canvas.Open();
        return canvas as T;
    }
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsLoad<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }
    public void CloseDirectly<T>() where T : UICanvas
    {
        if (IsLoad<T>())
        {
            canvasActives[typeof(T)].CloseDrirectly();
        }
    }
    public bool IsLoad<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null;
    }
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoad<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }
    // Lấy một UICanvas kiểu T
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoad<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, transform);
            canvasActives.Add(typeof(T), canvas);
        }
        return canvasActives[typeof(T)] as T;
    }
    private T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }
    public void CloseAll()
    {
        foreach (var canvas in canvasActives)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
