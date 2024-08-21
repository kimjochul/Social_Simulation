using UnityEngine;
using UnityEngine.UI;

public class RealPresentationManager : MonoBehaviour
{
    public Image slideImage; // 슬라이드를 표시할 UI Image 컴포넌트
    public Button nextButton; // 다음 슬라이드로 이동하는 버튼

    private SlideManager slideManager;
    private int currentSlideIndex = 0;

    private void Start()
    {
        // SlideManager 오브젝트 찾기 (DontDestroyOnLoad 된 오브젝트)
        slideManager = FindObjectOfType<SlideManager>();

        if (slideManager != null && slideManager.presentationData.slides.Count > 0)
        {
            LoadSlide(currentSlideIndex); // 첫 슬라이드를 로드합니다.
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
            // 여기서 다음 씬을 로드하거나 다른 동작을 수행할 수 있습니다.
        }
    }
}

