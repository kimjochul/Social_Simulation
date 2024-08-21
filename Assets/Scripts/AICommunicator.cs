using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class s : MonoBehaviour
{
    public InputField scriptInputField;  // 사용자가 입력한 대본을 받을 InputField
    private string aiUrl = "http://192.168.1.77:8080/ppt/script"; // AI 서버의 엔드포인트 URL

    // 텍스트를 AI 서버에 전송하는 메소드
    public void SendTextToAI(string text)
    {
        string userInputText = scriptInputField.text;  // InputField에서 사용자가 입력한 텍스트를 가져옴
        StartCoroutine(PostRequestJson(text));
    }

    IEnumerator PostRequestJson(string text)
    {
        // JSON 데이터로 변환
        string jsonData = JsonUtility.ToJson(new { inputText = text });

        // JSON 데이터로 POST 요청을 만듭니다.
        UnityWebRequest www = new UnityWebRequest(aiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("AI의 응답: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("서버와의 통신 실패: " + www.error);
        }
    }

}
