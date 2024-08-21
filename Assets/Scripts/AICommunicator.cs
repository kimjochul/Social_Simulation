using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;  // TextMeshPro ��� ��

public class AICommunicator : MonoBehaviour
{
    public TMP_InputField scriptInputField;  // ����ڰ� �Է��ϴ� �뺻 �ʵ�
    public TMP_InputField modifiedScriptField;  // AI �������� ǥ���ϰ� ����ڰ� ������ �� �ִ� �ʵ�

    private string aiUrl = "http://localhost:5000/modify-text"; // Flask ������ ���� URL

    public void SendTextToAI()
    {
        string userInputText = scriptInputField.text;  // InputField���� ����ڰ� �Է��� �ؽ�Ʈ�� ������
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
            string modifiedText = www.downloadHandler.text; // �����κ��� ���� ������
            modifiedScriptField.text = modifiedText; // �������� InputField�� ǥ��
        }
        else
        {
            Debug.LogError("�������� ��� ����: " + www.error);
        }
    }

    // �������� �鿣��� �����ϴ� �޼���
    public void SendFinalTextToBackend()
    {
        string finalText = modifiedScriptField.text; // ����ڰ� ������ ������
        // �� �������� �鿣��� �����ϴ� ������ ����
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
            Debug.Log("�������� �鿣��� ���۵Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("������ ������ ���� ����: " + www.error);
        }
    }
}
