using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //���� ������
    public float startValue; //������ ���� �κ�
    public float maxValue; //������ �ִ밪
    public float passiveValue; //�������� ��ȭ�� �ִ°�
    public Image uiBar;

    // Start is called before the first frame update
    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // ui ������Ʈ
        uiBar.fillAmount = GetPercentage(); //fillAmount�� ���� ������ ǥ��
    }

    /// <summary>
    /// ������ ��ȭ ǥ�������� ������ ��� �޼��� (ü���� ������ ���� ü�°� Ǯü���� ����)
    /// </summary>
    /// <returns></returns>
    float GetPercentage()
    {
        return curValue / maxValue;
    }


    /// <summary>
    /// ������ ȸ�� ��ȭ ����
    /// </summary>
    /// <param name="value"></param>
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);//�ִ밪�� �Ѿ�� maxValue������ ����
    }

    /// <summary>
    /// ������ ���� ��ȭ ����
    /// </summary>
    /// <param name="value"></param>
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);//�ּҰ����� �������� 0������ ����
    }
}
