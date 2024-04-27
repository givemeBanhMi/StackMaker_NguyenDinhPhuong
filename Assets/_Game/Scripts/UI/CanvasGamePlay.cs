using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] TextMeshProUGUI txtScore;
    public override void Setup()
    {
        base.Setup();
        UpdateScore(0);
    }
    public void UpdateScore(int score)
    {
        txtScore.text = score.ToString();
    }
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<CanvasSetting>();
    }
}