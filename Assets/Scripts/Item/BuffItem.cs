using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : MonoBehaviour
{
    private PlayerController controller;
    private Coroutine buffCoroutine;

    float buffValue;
    float buffTime;

    public void SetBuff(float value, float time)
    {
        controller = CharacterManager.Instance.Player.controller;

        buffValue = value;
        buffTime = time;
        Buff();
        //CharacterManager.Instance.Player.controller.buff += Buff;
    }

    public void Buff()
    {
        if (controller == null)
        {
            Debug.Log("��Ʈ�� �� ����");
        }

        Debug.Log("���� �۵���");

        //���� ��ġ�� ĳ���Ϳ��� �����ϰ� ���� �ð��� �ڷ�ƾ���� ����
        buffCoroutine = CoroutineManager.BuffCoroutine(BuffTime(buffValue, buffTime));//�ڷ�ƾ�� Ȱ��ȭ�� ������Ʈ������ �۵�
        //buffCoroutine = StartCoroutine(BuffTime(buffValue, buffTime));
    }

    private IEnumerator BuffTime(float value,float time)
    {
        float curSpeed = controller.moveSpeed;//���� �ӵ�
        controller.moveSpeed *= value;//���� ����

        yield return new WaitForSeconds(time);//���� ���� �ð�

        controller.moveSpeed = curSpeed;//�ӵ� ����
    }
}
