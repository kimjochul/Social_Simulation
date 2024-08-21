using UnityEngine;
using UnityEngine.UI;
using SFB;  // Standalone File Browser 플러그인을 사용하기 위해 SFB 네임스페이스를 추가합니다.
using System.IO;

public class ImageLoader : MonoBehaviour
{
    // UI에 이미지를 표시할 Image 컴포넌트 참조
    public Image displayImage;
    // 선택한 이미지 파일 경로를 저장하는 변수
    private string imagePath;

    // 파일 열기 대화상자를 통해 이미지를 선택하고 로드하는 메소드
    public void LoadImageFromFile()
    {
        // 파일 확장자 필터 설정: 이미지 파일(png, jpg, jpeg)과 모든 파일을 선택할 수 있게 합니다.
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };

        // 파일 열기 대화상자를 표시하고 선택된 파일의 경로를 가져옵니다.
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an Image", "", extensions, false);

        // 파일이 선택되고 경로가 유효할 경우, 이미지 로드를 진행합니다.
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            imagePath = paths[0];
            LoadImage(imagePath);
        }
    }

    // 지정된 경로의 이미지를 로드하여 UI에 표시하는 메소드
    void LoadImage(string path)
    {
        // 파일이 실제로 존재하는지 확인합니다.
        if (File.Exists(path))
        {
            // 파일의 모든 바이트 데이터를 읽어옵니다.
            byte[] fileData = File.ReadAllBytes(path);
            // 텍스처를 생성하고, 읽어온 이미지 데이터를 텍스처에 로드합니다.
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            // 텍스처를 사용하여 새로운 스프라이트를 생성합니다.
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // 생성한 스프라이트를 UI 이미지에 할당하여 표시합니다.
            displayImage.sprite = newSprite;
        }
    }
}
