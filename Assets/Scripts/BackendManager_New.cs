using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public struct TempInfo
{
    public string script;
}

[System.Serializable]
public struct ResultInfo
{
    public string edit_script_result;
}

public class BackendManager_New : MonoBehaviour
{
    private string backendUrl = "http://172.16.17.51:8080/ppt/edit_script";

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
        TempInfo tempInfo = new TempInfo();
        tempInfo.script = originalScript;
        string jsonData =   JsonUtility.ToJson(tempInfo, true);
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

            ResultInfo resultInfo = JsonUtility.FromJson<ResultInfo>(modifiedScript);
            onModifiedScriptReceived?.Invoke(resultInfo.edit_script_result);

            Debug.Log("Successfully sent data to the server.");
            Debug.Log("Server Response: " + www.downloadHandler.text); // �������� ���� ������ �α׷� ���
        }
        else
        {
            // ��û�� �������� ���, ���� �޽����� ����մϴ�.
            Debug.LogError("������ ��ǥ �뺻 ���� ����: " + www.error);
        }
    }

    public void SendFinalScript(int slideNumber, string finalScript, System.Action<string> onResponseReceived)
    {
        Debug.Log("SendFinalScript ȣ���");
        StartCoroutine(PostFinalScript(slideNumber, finalScript, onResponseReceived));
    }

    IEnumerator PostFinalScript(int slideNumber, string finalScript, System.Action<string> onResponseReceived)
    {
        Debug.Log("PostFinalScript �ڷ�ƾ ���۵�");

        TempInfo tempInfo = new TempInfo { script = finalScript };
        string jsonData = JsonUtility.ToJson(tempInfo, true);

        UnityWebRequest www = new UnityWebRequest(backendUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseText = www.downloadHandler.text;
            onResponseReceived?.Invoke(responseText);
        }
        else
        {
            Debug.LogError("������ ���� �뺻 ���� ����: " + www.error);
        }
    }

}
