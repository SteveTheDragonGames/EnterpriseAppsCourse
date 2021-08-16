using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Android;


public class LocationPanel : MonoBehaviour, IPanel
{

    public Text caseNumber;
    public RawImage map;
    public InputField mapNotes;

    public string apiKey;

    public float xCoord;// from mobile device
    public float yCoord;//from mobile device
    public int zoom; // set in inspector
    public int imgSize;//set in inspector
    public string url = "https://maps.googleapis.com/maps/api/staticmap?";


    public IEnumerator Start()
    {
        caseNumber.text = "CASE NUMBER:" + UIManager.Instance.activeCase.caseID;

        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        //grab GEO Data            
        if(Input.location.isEnabledByUser == true)
        {
            yield return new WaitForSeconds(4f);
            // Start service before querying location
            Input.location.Start();            

            // Wait until service initializes
            int maxWait = 4;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1.0f);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break; // stops the coroutine. No point in going further.
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                xCoord = Input.location.lastData.latitude;
                yCoord = Input.location.lastData.longitude;             
            }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
        }
        else
        {
            Debug.Log("Location services are not enabled");
        }

        DownloadGoogleMaps();    
    
    } 

        
       
    private void DownloadGoogleMaps()
    {
         //construct appropriate url
        url += "center=" + xCoord + ","+ yCoord + "&zoom=" + zoom + "&size=" + imgSize+"x"+ imgSize + "&key=" +apiKey; 
        //Download static map
        //apply the map to raw image
        //https://maps.googleapis.com/maps/api/staticmap?center=40.714728,-73.998672&zoom=12&size=400x400&key=YOUR_API_KEY

        
        StartCoroutine(DownloadMap());
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(mapNotes.text))
        {
            UIManager.Instance.activeCase.locationNotes = mapNotes.text;
        }
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
