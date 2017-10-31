using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HttpUtils  {

    protected string onComplete;

    public IEnumerator getHttpJson(string url, Action<string> onComplete) {
        var request = new WWW(url);
        yield return request;
        if (request.error != null)
        {
            Debug.Log("Error:" + request.error);
        }
        else
        {
            Debug.Log("Success");
            if(onComplete != null){
                onComplete(request.data);
            }
        }

    }


    public IEnumerator sendHttpDataJson(string url, string jsonString, Action<string> onComplete) {
        var encoding = new System.Text.UTF8Encoding();
        var postHeader = new Dictionary<string, string>();
        Debug.Log(jsonString);
        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("Content-Length", jsonString.Length + "");
        var request = new WWW(url, encoding.GetBytes(jsonString), postHeader);
        yield return request;
        if (request.error != null)
        {
            Debug.Log("Error:" + request.error);
        }
        else
        {
            Debug.Log("Success");
            if(onComplete != null){
                onComplete(request.data);
            }
        }

    }

}
