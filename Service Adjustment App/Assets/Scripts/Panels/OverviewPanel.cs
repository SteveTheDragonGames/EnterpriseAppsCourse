using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OverviewPanel : MonoBehaviour, IPanel
{

    public Text caseNumber, nameTitle, dateTitle, locationTitle, locationNotes, photoNotes;
    public RawImage photoTaken;

   

    

    void Start()
    {   

        caseNumber.text     = "CASE NUMBER: "   + UIManager.Instance.activeCase.caseID;
        nameTitle.text      = "NAME: "          + UIManager.Instance.activeCase.name;
        UIManager.Instance.activeCase.date = DateTime.Today.ToString();
        dateTitle.text      = "DATE: "          + UIManager.Instance.activeCase.date;
        locationTitle.text  = "LOCATION: \n"    + UIManager.Instance.activeCase.location;
        locationNotes.text  = "NOTES: \n"       + UIManager.Instance.activeCase.locationNotes;

        //rebuild the photo and display it:
        //convert bytes to png
        //convert texture2D to texture.

        Texture2D reconstructedImage = new Texture2D(1,1);
        //size doesn't matter, going to be replaced immediately.
        reconstructedImage.LoadImage(UIManager.Instance.activeCase.photoTaken);
        photoTaken.texture = (Texture)reconstructedImage;
        
        photoNotes.text     = "PHOTO NOTES: \n" + UIManager.Instance.activeCase.photoNotes;
    }

   public void ProcessInfo()
   {
       UIManager.Instance.SubmitButton();
   }
}
