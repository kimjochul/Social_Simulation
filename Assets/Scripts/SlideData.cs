// SlideData.cs
using System.Collections.Generic;

[System.Serializable]
public class SlideData
{
    public int slideNumber; // 슬라이드 번호
    public string imagePath; // 이미지 경로 또는 이미지 파일의 Base64 인코딩 데이터
    public string originalScript; // 사용자가 입력한 발표 대본 텍스트
    public string modifiedScript; // AI로부터 받은 발표 대본 수정본
    public string finalScript; // 사용자가 최종 수정하여 Confirm한 발표 대본
}
   