using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<IDamagalbe> things = new List<IDamagalbe>(); //지속 데미지 

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DealDamage",0, damageRate);
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicaIDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagalbe damagalbe))
        //충돌한 오브젝트가 IDamagalbe 컴포넌트를 가지고 잇다면 true
        //TryGetComponent 는 GetComponent 역할도 겸하고 있어서 컴포넌트를 가져오면서 반환값을 bool 쓸수잇는것이다
        //IDamagalbe(자료형) damagalbe(변수)에 가져온 컴포넌트을 넣고 
        {
            things.Add(damagalbe);//리스트에 그컴포넌트를 넣는다
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagalbe damagalbe))
        {
            things.Remove(damagalbe);//리스트에서 컴포넌트 제거
        }
    }
}
