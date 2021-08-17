using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

    public void SubmitButton()
    {
        //Create a new case
        Case awsCase = new Case();
        //populate the data
        awsCase.caseID = activeCase.caseID;
        awsCase.name = activeCase.name;
        awsCase.date = activeCase.date;
        awsCase.locationNotes = activeCase.locationNotes;
        awsCase.photoTaken = activeCase.photoTaken;
        awsCase.photoNotes = activeCase.photoNotes;

        BinaryFormatter bf = new BinaryFormatter();

        //Create file
        FileStream file = File.Create(Application.persistentDataPath + "/case#" + awsCase.caseID + ".dat");

        //with file open write to file
        bf.Serialize(file, awsCase);

        //close the file
        file.Close();

        Debug.Log("Application Data path: "+ Application.persistentDataPath);
    }


    
}
