using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryDataHolder : MonoBehaviour
{
    public static CategoryDataHolder Instance;
    public enum AvailableCategories { PhysicsDraw,Quiggle,jellyCar,SoftBody,MarbleGame,Coloring3D,FoodMonster,Ragdoll};
    public AvailableCategories ActiveCategory;
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
