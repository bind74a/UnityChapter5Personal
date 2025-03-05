using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicaIDamage(int damage);
}

public class PlayerCondition : MonoBehaviour , IDamagalbe
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    //델리게이트
    public event Action onTakeDamage;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue == 0f )
        {
            Die();
        }
    }
    /// <summary>
    /// 체력 회복 메서드
    /// </summary>
    /// <param name="amout"></param>
    public void Heal(float amout)
    {
        health.Add(amout);
    }
    /// <summary>
    /// 배고픔 회복 메서드
    /// </summary>
    /// <param name="amout"></param>
    public void Eat(float amout)
    {
        hunger.Add(amout);
    }
    
    void Die()
    {
        Debug.Log("죽엇습니다.");
    }

    /// <summary>
    /// 데미지 적용 함수
    /// </summary>
    /// <param name="damage"></param>
    public void TakePhysicaIDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();//(onTakeDamage) 델리게이트 변수에 들어온 함수 실행 
    }
}
