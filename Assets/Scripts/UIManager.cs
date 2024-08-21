using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI 요소들을 Unity 에디터에서 할당할 수 있도록 public으로 선언합니다.
    public Image displayImage; // 슬라이드에 표시될 이미지를 담는 UI 이미지
    public InputField originalScriptField; // 사용자가 입력한 원본 스크립트를 담는 입력 필드
    public InputField modifiedScriptField; // 서버로부터 반환된 수정된 스크립트를 표시하는 입력 필드

    public Button previousButton;
    public Button nextButton;

    public Text slideNumberText; // 슬라이드 번호를 표시할 Text UI


    // 다른 매니저 스크립트를 참조하기 위한 변수들
    private SlideManager slideManager;
    private BackendManager_New backendManager;

    private int currentSlideNumber = 1;

    // 초기화 메소드
    private void Start()
    {
        // SlideManager와 BackendManager 컴포넌트를 가져옵니다.
        slideManager = GetComponent<SlideManager>();
        backendManager = GetComponent<BackendManager_New>();

        //previousButton.onClick.AddListener(OnPreviousButtonClick);
        //nextButton.onClick.AddListener(OnNextButtonClick);  

        LoadSlide(currentSlideNumber);
    }

    // "Add Slide" 버튼 클릭 시 호출되는 메소드
    public void OnAddSlideButtonClick()
    {
        //// 현재 표시된 이미지를 Base64 문자열로 변환합니다.
        //string imageBase64 = ImageToBase64();
        //// 사용자가 입력한 원본 스크립트를 가져옵니다.
        //string originalScript = originalScriptField.text;

        //// 슬라이드 매니저를 통해 슬라이드를 추가합니다.
        //// 현재 슬라이드 번호를 사용하여 저장합니다.
        //slideManager.AddSlide(currentSlideNumber, imageBase64, originalScript);
    }

    // "Send Data" 버튼 클릭 시 호출되는 메소드
    public void OnSendDataButtonClick()
    {
        // >>>>>>>>>>>>>> OnAddSlideButtonClick()에 있던 파트(시작)

        // 현재 표시된 이미지를 Base64 문자열로 변환합니다.
        string imageBase64 = ImageToBase64();
        // 사용자가 입력한 원본 스크립트를 가져옵니다.
        string originalScript = originalScriptField.text;

        // 슬라이드 매니저를 통해 슬라이드를 추가합니다.
        // 현재 슬라이드 번호를 사용하여 저장합니다.
        slideManager.AddSlide(currentSlideNumber, imageBase64, originalScript);

        // OnAddSlideButtonClick()에 있던 파트(끝) <<<<<<<<<<<<<<<<



        Debug.Log("OnSendDataButton 클릭되었음.");
        // 슬라이드 번호를 설정합니다. 여기서는 1번 슬라이드를 선택합니다.
        int slideNumber = 1;
        SlideData slideData = slideManager.GetSlide(slideNumber);

        if (slideData != null)
        {
            Debug.Log("slideData가 null이 아님");

            // 서버로 데이터를 전송하고, 응답을 처리할 콜백 메소드를 전달합니다.
            backendManager.SendPresentationData(slideData.slideNumber, slideData.originalScript, OnModifiedScriptReceived);
        }
        else
        {
            Debug.LogError("Slide not found!");
        }
    }


    // 서버로부터 수정된 스크립트를 받았을 때 호출되는 콜백 메소드
    private void OnModifiedScriptReceived(string modifiedScript)
    {

        // 서버로부터 받은 수정된 스크립트를 수정된 스크립트 필드에 표시합니다.
        modifiedScriptField.text = modifiedScript;
    }

    // 현재 표시된 이미지를 Base64 문자열로 변환하는 메소드
    private string ImageToBase64()
    {
        if (displayImage.sprite == null)
        {
            Debug.LogError("Error: No image has been loaded. Please load an image before adding a slide.");
            return null;
        }

        // displayImage에 할당된 Sprite의 텍스처를 가져옵니다.
        Texture2D texture = displayImage.sprite.texture;
        // 텍스처 데이터를 PNG 포맷의 바이트 배열로 인코딩합니다.
        byte[] imageBytes = texture.EncodeToPNG();
        // 바이트 배열을 Base64 문자열로 변환하여 반환합니다.
        return System.Convert.ToBase64String(imageBytes);
    }

    private void LoadSlide(int slideNumber)
    {
        SlideData slideData = slideManager.GetSlide(slideNumber);

        if (slideData != null)
        {
            // 이미지 로드
            if (!string.IsNullOrEmpty(slideData.imagePath))
            {
                byte[] imageData = System.Convert.FromBase64String(slideData.imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                displayImage.sprite = newSprite;
            }

            // 스크립트 로드
            originalScriptField.text = slideData.originalScript;
            modifiedScriptField.text = slideData.modifiedScript;
        }
        else
        {
            // 슬라이드가 없을 경우 필드를 비웁니다.
            displayImage.sprite = null;
            originalScriptField.text = "";
            modifiedScriptField.text = "";
        }

        // 슬라이드 번호 UI 업데이트
        slideNumberText.text = "Slide: " + slideNumber;
    }

    public void OnPreviousButtonClick()
    {
        if (currentSlideNumber > 1)
        {
            currentSlideNumber--;
            LoadSlide(currentSlideNumber);
        }
    }

    public void OnNextButtonClick()
    {
        Debug.Log("Next button clicked.");

        currentSlideNumber++;
        LoadSlide(currentSlideNumber);
    }

    public void OnSendFinalScriptButtonClick(int slideNumber)
    {
        SlideData slideData = slideManager.GetSlide(slideNumber);

        if (slideData != null)
        {
            // finalScript를 백엔드로 전송
            backendManager.SendFinalScript(slideData.slideNumber, slideData.finalScript, OnFinalScriptSent);
        }
        else
        {
            Debug.LogError("Slide not found!");
        }
    }

    private void OnFinalScriptSent(string response)
    {
        // 서버로부터 응답을 받은 후 처리
        Debug.Log("Final Script sent successfully. Server Response: " + response);
    }

}
