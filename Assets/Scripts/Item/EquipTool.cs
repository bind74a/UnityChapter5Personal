using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;//���� �ֱ�
    private bool attacking;//���� �÷���
    public float attackDistance;//���� ��Ÿ�

    [Header("Resource Gathering")]//���ҽ��� �����ü��ִ��� bool��
    public bool doesGatherResources;

    [Header("Combat")]//������ ���������� ����
    public bool doesDealDamage; //�������� �ټ��մ���?
    public int damage; //������ ���� ����

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void OnAttackInput()
    {
        if(!attacking)
        {
            attacking = true;
            anim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }
}
