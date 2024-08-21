using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class s : MonoBehaviour
{
    public InputField scriptInputField;  // ����ڰ� �Է��� �뺻�� ���� InputField
    private string aiUrl = "http://192.168.1.77:8080/ppt/script"; // AI ������ ��������Ʈ URL

    // �ؽ�Ʈ�� AI ������ �����ϴ� �޼ҵ�
    public void SendTextToAI(string text)
    {
        string userInputText = scriptInputField.text;  // InputField���� ����ڰ� �Է��� �ؽ�Ʈ�� ������
        StartCoroutine(PostRequestJson(text));
    }

    IEnumerator PostRequestJson(string text)
    {
        // JSON �����ͷ� ��ȯ
        string jsonData = JsonUtility.ToJson(new { inputText = text });

        // JSON �����ͷ� POST ��û�� ����ϴ�.
        UnityWebRequest www = new UnityWebRequest(aiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("AI�� ����: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("�������� ��� ����: " + www.error);
        }
    }

}
