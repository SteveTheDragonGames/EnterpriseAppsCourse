using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LocationPanel : MonoBehaviour, IPanel
{

    public Text caseNumber;
    public RawImage map;
    public InputField notes;

    public string apiKey;

    public float xCoord;// from mobile device
    public float yCoord;//from mobile device
    public int zoom; // set in inspector
    public int imgSize;//set in inspector
    public string url = "https://maps.googleapis.com/maps/api/staticmap?";


    public void OnEnable()
    {
        //construct appropriate url
        url += "center=" + xCoord + ","+ yCoord + "&zoom=" + zoom + "&size=" + imgSize+"x"+ imgSize + "&key=" +apiKey; 
        //Download static map
        //apply the map to raw image
        //https://maps.googleapis.com/maps/api/staticmap?center=40.714728,-73.998672&zoom=12&size=400x400&key=YOUR_API_KEY

        

        caseNumber.text = "CASE NUMBER:" + UIManager.Instance.activeCase.caseID;

        StartCoroutine(DownloadMap());


    }

    public void ProcessInfo()
    {

    }

    IEnumerator DownloadMap()
    {
        using(UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Successfully downloaded image");
                var texture = DownloadHandlerTexture.GetContent(request);
                map.texture = texture;
            }
        }
    }
}
