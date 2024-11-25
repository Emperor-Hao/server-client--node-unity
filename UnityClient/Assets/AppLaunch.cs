﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class AppLaunch : UnitySingleton<AppLaunch>
{

    private void Start()
    {

        
        //var uri = "http://127.0.0.1:3000/version.ini";
        //var uri = "http://127.0.0.1:3000/node.jpg";
        //var uri = "http://127.0.0.1:3000/UploadImgFile";
        //StartCoroutine(GetMethodTest(uri));
        //StartCoroutine(DownloadAndSaveBinFile(uri));
        //StartCoroutine(UploadFileToServer(uri));


        //var uri = "http://127.0.0.1:3000/submitData";
        //StartCoroutine(GetSubmitInfo(uri));

        var uri = "http://127.0.0.1:3000/uploadDataResult";
        StartCoroutine(UploadMsg(uri));
    }


    /// <summary>
    /// 简易通讯测试样例
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    IEnumerator GetMethodTest(string uri)
    {
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
        request.Dispose();
        yield break;
    }


    /// <summary>
    /// 上传文件到服务器示例
    /// </summary>
    /// <returns></returns>
    IEnumerator UploadFileToServer(string uri) {
        string fileName = Application.persistentDataPath + "/unity.gif";
        byte[] fileDatas = GameUtility.SafeReadAllBytes(fileName);
        Debug.Log(fileDatas.Length);

        //使用表格形式上传数据
        // 上传 http post来传文件;
        // WWWForm formData = new WWWForm();
        // formData.AddBinaryData("data", fileDatas);
        // Debug.Log(formData.data.Length - fileDatas.Length);
        // UnityWebRequest req = UnityWebRequest.Post(url, formData);

        UnityWebRequest req = UnityWebRequest.Put(uri, fileDatas);
        yield return req.SendWebRequest();

        Debug.Log("Server return: " + req.downloadHandler.text);

        req.Dispose();
        yield break;
    }



    /// <summary>
    /// 通过url传参数到服务器示例
    /// </summary>
    /// <param name="uname"></param>
    /// <param name="uscore"></param>
    /// <returns></returns>
    IEnumerator UpdateUserDataToServerDB(string uname, int uscore) {
        string url = "http://127.0.0.1:6080/UpLoadUserData?" + "uname=" + uname + "&uscore=" + uscore;
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        Debug.Log("Server Return: " + req.downloadHandler.text);

        req.Dispose();
        yield break;
    }



    /// <summary>
    /// 下载并保存文件
    /// </summary>
    /// <returns></returns>
    IEnumerator DownloadAndSaveBinFile(string uri)
    {
        UnityWebRequest req = UnityWebRequest.Get(uri);
        yield return req.SendWebRequest();
        // req.downloadHandler.data:存放了二进制数据;
        string outputFileName = Application.persistentDataPath + "/Img/test.png";
        Debug.Log(outputFileName);
        GameUtility.SafeWriteAllBytes(outputFileName, req.downloadHandler.data);
        // end

        req.Dispose();
        yield break;
    }
    /// <summary>
    /// 查看版本信息示例
    /// </summary>
    /// <returns></returns>
    IEnumerator GetSubmitInfo(string uri) {
        string userInfo = uri+"?Id=1";
        UnityWebRequest req = UnityWebRequest.Get(userInfo);
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        var msg = req.downloadHandler.text;
        Debug.Log("Server return:" + msg);

        //req.Dispose(); // 释放掉我们的请对象;
        yield break;
    }


    IEnumerator UploadMsg(string url)
    {
        string json = "{\"name\":\"John\", \"age\":30}";
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // 发送请求
        yield return www.SendWebRequest();
        Debug.Log("Server Return: " + www.downloadHandler.text);
    }
}
