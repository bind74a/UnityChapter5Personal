using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;//�ڷ�ƾ�� ����Ҷ��� ���� ����


    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
        //onTakeDamage ��������Ʈ ������ Flash �Լ� �߰�
    }

    public void Flash()
    {
        //�������� �ڷ�ƾ�� ������ ���� ����
        if (coroutine != null)
        {
            StopCoroutine(coroutine);//���� �ȿ��� ����ǰ��ִ� �ڷ�ƾ ����
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());//�ڷ�ƾ �����ѵ� coroutine ������ ���� �����ȿ��� ��� ����� ����
    }
    
    /// <summary>
    /// �ڷ�ƾ �ǽ� �ǰ� ȭ�� (�ǰݽ� ȭ���� �Ӿ����°�)
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while(a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime; //�ǰݽ� ������ ȭ���� ���ƿ��°�
            image.color = new Color(1f,100f/255f,100f/255f, a);
            yield return null; //yield�� �������� ��ٸ��� �ڷ�ƾ �۵� (while�� ����) ȭ�� �ǰ� ������
        }

        image.enabled = false;
    }
}
