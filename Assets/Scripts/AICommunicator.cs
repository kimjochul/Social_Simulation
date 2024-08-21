using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;  // TextMeshPro 사용 시

public class AICommunicator : MonoBehaviour
{
    public TMP_InputField scriptInputField;  // 사용자가 입력하는 대본 필드
    public TMP_InputField modifiedScriptField;  // AI 수정본을 표시하고 사용자가 수정할 수 있는 필드

    private string aiUrl = "http://localhost:5000/modify-text"; // Flask 서버의 로컬 URL

    public void SendTextToAI()
    {
        string userInputText = scriptInputField.text;  // InputField에서 사용자가 입력한 텍스트를 가져옴
        StartCoroutine(PostRequest(userInputText));
    }

    IEnumerator PostRequest(string text)
    {
        string jsonData = JsonUtility.ToJson(new { inputText = text });

        UnityWebRequest www = new UnityWebRequest(aiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string modifiedText = www.downloadHandler.text; // 서버로부터 받은 수정본
            modifiedScriptField.text = modifiedText; // 수정본을 InputField에 표시
        }
        else
        {
            Debug.LogError("서버와의 통신 실패: " + www.error);
        }
    }

    // 최종본을 백엔드로 전송하는 메서드
    public void SendFinalTextToBackend()
    {
        string finalText = modifiedScriptField.text; // 사용자가 수정한 최종본
        // 이 최종본을 백엔드로 전송하는 로직을 구현
        StartCoroutine(PostFinalText(finalText));
    }

    IEnumerator PostFinalText(string finalText)
    {
        string jsonData = JsonUtility.ToJson(new { finalText = finalText });

        UnityWebRequest www = new UnityWebRequest(aiUrl + "/finalize", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("최종본이 백엔드로 전송되었습니다.");
        }
        else
        {
            Debug.LogError("서버로 최종본 전송 실패: " + www.error);
        }
    }
}
