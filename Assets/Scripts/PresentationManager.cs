//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//[System.Serializable]
//public class SlideData
//{
//    public int slideNumber; // �����̵� ��ȣ
//    public string imagePath; // �̹��� ��� �Ǵ� �̹��� ������ Base64 ���ڵ� ������
//    public string originalScript; // ����ڰ� �Է��� ��ǥ �뺻 �ؽ�Ʈ
//    public string modifiedScript; // AI�κ��� ���� ��ǥ �뺻 ������
//    public string finalScript; // ����ڰ� ���� �����Ͽ� Confirm�� ��ǥ �뺻
//}

//[System.Serializable]
//public class PresentationData
//{
//    public List<SlideData> slides; // ��ü �����̵� �����͸� �����ϴ� ����Ʈ
//}


//public class PresentationManager : MonoBehaviour
//{
//    public PresentationData presentationData = new PresentationData();

//    // �����̵带 �߰��ϴ� �޼���
//    public void AddSlide(int slideNumber, string imagePath, string originalScript)
//    {
//        SlideData slideData = new SlideData
//        {
//            slideNumber = slideNumber,
//            imagePath = imagePath,
//            originalScript = originalScript
//        };

//        presentationData.slides.Add(slideData);
//    }

//    // �����̵带 ������Ʈ�ϴ� �޼���
//    public void UpdateSlide(int slideNumber, string modifiedScript, string finalScript)
//    {
//        SlideData slide = presentationData.slides.Find(s => s.slideNumber == slideNumber);
//        if (slide != null)
//        {
//            slide.modifiedScript = modifiedScript;
//            slide.finalScript = finalScript;
//        }
//    }

//    // ��ǥ �����͸� ������ �����ϴ� �޼���
//    public void SendPresentationDataToBackend()
//    {
//        string jsonData = JsonUtility.ToJson(presentationData);
//        StartCoroutine(PostPresentationData(jsonData));
//    }

//    // ������ �����ϴ� �ڷ�ƾ
//    IEnumerator PostPresentationData(string jsonData)
//    {
//        UnityWebRequest www = new UnityWebRequest("your_backend_url", "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        www.downloadHandler = new DownloadHandlerBuffer();
//        www.SetRequestHeader("Content-Type", "application/json");

//        yield return www.SendWebRequest();

//        if (www.result == UnityWebRequest.Result.Success)
//        {
//            Debug.Log("��ǥ �ڷᰡ �鿣��� ���۵Ǿ����ϴ�.");
//        }
//        else
//        {
//            Debug.LogError("������ ��ǥ �ڷ� ���� ����: " + www.error);
//        }
//    }

//    // ����: ��ư Ŭ�� �̺�Ʈ�� �����̵� �߰�
//    public void OnAddSlideButtonClick()
//    {
//        string imagePath = "path_to_image"; // �̹��� ��θ� �������� ���� �ʿ�
//        string originalScript = "User's script"; // ����ڰ� �Է��� �ؽ�Ʈ ��������
//        int slideNumber = 1; // �����̵� ��ȣ ����

//        AddSlide(slideNumber, imagePath, originalScript);
//    }

//    // ����: ���� ��ǥ �ڷḦ ������ ����
//    public void OnSendDataButtonClick()
//    {
//        SendPresentationDataToBackend();
//    }

//    // ����: �����̵� �̵� �� ������ �ε�
//    public void LoadSlide(int slideNumber)
//    {
//        SlideData slide = presentationData.slides.Find(s => s.slideNumber == slideNumber);
//        if (slide != null)
//        {
//            // �̹����� �ؽ�Ʈ�� UI ������Ʈ�� �Ҵ�
//            displayImage.sprite = LoadSpriteFromPath(slide.imagePath);
//            originalScriptField.text = slide.originalScript;
//            modifiedScriptField.text = slide.modifiedScript;
//        }
//    }

//    // �̹��� �ε� �޼��� ����
//    private Sprite LoadSpriteFromPath(string path)
//    {
//        // �̹��� ��ο��� Sprite�� �ε��ϴ� ����
//        Texture2D texture = LoadTextureFromFile(path);
//        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//    }
//}

