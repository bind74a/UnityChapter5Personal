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

    //��������Ʈ
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
    /// ü�� ȸ�� �޼���
    /// </summary>
    /// <param name="amout"></param>
    public void Heal(float amout)
    {
        health.Add(amout);
    }
    /// <summary>
    /// ����� ȸ�� �޼���
    /// </summary>
    /// <param name="amout"></param>
    public void Eat(float amout)
    {
        hunger.Add(amout);
    }
    
    void Die()
    {
        Debug.Log("�׾����ϴ�.");
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakePhysicaIDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();//(onTakeDamage) ��������Ʈ ������ ���� �Լ� ���� 
    }
}
