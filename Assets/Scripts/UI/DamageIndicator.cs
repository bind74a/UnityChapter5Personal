using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;//코루틴을 사용할때는 변수 지정


    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
        //onTakeDamage 델리게이트 변수에 Flash 함수 추가
    }

    public void Flash()
    {
        //실행중인 코루틴이 있을시 강제 종료
        if (coroutine != null)
        {
            StopCoroutine(coroutine);//변수 안에서 실행되고있던 코루틴 정지
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());//코루틴 시작한뒤 coroutine 변수에 저장 변수안에서 계속 실행될 예정
    }
    
    /// <summary>
    /// 코루틴 실습 피격 화면 (피격시 화면이 붉어지는것)
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while(a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime; //피격시 서서히 화면이 돌아오는곳
            image.color = new Color(1f,100f/255f,100f/255f, a);
            yield return null; //yield은 한프레임 기다리고 코루틴 작동 (while문 실행) 화면 피격 딜레이
        }

        image.enabled = false;
    }
}
