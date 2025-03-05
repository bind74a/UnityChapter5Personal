using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //현재 게이지
    public float startValue; //게이지 시작 부분
    public float maxValue; //게이지 최대값
    public float passiveValue; //게이지의 변화를 주는값
    public Image uiBar;

    // Start is called before the first frame update
    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // ui 업데이트
        uiBar.fillAmount = GetPercentage(); //fillAmount로 현재 게이지 표기
    }

    /// <summary>
    /// 게이지 변화 표현을위한 게이지 등분 메서드 (체력이 닳을시 현재 체력과 풀체력의 간격)
    /// </summary>
    /// <returns></returns>
    float GetPercentage()
    {
        return curValue / maxValue;
    }


    /// <summary>
    /// 게이지 회복 변화 연산
    /// </summary>
    /// <param name="value"></param>
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);//최대값이 넘어갈시 maxValue값으로 고정
    }

    /// <summary>
    /// 게이지 감소 변화 연산
    /// </summary>
    /// <param name="value"></param>
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);//최소값보다 내려갈시 0값으로 고정
    }
}
