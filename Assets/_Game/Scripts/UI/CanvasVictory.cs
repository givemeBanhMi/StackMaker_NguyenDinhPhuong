using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasVictory : UICanvas
{
    [SerializeField] TextMeshProUGUI txtScore;
    public void SetBestScore(int score)
    {
        txtScore.text = score.ToString();
    }
   public void NextLevelButton()
   {
         //GameManager.Instance.NextLevel();
   }
    public void RestartButton()
    {
            //GameManager.Instance.RestartLevel();
    }
    
    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }  
}