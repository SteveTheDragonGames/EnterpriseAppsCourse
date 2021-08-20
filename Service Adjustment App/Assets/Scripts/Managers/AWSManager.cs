using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if(_instance == null)
            {  
               Debug.LogError("AWSManager is NULL"); 
            }
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

    
    private void Awake()
    {
        _instance = this;

        _AWSCognitoCredentials = Keys.KEY_AWS_COGNITO_CREDENTIALS;
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        
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


    }

    public void UploadToS3(string path, string caseID)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "std-service-app-case-files",
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
            }
            else
            {
                Debug.Log("Exception occured during uploading" + responseObj.Exception);
            }
        });

    }

    
  
}
