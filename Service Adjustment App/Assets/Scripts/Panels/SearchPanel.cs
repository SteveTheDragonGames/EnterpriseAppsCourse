using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, IPanel
{

    public InputField caseNumberInput;

    public SelectPanel selectPanel;

    public void ProcessInfo()
    {
        //download list of all objects inside of the service-app-casefiles bucket.

        AWSManager.Instance.GetList(caseNumberInput.text, ()=>{
            selectPanel.gameObject.SetActive(true);
        });

        
        
        //compare those to the caseNumberInput
        //if we find a match, download the object

    }
}
