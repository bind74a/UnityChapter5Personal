using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<IDamagalbe> things = new List<IDamagalbe>(); //���� ������ 

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
        //�浹�� ������Ʈ�� IDamagalbe ������Ʈ�� ������ �մٸ� true
        //TryGetComponent �� GetComponent ���ҵ� ���ϰ� �־ ������Ʈ�� �������鼭 ��ȯ���� bool �����մ°��̴�
        //IDamagalbe(�ڷ���) damagalbe(����)�� ������ ������Ʈ�� �ְ� 
        {
            things.Add(damagalbe);//����Ʈ�� ��������Ʈ�� �ִ´�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagalbe damagalbe))
        {
            things.Remove(damagalbe);//����Ʈ���� ������Ʈ ����
        }
    }
}
