using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;//공격 주기
    private bool attacking;//공격 플래그
    public float attackDistance;//공격 사거리

    [Header("Resource Gathering")]//리소스를 가져올수있는지 bool값
    public bool doesGatherResources;

    [Header("Combat")]//공격이 가능한지를 설정
    public bool doesDealDamage; //데미지를 줄수잇는지?
    public int damage; //데미지 지정 변수

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
