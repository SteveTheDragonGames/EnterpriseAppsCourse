using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get{
            if(_instance == null)
            {
                Debug.LogError("UIManager is NULL!");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }
    
    public Case activeCase;

    public void CreateNewCase()
    {
        activeCase = new Case();
    }

    
}
