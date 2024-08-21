using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BackendManager : MonoBehaviour
{
    private string backendUrl = "http://192.168.1.77:8080/ppt/script";

    // ��ǥ �����͸� ������ ������ �޼ҵ�. �����̵� ��ȣ�� ���� �뺻�� ������ �����ϸ�,
    // �����κ��� ������ �뺻�� ������, onModifiedScriptReceived �ݹ��� ���� ��ȯ�մϴ�.
    public void SendPresentationData(int slideNumber, string originalScript, System.Action<string> onModifiedScriptReceived)
    {
        Debug.Log("SendPresentationData ȣ���");

        // �ڷ�ƾ�� �����Ͽ� �񵿱� ������� �����͸� ������ �����մϴ�.
        StartCoroutine(PostPresentationData(slideNumber, originalScript, onModifiedScriptReceived));
    }

    // ��ǥ �����͸� ������ POST ��û�� ���� �����ϴ� �ڷ�ƾ.
    // �����̵� ��ȣ�� ���� �뺻�� JSON ���·� ����ȭ�Ͽ� ������ �����մϴ�.
    IEnumerator PostPresentationData(int slideNumber, string originalScript, System.Action<string> onModifiedScriptReceived)
    {
        Debug.Log("PostPresentationData �ڷ�ƾ ���۵�");

        // �����̵� ��ȣ�� ���� �뺻�� JSON ������ ���ڿ��� ��ȯ�մϴ�.
        string jsonData = JsonUtility.ToJson(new { slideNumber, originalScript });
        Debug.Log("JSON ������ ������: " + jsonData);

        // ���ο� UnityWebRequest�� �����Ͽ� POST ��û�� �����մϴ�.
        UnityWebRequest www = new UnityWebRequest(backendUrl, "POST");

        // JSON �����͸� ����Ʈ �迭�� ��ȯ�Ͽ� ������ �����͸� �����մϴ�.
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // ���� ������ ó���� DownloadHandlerBuffer�� �����մϴ�.
        www.downloadHandler = new DownloadHandlerBuffer();

        // ��û ����� Content-Type�� JSON���� �����Ͽ� ������ �ùٸ��� �����͸� ó���� �� �ֵ��� �մϴ�.
        www.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("�� ��û �غ� �Ϸ�. ������ ���� �õ� ��...");

        // ������ ��û�� ������ ������ ��ٸ��ϴ�.
        yield return www.SendWebRequest();

        Debug.Log("�� ��û ���� �Ϸ�");

        // ��û�� �����ߴ��� Ȯ���մϴ�.
        if (www.result == UnityWebRequest.Result.Success)
        {
            // �����κ��� ������ �뺻�� �޽��ϴ�.
            string modifiedScript = www.downloadHandler.text;
            // ������ �뺻�� �ݹ��� ���� ��ȯ�մϴ�.
            onModifiedScriptReceived?.Invoke(modifiedScript);

            Debug.Log("Successfully sent data to the server.");
            Debug.Log("Server Response: " + www.downloadHandler.text); // �������� ���� ������ �α׷� ���
        }
        else
        {
            // ��û�� �������� ���, ���� �޽����� ����մϴ�.
            Debug.LogError("������ ��ǥ �뺻 ���� ����: " + www.error);
        }
    }
}
