using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CanvasSetting : UICanvas
{
   public void CloseSetting()
   {
      UIManager.Instance.CloseDirectly<CanvasSetting>();
   }
}
