//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//[System.Serializable]
//public class SlideData
//{
//    public int slideNumber; // 슬라이드 번호
//    public string imagePath; // 이미지 경로 또는 이미지 파일의 Base64 인코딩 데이터
//    public string originalScript; // 사용자가 입력한 발표 대본 텍스트
//    public string modifiedScript; // AI로부터 받은 발표 대본 수정본
//    public string finalScript; // 사용자가 최종 수정하여 Confirm한 발표 대본
//}

//[System.Serializable]
//public class PresentationData
//{
//    public List<SlideData> slides; // 전체 슬라이드 데이터를 저장하는 리스트
//}


//public class PresentationManager : MonoBehaviour
//{
//    public PresentationData presentationData = new PresentationData();

//    // 슬라이드를 추가하는 메서드
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

//    // 슬라이드를 업데이트하는 메서드
//    public void UpdateSlide(int slideNumber, string modifiedScript, string finalScript)
//    {
//        SlideData slide = presentationData.slides.Find(s => s.slideNumber == slideNumber);
//        if (slide != null)
//        {
//            slide.modifiedScript = modifiedScript;
//            slide.finalScript = finalScript;
//        }
//    }

//    // 발표 데이터를 서버로 전송하는 메서드
//    public void SendPresentationDataToBackend()
//    {
//        string jsonData = JsonUtility.ToJson(presentationData);
//        StartCoroutine(PostPresentationData(jsonData));
//    }

//    // 서버로 전송하는 코루틴
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
//            Debug.Log("발표 자료가 백엔드로 전송되었습니다.");
//        }
//        else
//        {
//            Debug.LogError("서버로 발표 자료 전송 실패: " + www.error);
//        }
//    }

//    // 예시: 버튼 클릭 이벤트로 슬라이드 추가
//    public void OnAddSlideButtonClick()
//    {
//        string imagePath = "path_to_image"; // 이미지 경로를 가져오는 로직 필요
//        string originalScript = "User's script"; // 사용자가 입력한 텍스트 가져오기
//        int slideNumber = 1; // 슬라이드 번호 설정

//        AddSlide(slideNumber, imagePath, originalScript);
//    }

//    // 예시: 최종 발표 자료를 서버로 전송
//    public void OnSendDataButtonClick()
//    {
//        SendPresentationDataToBackend();
//    }

//    // 예시: 슬라이드 이동 시 데이터 로드
//    public void LoadSlide(int slideNumber)
//    {
//        SlideData slide = presentationData.slides.Find(s => s.slideNumber == slideNumber);
//        if (slide != null)
//        {
//            // 이미지와 텍스트를 UI 컴포넌트에 할당
//            displayImage.sprite = LoadSpriteFromPath(slide.imagePath);
//            originalScriptField.text = slide.originalScript;
//            modifiedScriptField.text = slide.modifiedScript;
//        }
//    }

//    // 이미지 로드 메서드 예시
//    private Sprite LoadSpriteFromPath(string path)
//    {
//        // 이미지 경로에서 Sprite를 로드하는 로직
//        Texture2D texture = LoadTextureFromFile(path);
//        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//    }
//}

