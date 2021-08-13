using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public InputField firstName, lastName;

   public void ProcessInfo()
   {
       //check if first name and last name aren't empty
       if(firstName.text != "" && lastName.text != "")
       {
           UIManager.Instance.activeCase.name = firstName.text + " " + lastName.text;
           UIManager.Instance.locationPanel.SetActive(true);
           UIManager.Instance.clientInfoPanel.SetActive(false);
       }

   }

   public int GenerateCase()
   {
       int caseID = Random.Range(1,1000);
       return caseID;       
   }
}
