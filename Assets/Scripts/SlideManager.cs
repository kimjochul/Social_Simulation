using UnityEngine;

// SlideManager�� ���������̼� �����̵带 �����ϴ� Ŭ�����Դϴ�.
// �����̵带 �߰�, ������Ʈ, ��ȸ�ϴ� ����� �����մϴ�.
public class SlideManager : MonoBehaviour
{
    // PresentationData ��ü�� ���������̼��� ��� �����̵� �����͸� �����մϴ�.
    public PresentationData presentationData = new PresentationData();

    // AddSlide �޼���� ���ο� �����̵带 �߰��մϴ�.
    // slideNumber: �����̵� ��ȣ
    // imagePath: �����̵� �̹��� ���
    // originalScript: �����̵��� ���� ��ũ��Ʈ
    public void AddSlide(int slideNumber, string imageBase64, string originalScript)
    {
        SlideData slideData = new SlideData
        {   
            slideNumber = slideNumber,
            imagePath = imageBase64, // �̹��� �����͸� Base64�� ����
            originalScript = originalScript
        };

        presentationData.slides.Add(slideData);
    }

    // UpdateSlide �޼���� ���� �����̵带 ������Ʈ�մϴ�.
    // slideNumber: ������Ʈ�� �����̵� ��ȣ
    // modifiedScript: ������ ��ũ��Ʈ
    // finalScript: ���� ��ũ��Ʈ
    public void UpdateSlide(int slideNumber, string modifiedScript, string finalScript)
    {
        SlideData slide = presentationData.slides.Find(s => s.slideNumber == slideNumber);

        if (slide != null)
        {
            slide.modifiedScript = modifiedScript;
            slide.finalScript = finalScript;
        }
    }

    // GetSlide �޼���� Ư�� �����̵带 ��ȯ�մϴ�.
    // slideNumber: ������ �����̵� ��ȣ
    // ��ȯ��: �����̵� ������(SlideData)
    public SlideData GetSlide(int slideNumber)
    {
        return presentationData.slides.Find(s => s.slideNumber == slideNumber);
    }
}
