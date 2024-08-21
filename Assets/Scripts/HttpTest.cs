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
        
        //�̹��� �ٿ� �� ��������Ʈ�� ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HttpInfo info = new HttpInfo();
            info.url = "https://ssl.pstatic.net/melona/libs/1507/1507818/2e18c2eb034e05f42c0b_20240709174123699.jpg";

            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                // �ٿ�ε� �� �����͸� Texture2D�� ��ȯ
                DownloadHandlerTexture handler = downloadHandler as DownloadHandlerTexture;
                Texture2D texture = handler.texture;

                // texture�� �̿��ؼ� Sprite�� ��ȯ
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
            //������ �����͸� ������
            UserInfo userInfo = new UserInfo();
            userInfo.name = "��Ÿ����";
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

        //�������� ������ �ø� ��ũ��Ʈ ������
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ScriptTest scripttest = new ScriptTest();
            scripttest.script = "��ũ�����̶� ��ǻ�� ����� �Ǵ� ����Ʈ�� �� ���ڱ�� ȭ�鿡 ���̴� �״�θ� ���� ��� ������ �ǹ��մϴ�. �Ϲ����� �������� �ٸ��� �ؽ�Ʈ�� �����ϴ� �̹����̰ų�, �̹����� �ؽ�Ʈ�� ȥ��Ǿ� �ִ� ��찡 �����ϴ�. ������� ��ũ������ ����ϴ� ������ Ư�� ���� ���ų� ���ͳ� �˻� ���̵� �޴����� ���������� ���� ������ �� �ִ� ������ ������ ������ �� �ֱ� �����Դϴ�. �׷��� �پ��� ������ ���ϼ� ���� �����Ǿ� �־� ã�� ����� �������� �ֽ��ϴ�.";
            HttpInfo info = new HttpInfo();
            info.url = "http://172.20.10.7:8080/ppt/edit_script";
            info.body = JsonUtility.ToJson(scripttest);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                print(downloadHandler.text);
                //string jsonData = "{ \"data\" :" + downloadHandler.text + "}";
                ////jsonData �� PostInfoArray������ �ٲ���
                //allPostInfo = JsonUtility.FromJson<PostInfoArray>(jsonData);
            };

            
            StartCoroutine(HttpMgr.GetInstance().Post(info));
        }

        //�������� ���ڵ� ���� ������
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
                print("��� ũ�� : " + downloadHandler.data.Length);
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
