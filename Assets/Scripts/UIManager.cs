using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI ��ҵ��� Unity �����Ϳ��� �Ҵ��� �� �ֵ��� public���� �����մϴ�.
    public Image displayImage; // �����̵忡 ǥ�õ� �̹����� ��� UI �̹���
    public InputField originalScriptField; // ����ڰ� �Է��� ���� ��ũ��Ʈ�� ��� �Է� �ʵ�
    public InputField modifiedScriptField; // �����κ��� ��ȯ�� ������ ��ũ��Ʈ�� ǥ���ϴ� �Է� �ʵ�

    public Button previousButton;
    public Button nextButton;

    public Text slideNumberText; // �����̵� ��ȣ�� ǥ���� Text UI


    // �ٸ� �Ŵ��� ��ũ��Ʈ�� �����ϱ� ���� ������
    private SlideManager slideManager;
    private BackendManager_New backendManager;

    private int currentSlideNumber = 1;

    // �ʱ�ȭ �޼ҵ�
    private void Start()
    {
        // SlideManager�� BackendManager ������Ʈ�� �����ɴϴ�.
        slideManager = GetComponent<SlideManager>();
        backendManager = GetComponent<BackendManager_New>();

        //previousButton.onClick.AddListener(OnPreviousButtonClick);
        //nextButton.onClick.AddListener(OnNextButtonClick);  

        LoadSlide(currentSlideNumber);
    }

    // "Add Slide" ��ư Ŭ�� �� ȣ��Ǵ� �޼ҵ�
    public void OnAddSlideButtonClick()
    {
        //// ���� ǥ�õ� �̹����� Base64 ���ڿ��� ��ȯ�մϴ�.
        //string imageBase64 = ImageToBase64();
        //// ����ڰ� �Է��� ���� ��ũ��Ʈ�� �����ɴϴ�.
        //string originalScript = originalScriptField.text;

        //// �����̵� �Ŵ����� ���� �����̵带 �߰��մϴ�.
        //// ���� �����̵� ��ȣ�� ����Ͽ� �����մϴ�.
        //slideManager.AddSlide(currentSlideNumber, imageBase64, originalScript);
    }

    // "Send Data" ��ư Ŭ�� �� ȣ��Ǵ� �޼ҵ�
    public void OnSendDataButtonClick()
    {
        // >>>>>>>>>>>>>> OnAddSlideButtonClick()�� �ִ� ��Ʈ(����)

        // ���� ǥ�õ� �̹����� Base64 ���ڿ��� ��ȯ�մϴ�.
        string imageBase64 = ImageToBase64();
        // ����ڰ� �Է��� ���� ��ũ��Ʈ�� �����ɴϴ�.
        string originalScript = originalScriptField.text;

        // �����̵� �Ŵ����� ���� �����̵带 �߰��մϴ�.
        // ���� �����̵� ��ȣ�� ����Ͽ� �����մϴ�.
        slideManager.AddSlide(currentSlideNumber, imageBase64, originalScript);

        // OnAddSlideButtonClick()�� �ִ� ��Ʈ(��) <<<<<<<<<<<<<<<<



        Debug.Log("OnSendDataButton Ŭ���Ǿ���.");
        // �����̵� ��ȣ�� �����մϴ�. ���⼭�� 1�� �����̵带 �����մϴ�.
        int slideNumber = 1;
        SlideData slideData = slideManager.GetSlide(slideNumber);

        if (slideData != null)
        {
            Debug.Log("slideData�� null�� �ƴ�");

            // ������ �����͸� �����ϰ�, ������ ó���� �ݹ� �޼ҵ带 �����մϴ�.
            backendManager.SendPresentationData(slideData.slideNumber, slideData.originalScript, OnModifiedScriptReceived);
        }
        else
        {
            Debug.LogError("Slide not found!");
        }
    }


    // �����κ��� ������ ��ũ��Ʈ�� �޾��� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    private void OnModifiedScriptReceived(string modifiedScript)
    {

        // �����κ��� ���� ������ ��ũ��Ʈ�� ������ ��ũ��Ʈ �ʵ忡 ǥ���մϴ�.
        modifiedScriptField.text = modifiedScript;
    }

    // ���� ǥ�õ� �̹����� Base64 ���ڿ��� ��ȯ�ϴ� �޼ҵ�
    private string ImageToBase64()
    {
        if (displayImage.sprite == null)
        {
            Debug.LogError("Error: No image has been loaded. Please load an image before adding a slide.");
            return null;
        }

        // displayImage�� �Ҵ�� Sprite�� �ؽ�ó�� �����ɴϴ�.
        Texture2D texture = displayImage.sprite.texture;
        // �ؽ�ó �����͸� PNG ������ ����Ʈ �迭�� ���ڵ��մϴ�.
        byte[] imageBytes = texture.EncodeToPNG();
        // ����Ʈ �迭�� Base64 ���ڿ��� ��ȯ�Ͽ� ��ȯ�մϴ�.
        return System.Convert.ToBase64String(imageBytes);
    }

    private void LoadSlide(int slideNumber)
    {
        SlideData slideData = slideManager.GetSlide(slideNumber);

        if (slideData != null)
        {
            // �̹��� �ε�
            if (!string.IsNullOrEmpty(slideData.imagePath))
            {
                byte[] imageData = System.Convert.FromBase64String(slideData.imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                displayImage.sprite = newSprite;
            }

            // ��ũ��Ʈ �ε�
            originalScriptField.text = slideData.originalScript;
            modifiedScriptField.text = slideData.modifiedScript;
        }
        else
        {
            // �����̵尡 ���� ��� �ʵ带 ���ϴ�.
            displayImage.sprite = null;
            originalScriptField.text = "";
            modifiedScriptField.text = "";
        }

        // �����̵� ��ȣ UI ������Ʈ
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
    /*
    public void OnSendFinalScriptButtonClick(int slideNumber)
    {
        SlideData slideData = slideManager.GetSlide(slideNumber);

        if (slideData != null)
        {
            // finalScript�� �鿣��� ����
            //backendManager.SendFinalScript(slideData.slideNumber, slideData.finalScript, OnFinalScriptSent);
        }
        else
        {
            Debug.LogError("Slide not found!");
        }
    }
*/
    private void OnFinalScriptSent(string response)
    {
        // �����κ��� ������ ���� �� ó��
        Debug.Log("Final Script sent successfully. Server Response: " + response);
    }

}
