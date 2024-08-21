using UnityEngine;
using UnityEngine.UI;
using SFB;  // Standalone File Browser �÷������� ����ϱ� ���� SFB ���ӽ����̽��� �߰��մϴ�.
using System.IO;

public class ImageLoader : MonoBehaviour
{
    // UI�� �̹����� ǥ���� Image ������Ʈ ����
    public Image displayImage;
    // ������ �̹��� ���� ��θ� �����ϴ� ����
    private string imagePath;

    // ���� ���� ��ȭ���ڸ� ���� �̹����� �����ϰ� �ε��ϴ� �޼ҵ�
    public void LoadImageFromFile()
    {
        // ���� Ȯ���� ���� ����: �̹��� ����(png, jpg, jpeg)�� ��� ������ ������ �� �ְ� �մϴ�.
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };

        // ���� ���� ��ȭ���ڸ� ǥ���ϰ� ���õ� ������ ��θ� �����ɴϴ�.
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an Image", "", extensions, false);

        // ������ ���õǰ� ��ΰ� ��ȿ�� ���, �̹��� �ε带 �����մϴ�.
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            imagePath = paths[0];
            LoadImage(imagePath);
        }
    }

    // ������ ����� �̹����� �ε��Ͽ� UI�� ǥ���ϴ� �޼ҵ�
    void LoadImage(string path)
    {
        // ������ ������ �����ϴ��� Ȯ���մϴ�.
        if (File.Exists(path))
        {
            // ������ ��� ����Ʈ �����͸� �о�ɴϴ�.
            byte[] fileData = File.ReadAllBytes(path);
            // �ؽ�ó�� �����ϰ�, �о�� �̹��� �����͸� �ؽ�ó�� �ε��մϴ�.
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            // �ؽ�ó�� ����Ͽ� ���ο� ��������Ʈ�� �����մϴ�.
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // ������ ��������Ʈ�� UI �̹����� �Ҵ��Ͽ� ǥ���մϴ�.
            displayImage.sprite = newSprite;
        }
    }
}
