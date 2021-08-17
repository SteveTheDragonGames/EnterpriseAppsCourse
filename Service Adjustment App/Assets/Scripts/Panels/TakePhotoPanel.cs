using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotoPanel : MonoBehaviour, IPanel
{
    public Text caseNumber;
    public RawImage photoTaken;
    public InputField photoNotes;

    private string imgPath;

    void Start()
    {
        caseNumber.text = "CASE NUMBER:" + UIManager.Instance.activeCase.caseID;
    }

   public void ProcessInfo()
   {


        if (!string.IsNullOrEmpty(photoNotes.text))
        {
            byte[] imgData = null;

            if(string.IsNullOrEmpty(imgPath)==false)
            {
                //Create a 2d texture
                //apply the texture from the image path
                //encode to PNG
                //store bytes to phototaken (activeCase)

                Texture2D img = NativeCamera.LoadImageAtPath(imgPath, 512, false);
                imgData = img.EncodeToPNG();
            }
          

            UIManager.Instance.activeCase.photoNotes = photoNotes.text;
            UIManager.Instance.activeCase.photoTaken = imgData;         
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
                Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize, false );

                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                photoTaken.texture = texture;
                photoTaken.gameObject.SetActive(true);
                GameObject.Find("Image_Take_Photo").SetActive(false);
                imgPath = path;
        
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }


}
