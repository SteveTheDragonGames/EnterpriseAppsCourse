using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Util;
using Amazon.S3.Model;


public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if(_instance == null) Debug.LogError("AWSManager is NULL"); 

            return _instance;
        }
    }

    private string _AWSCognitoCredentials;
    
    public string S3Region = RegionEndpoint.USEast2.SystemName;
    
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if(_s3Client == null)
            {
                _s3Client = 
                new AmazonS3Client(new CognitoAWSCredentials(_AWSCognitoCredentials, RegionEndpoint.USEast2), _S3Region);
            }

            return _s3Client;
        }
    }

    
    private string _bucketName = "std-service-app-case-files";
    
    private void Awake()
    {
        _instance = this;

        _AWSCognitoCredentials = Keys.KEY_AWS_COGNITO_CREDENTIALS;
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        
        /*
        S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            
            if (responseObject.Exception == null)
            {
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    Debug.Log("Bucket name: " + s3b.BucketName);
                });
            }
            else
            {
                Debug.Log ("AWS error" + responseObject.Exception);
            }
        });

        */
    }

    public void UploadToS3(string path, string caseID)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = _bucketName,
            Key = "case#"+caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(request, (responseObj) => 
        {
            if(responseObj.Exception == null)
            {
                Debug.Log("Successfully posted to bucket");
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("Exception occured during uploading" + responseObj.Exception);
            }
        });

    }

    public void GetList(string caseNumber, Action onComplete = null)
    {

        string target = "case#" + caseNumber;

        Debug.Log("AWSManager :: GetList()");
        var request = new ListObjectsRequest()
        {
            BucketName = _bucketName
        };

        S3Client.ListObjectsAsync(request, (responseObject) => 
        {
            if(responseObject.Exception == null)
            {
                bool caseFound = responseObject.Response.S3Objects.Any(obj => obj.Key == target);

                if (caseFound == true)
                {
                    Debug.Log("Found the case!");
                    S3Client.GetObjectAsync(_bucketName, target, (responseObject) => 
                    {                         
                        //read the data and apply it to a casefile (object) to be used.
                        //i.e. if we successfully download the file, it will be placed in responseObject.
                        
                        //check if response stream is empty or null.
                        if (responseObject.Response.ResponseStream != null)
                        {
                            //byte array to store data from file
                            byte[] data  = null;
                            //this will be populated in the memory stream.

                            //create a stream reader to read the response data.
                            //we use a using so stream is disposed when done using it.
                            using(StreamReader reader = new StreamReader(responseObject.Response.ResponseStream))
                            {
                                //access memory stream
                                //we open a new stream but a memory stream
                                //this just gives us -access- to the memory of what we downloaded.
                                //to actually recreate our casefile object, we'll need to use
                                //memory.write() to populate a databyte array with the memorystream data.
                                //memory.write() also needs buffer, offset, and count.

                                using(MemoryStream memory = new MemoryStream())
                                {
                                    var buffer = new byte[512];
                                    var bytesRead = default(int); // parsing byte.
                                    //create a while loop to parse through bytes read.
                                    while((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length))>0)
                                    {
                                        //while there's bytes to be read and the file length > 0, we'll write all the bytes we're reading.
                                        memory.Write(buffer,0,bytesRead);
                                    } 
                                    //place the memory into the byte array called data we created earlier.
                                    data = memory.ToArray();
                                }
                            }

                            //Convert those bytes to a case (Object)
                            //we'll use yet another memory stream

                            using (MemoryStream memory = new MemoryStream(data))
                            {
                                //we're going to need a binary formatter to reconstruct this stream.
                                //be sure to include the System.Runtime.Serialization.Formatters.Binary namespace

                                BinaryFormatter _binaryFormatter = new BinaryFormatter();
                                Case downloadedCase = (Case)_binaryFormatter.Deserialize(memory);
                                Debug.Log("Downloaded Case Name: " + downloadedCase.name);
                                UIManager.Instance.activeCase = downloadedCase;
                                
                                if(onComplete != null)
                                {
                                    onComplete();
                                }
                                    

                            }
                        }
                    });
                }
                else
                {
                    Debug.Log("Case not found.");
                }
            }
            else
            {
                Debug.LogError("Error getting list of items off s3" + responseObject.Exception);
            }
        });
    }

    
  
}
