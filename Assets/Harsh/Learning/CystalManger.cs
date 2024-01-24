using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CystalManger : UIManager
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Score = Score + 3;
            Debug.Log(Score);
        }
           
    }
}
