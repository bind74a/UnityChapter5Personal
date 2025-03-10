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
            Debug.Log("컨트롤 값 없슴");
        }

        Debug.Log("버프 작동중");

        //버프 수치를 캐릭터에게 적용하고 버프 시간을 코루틴으로 연결
        buffCoroutine = CoroutineManager.BuffCoroutine(BuffTime(buffValue, buffTime));//코루틴을 활성화된 오브젝트에서만 작동
        //buffCoroutine = StartCoroutine(BuffTime(buffValue, buffTime));
    }

    private IEnumerator BuffTime(float value,float time)
    {
        float curSpeed = controller.moveSpeed;//현재 속도
        controller.moveSpeed *= value;//버프 적용

        yield return new WaitForSeconds(time);//버프 지속 시간

        controller.moveSpeed = curSpeed;//속도 복구
    }
}
