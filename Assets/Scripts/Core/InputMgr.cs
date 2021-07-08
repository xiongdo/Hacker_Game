using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : Singleton<InputMgr>
{
    private void Update()
    {
        var mouseCellPos = Utility.GetMouseCellPosition();
        Debug.Log("the mouse pos is " + mouseCellPos.x + ", " + mouseCellPos.y);
    }
}
