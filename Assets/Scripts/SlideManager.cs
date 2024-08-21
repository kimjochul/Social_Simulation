using UnityEngine;

// SlideManager는 프레젠테이션 슬라이드를 관리하는 클래스입니다.
// 슬라이드를 추가, 업데이트, 조회하는 기능을 제공합니다.
public class SlideManager : MonoBehaviour
{
    // PresentationData 객체는 프레젠테이션의 모든 슬라이드 데이터를 저장합니다.
    public PresentationData presentationData = new PresentationData();

    // AddSlide 메서드는 새로운 슬라이드를 추가합니다.
    // slideNumber: 슬라이드 번호
    // imagePath: 슬라이드 이미지 경로
    // originalScript: 슬라이드의 원본 스크립트
    public void AddSlide(int slideNumber, string imageBase64, string originalScript)
    {
        SlideData slideData = new SlideData
        {   
            slideNumber = slideNumber,
            imagePath = imageBase64, // 이미지 데이터를 Base64로 저장
            originalScript = originalScript
        };

        presentationData.slides.Add(slideData);
    }

    // UpdateSlide 메서드는 기존 슬라이드를 업데이트합니다.
    // slideNumber: 업데이트할 슬라이드 번호
    // modifiedScript: 수정된 스크립트
    // finalScript: 최종 스크립트
    public void UpdateSlide(int slideNumber, string modifiedScript, string finalScript)
    {
        SlideData slide = presentationData.slides.Find(s => s.slideNumber == slideNumber);

        if (slide != null)
        {
            slide.modifiedScript = modifiedScript;
            slide.finalScript = finalScript;
        }
    }

    // GetSlide 메서드는 특정 슬라이드를 반환합니다.
    // slideNumber: 가져올 슬라이드 번호
    // 반환값: 슬라이드 데이터(SlideData)
    public SlideData GetSlide(int slideNumber)
    {
        return presentationData.slides.Find(s => s.slideNumber == slideNumber);
    }
}
