using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotoPanel : MonoBehaviour, IPanel
{
    public Text caseNumber;
    public RawImage photoTaken;
    public InputField photoNotes;

    void Start()
    {
        caseNumber.text = "CASE NUMBER:" + UIManager.Instance.activeCase.caseID;
    }

   public void ProcessInfo()
   {
        if (!string.IsNullOrEmpty(photoNotes.text))
        {
            UIManager.Instance.activeCase.photoNotes = photoNotes.text;
            UIManager.Instance.activeCase.photoTaken = photoTaken;
        }
   }

   public void TakePictureButton()
   {
       TakePicture(512);
   }

   private void TakePicture( int maxSize )
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );

                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                photoTaken.texture = texture;
                photoTaken.gameObject.SetActive(true);
                GameObject.Find("Image_Take_Photo").SetActive(false);
        
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }


}
