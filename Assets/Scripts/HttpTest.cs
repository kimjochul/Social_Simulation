using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpTest;

public class HttpTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public PostInfoArray allPostInfo;

    // Update is called once per frame
    void Update()
    {
        //Get
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HttpInfo info = new HttpInfo();
            info.url = "http://192.168.1.77:8080/test/edit_script";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
                
            };

            
            StartCoroutine(HttpMgr.GetInstance().Get(info));
        }
        
        //이미지 다운 후 스프라이트로 변환
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HttpInfo info = new HttpInfo();
            info.url = "https://ssl.pstatic.net/melona/libs/1507/1507818/2e18c2eb034e05f42c0b_20240709174123699.jpg";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                // 다운로드 된 데이터를 Texture2D로 변환
                DownloadHandlerTexture handler = downloadHandler as DownloadHandlerTexture;
                Texture2D texture = handler.texture;

                // texture를 이용해서 Sprite로 변환
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                Image image = GameObject.Find("Image").GetComponent<Image>();
                image.sprite = sprite;
            };

            //StartCoroutine(Get(info));
            StartCoroutine(HttpMgr.GetInstance().DownloadSprite(info));
        }

        //POST
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //가상의 데이터를 만들자
            UserInfo userInfo = new UserInfo();
            userInfo.name = "메타버스";
            userInfo.age = 3;
            userInfo.height = 185.5f;

            HttpInfo info = new HttpInfo();
            info.url = "http://192.168.1.77:8080/test/edit_script";
            info.body = JsonUtility.ToJson(userInfo);
            info.contentType = "application/json";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
            };

            //StartCoroutine(Get(info));
            StartCoroutine(HttpMgr.GetInstance().Post(info));
        }

        //formdata
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HttpInfo info = new HttpInfo();
            info.url = "http://mtvs.helloworldlabs.kr:7771/api/file";
            info.contentType = "multipart/form-data";
            info.body = "C:\\Users\\Admin\\Desktop\\wallpaper.jpg";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                System.IO.File.WriteAllBytes(Application.dataPath + "/dai.jpg", downloadHandler.data);
            };
            StartCoroutine(HttpMgr.GetInstance().UploadFileByFormData(info));
        }
        //filebybyte
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HttpInfo info = new HttpInfo();
            info.url = "http://mtvs.helloworldlabs.kr:7771/api/byte";
            info.contentType = "image/jpg";
            info.body = "C:\\Users\\Admin\\Videos\\Finalfinnalfinal\\image.jpg";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                System.IO.File.WriteAllBytes(Application.dataPath + "/meh.jpg", downloadHandler.data);
            };
            StartCoroutine(HttpMgr.GetInstance().UploadFileByByte(info));
        }

        //서버에게 유저가 올린 스크립트 보내기
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ScriptTest scripttest = new ScriptTest();
            scripttest.script = "스크린샷이란 컴퓨터 모니터 또는 스마트폰 등 전자기기 화면에 보이는 그대로를 찍은 출력 사진을 의미합니다. 일반적인 사진과는 다르게 텍스트만 존재하는 이미지이거나, 이미지와 텍스트가 혼재되어 있는 경우가 많습니다. 사람들이 스크린샷을 사용하는 이유는 특정 앱을 열거나 인터넷 검색 없이도 휴대폰의 갤러리에서 쉽게 접근할 수 있는 정보를 빠르게 저장할 수 있기 때문입니다. 그러나 다양한 정보가 통일성 없이 나열되어 있어 찾기 어려운 문제점이 있습니다.";
            HttpInfo info = new HttpInfo();
            info.url = "http://172.20.10.7:8080/ppt/edit_script";
            info.body = JsonUtility.ToJson(scripttest);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
                //string jsonData = "{ \"data\" :" + downloadHandler.text + "}";
                ////jsonData 를 PostInfoArray형으로 바꾸자
                //allPostInfo = JsonUtility.FromJson<PostInfoArray>(jsonData);
            };

            
            StartCoroutine(HttpMgr.GetInstance().Post(info));
        }

        //서버에게 레코드 파일 보내기
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            HttpInfo info = new HttpInfo();
            info.url = "http://172.20.10.7:8080/ppt/compare_similarity";
            info.contentType = "multipart/form-data";
            info.body = "C:\\Users\\Admin\\Music\\Record.WAV";
          //  info.form = new WWWForm();

            //  info.form.AddBinaryData("file", File.ReadAllBytes("C:\\Users\\Admin\\Music\\Record.WAV"), "Record.WAV", "audio/wav");

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print("결과 크기 : " + downloadHandler.data.Length);
                // Save the received data as a file
                //System.IO.File.WriteAllBytes("C:\\Users\\Admin\\Desktop\\DownLoadFIle\\received.WAV", downloadHandler.data);
            };

            StartCoroutine(HttpMgr.GetInstance().UploadFileByFormData(info)); // Pass the form to the upload method

            //info.body = "C:\\Users\\Admin\\Music\\Record";
            //info.onComplete = (DownloadHandler downloadHandler) =>
            //{
            //    System.IO.File.WriteAllBytes(Application.dataPath + "/received.WAV", downloadHandler.data);
            //};
            //StartCoroutine(HttpMgr.GetInstance().UploadFileByFormData(info));
        }
    }

    [System.Serializable]
    public struct UserInfo
    {
        public string name;
        public int age;
        public float height;

    }

    [System.Serializable]
    public struct ScriptTest
    {
        public string script; 
    }
    //void OnComplete(DownloadHandler downloadHandler)
    //{

    //}
}
