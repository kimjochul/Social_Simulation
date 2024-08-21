using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[System.Serializable]
public struct PostInfo
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

[System.Serializable]
public struct PostInfoArray
{
    public List<PostInfo> data;
}

public class HttpInfo
{
    public string url = "";

    //body������
    public string body = "";

    //contenttype
    public string contentType = "";

    //for formdata
    public WWWForm form;

    //��� ���� �� ȣ��Ǵ� �Լ� ���� ���� (delegate ����������)
    public Action<DownloadHandler> onComplete;

}
public class HttpMgr : MonoBehaviour
{
    static HttpMgr instance;

    public static HttpMgr GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = "HttpManager";
            go.AddComponent<HttpMgr>();
            //�ٷ� awakeȣ��
        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // GET : �������� �����͸� ��ȸ ��û 
    public IEnumerator Get(HttpInfo info)
    {
        //string url = "https://jsonplaceholder.typicode.com/posts/";

        //"https://ssl.pstatic.net/melona/libs/1506/1506331/b8145c5a724d3f2c9d2b_20240813152032478.jpg";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(info.url))
        {
            //������ ��û ������ 
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }

    }

    //�������� ���� ������ �����͸� �������� ��û
    public IEnumerator Post(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            //������ ��û ������ 
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }
    }

    //���� ���ε� (form-data)
    public IEnumerator UploadFileByFormData(HttpInfo info)
    {
        //info.data���� ������ ��ġ
        //info.data�� �ִ� ������ byte�迭�� �о����
        byte[] data = File.ReadAllBytes(info.body);

        //data�� MultipartForm���� ����
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, "Record.WAV", info.contentType));
        //info.form.AddBinaryData("file", File.ReadAllBytes("C:\\Users\\Admin\\Music\\Record.WAV"), "Record.WAV", "audio/wav");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            //������ ��û ������ 
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }
    }

    //���� ���ε�
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        //info.data���� ������ ��ġ
        //info.data�� �ִ� ������ byte�迭�� �о����
        byte[] data = File.ReadAllBytes(info.body);


        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            // ���ε��ϴ� ������
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            //���� �޴� ������ ����
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            //������ ��û ������ 
            yield return webRequest.SendWebRequest();

            //�������� ���� �Դ�
            DoneRequest(webRequest, info);

        }
    }

    public IEnumerator DownloadSprite(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(info.url))
        {
            yield return webRequest.SendWebRequest();
            DoneRequest(webRequest, info);
        }
    }

    public IEnumerator DownloadAudio(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(info.url, AudioType.WAV))
        {
            yield return webRequest.SendWebRequest();
            {
                //DownloadHandlerAudioClip handler = webRequest.downloadHandler as DownloadHandlerAudioClip;
                //handler.audioClip �� Audiosource�� �����ϰ� �÷��� 
            }
            DoneRequest(webRequest, info);
        }
    }
    void DoneRequest(UnityWebRequest webRequest, HttpInfo info)
    {
        //�������� ������ �Դ� 
        //���࿡ ����� �����̶��
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            //�츮 ���ϴ� �����͸� ó�� 
            //print(webRequest.downloadHandler.text);
            //File.WriteAllBytes(Application.dataPath + "/image.jpg", webRequest.downloadHandler.data);
            //-> ����� �����͸� ��û�� Ŭ������ ������ 
            if (info.onComplete != null)
            {
                info.onComplete(webRequest.downloadHandler);
            }
        }
        //�׷��� �ʴٸ� (Error���)
        else
        {
            //Error ������ ���
            Debug.LogError("Net Error: " + webRequest.error);
        }
    }
}
