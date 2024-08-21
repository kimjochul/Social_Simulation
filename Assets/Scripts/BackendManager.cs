using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BackendManager : MonoBehaviour
{
    private string backendUrl = "http://192.168.1.77:8080/ppt/script";

    // 발표 데이터를 서버로 보내는 메소드. 슬라이드 번호와 원본 대본을 서버로 전송하며,
    // 서버로부터 수정된 대본을 받으면, onModifiedScriptReceived 콜백을 통해 반환합니다.
    public void SendPresentationData(int slideNumber, string originalScript, System.Action<string> onModifiedScriptReceived)
    {
        Debug.Log("SendPresentationData 호출됨");

        // 코루틴을 시작하여 비동기 방식으로 데이터를 서버에 전송합니다.
        StartCoroutine(PostPresentationData(slideNumber, originalScript, onModifiedScriptReceived));
    }

    // 발표 데이터를 서버로 POST 요청을 통해 전송하는 코루틴.
    // 슬라이드 번호와 원본 대본을 JSON 형태로 직렬화하여 서버로 전송합니다.
    IEnumerator PostPresentationData(int slideNumber, string originalScript, System.Action<string> onModifiedScriptReceived)
    {
        Debug.Log("PostPresentationData 코루틴 시작됨");

        // 슬라이드 번호와 원본 대본을 JSON 형식의 문자열로 변환합니다.
        string jsonData = JsonUtility.ToJson(new { slideNumber, originalScript });
        Debug.Log("JSON 데이터 생성됨: " + jsonData);

        // 새로운 UnityWebRequest를 생성하여 POST 요청을 설정합니다.
        UnityWebRequest www = new UnityWebRequest(backendUrl, "POST");

        // JSON 데이터를 바이트 배열로 변환하여 전송할 데이터를 설정합니다.
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // 서버 응답을 처리할 DownloadHandlerBuffer를 설정합니다.
        www.downloadHandler = new DownloadHandlerBuffer();

        // 요청 헤더에 Content-Type을 JSON으로 설정하여 서버가 올바르게 데이터를 처리할 수 있도록 합니다.
        www.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("웹 요청 준비 완료. 서버에 전송 시도 중...");

        // 서버에 요청을 보내고 응답을 기다립니다.
        yield return www.SendWebRequest();

        Debug.Log("웹 요청 전송 완료");

        // 요청이 성공했는지 확인합니다.
        if (www.result == UnityWebRequest.Result.Success)
        {
            // 서버로부터 수정된 대본을 받습니다.
            string modifiedScript = www.downloadHandler.text;
            // 수정된 대본을 콜백을 통해 반환합니다.
            onModifiedScriptReceived?.Invoke(modifiedScript);

            Debug.Log("Successfully sent data to the server.");
            Debug.Log("Server Response: " + www.downloadHandler.text); // 서버에서 받은 응답을 로그로 출력
        }
        else
        {
            // 요청이 실패했을 경우, 오류 메시지를 출력합니다.
            Debug.LogError("서버로 발표 대본 전송 실패: " + www.error);
        }
    }
}
