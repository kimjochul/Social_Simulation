// SlideData.cs
using System.Collections.Generic;

[System.Serializable]
public class SlideData
{
    public int slideNumber; // �����̵� ��ȣ
    public string imagePath; // �̹��� ��� �Ǵ� �̹��� ������ Base64 ���ڵ� ������
    public string originalScript; // ����ڰ� �Է��� ��ǥ �뺻 �ؽ�Ʈ
    public string modifiedScript; // AI�κ��� ���� ��ǥ �뺻 ������
    public string finalScript; // ����ڰ� ���� �����Ͽ� Confirm�� ��ǥ �뺻
}
   