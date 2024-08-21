using UnityEngine;
using UnityEngine.UI;

public class ImageDisplayer : MonoBehaviour
{
    public RawImage imageDisplay;

    void Start()
    {
        // PlayerPrefs���� Base64�� ���ڵ��� �̹��� �ҷ�����
        string imageBase64 = PlayerPrefs.GetString("savedImage", null);
        if (!string.IsNullOrEmpty(imageBase64))
        {
            byte[] imageData = System.Convert.FromBase64String(imageBase64);

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            imageDisplay.texture = texture;
            imageDisplay.SetNativeSize();
        }
    }
}
