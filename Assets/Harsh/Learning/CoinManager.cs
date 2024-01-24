using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : UIManager
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Score = Score + 10;
            Debug.Log(Score);
        }
           
    }
}
