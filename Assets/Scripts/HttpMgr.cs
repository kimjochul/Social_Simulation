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

    //body데이터
    public string body = "";

    //contenttype
    public string contentType = "";

    //for formdata
    public WWWForm form;

    //통신 성공 후 호출되는 함수 담을 변수 (delegate 생각나야함)
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
            //바로 awake호출
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

    // GET : 서버에게 데이터를 조회 요청 
    public IEnumerator Get(HttpInfo info)
    {
        //string url = "https://jsonplaceholder.typicode.com/posts/";

        //"https://ssl.pstatic.net/melona/libs/1506/1506331/b8145c5a724d3f2c9d2b_20240813152032478.jpg";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(info.url))
        {
            //서버에 요청 보내기 
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }

    }

    //서버에게 내가 보내는 데이터를 생성해줘 요청
    public IEnumerator Post(HttpInfo info)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, info.body, info.contentType))
        {
            //서버에 요청 보내기 
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }
    }

    //파일 업로드 (form-data)
    public IEnumerator UploadFileByFormData(HttpInfo info)
    {
        //info.data에는 파일의 위치
        //info.data에 있는 파일을 byte배열로 읽어오자
        byte[] data = File.ReadAllBytes(info.body);

        //data를 MultipartForm으로 셋팅
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, "Record.WAV", info.contentType));
        //info.form.AddBinaryData("file", File.ReadAllBytes("C:\\Users\\Admin\\Music\\Record.WAV"), "Record.WAV", "audio/wav");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(info.url, formData))
        {
            //서버에 요청 보내기 
            yield return webRequest.SendWebRequest();

            DoneRequest(webRequest, info);
        }
    }

    //파일 업로드
    public IEnumerator UploadFileByByte(HttpInfo info)
    {
        //info.data에는 파일의 위치
        //info.data에 있는 파일을 byte배열로 읽어오자
        byte[] data = File.ReadAllBytes(info.body);


        using (UnityWebRequest webRequest = new UnityWebRequest(info.url, "POST"))
        {
            // 업로드하는 데이터
            webRequest.uploadHandler = new UploadHandlerRaw(data);
            webRequest.uploadHandler.contentType = info.contentType;

            //응답 받는 데이터 공간
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            //서버에 요청 보내기 
            yield return webRequest.SendWebRequest();

            //서버에게 응답 왔다
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
                //handler.audioClip 을 Audiosource에 세팅하고 플레이 
            }
            DoneRequest(webRequest, info);
        }
    }
    void DoneRequest(UnityWebRequest webRequest, HttpInfo info)
    {
        //서버에게 응답이 왔다 
        //만약에 결과가 정상이라면
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            //우리 원하는 데이터를 처리 
            //print(webRequest.downloadHandler.text);
            //File.WriteAllBytes(Application.dataPath + "/image.jpg", webRequest.downloadHandler.data);
            //-> 응답온 데이터를 요청한 클래스로 보내자 
            if (info.onComplete != null)
            {
                info.onComplete(webRequest.downloadHandler);
            }
        }
        //그렇지 않다면 (Error라면)
        else
        {
            //Error 이유를 출력
            Debug.LogError("Net Error: " + webRequest.error);
        }
    }
}
