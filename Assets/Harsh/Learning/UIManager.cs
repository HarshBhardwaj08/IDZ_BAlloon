using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
  
    public int Score;

   
   public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Score++;
            Debug.Log(Score);
        }
    }
}
