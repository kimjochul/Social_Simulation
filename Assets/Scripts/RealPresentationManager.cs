using UnityEngine;
using UnityEngine.UI;

public class RealPresentationManager : MonoBehaviour
{
    public Image slideImage; // �����̵带 ǥ���� UI Image ������Ʈ
    public Button nextButton; // ���� �����̵�� �̵��ϴ� ��ư

    private SlideManager slideManager;
    private int currentSlideIndex = 0;

    private void Start()
    {
        // SlideManager ������Ʈ ã�� (DontDestroyOnLoad �� ������Ʈ)
        slideManager = FindObjectOfType<SlideManager>();

        if (slideManager != null && slideManager.presentationData.slides.Count > 0)
        {
            LoadSlide(currentSlideIndex); // ù �����̵带 �ε��մϴ�.
        }

        nextButton.onClick.AddListener(OnNextButtonClick);
    }

    private void LoadSlide(int slideIndex)
    {
        if (slideIndex < slideManager.presentationData.slides.Count)
        {
            SlideData slideData = slideManager.presentationData.slides[slideIndex];
            byte[] imageData = System.Convert.FromBase64String(slideData.imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            slideImage.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Slide index out of range");
        }
    }

    private void OnNextButtonClick()
    {
        currentSlideIndex++;
        if (currentSlideIndex < slideManager.presentationData.slides.Count)
        {
            LoadSlide(currentSlideIndex);
        }
        else
        {
            Debug.Log("End of slides reached.");
            // ���⼭ ���� ���� �ε��ϰų� �ٸ� ������ ������ �� �ֽ��ϴ�.
        }
    }
}

