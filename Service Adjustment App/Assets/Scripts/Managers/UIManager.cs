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
    public GameObject borderPanel;
    public GameObject clientInfoPanel;
    public GameObject locationPanel;
    public GameObject TakePhotoPanel;

    public Case activeCase;

    public void CreateNewCase()
    {
        clientInfoPanel.SetActive(true);
        borderPanel.SetActive(true);
        activeCase = new Case();

        ClientInfoPanel clientPanel = clientInfoPanel.GetComponent<ClientInfoPanel>();
        int caseID = clientPanel.GenerateCase();
        activeCase.caseID = caseID.ToString();
        clientPanel.caseNumberText.text = "CASE NUMBER: " + activeCase.caseID;
    }


    
}
