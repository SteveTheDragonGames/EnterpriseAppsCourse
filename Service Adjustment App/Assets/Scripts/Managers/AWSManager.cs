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
    public string AWSCognitoCredentials;
    public string S3Region = RegionEndpoint.USEast2.SystemName;
        private RegionEndpoint _S3Region
        {
            get { return RegionEndpoint.GetBySystemName(S3Region); }
        }

    
    private void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        CognitoAWSCredentials credentials = new CognitoAWSCredentials (
        AWSCognitoCredentials, // Identity Pool ID
        RegionEndpoint.USEast2 // Region
        );

        AmazonS3Client S3Client = new AmazonS3Client (credentials, _S3Region);


        
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

    
  
}
