using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;//공격 주기
    private bool attacking;//공격 플래그
    public float attackDistance;//공격 사거리
    public float useStamina;// 사용될 스테미나

    [Header("Resource Gathering")]//리소스(자원)를 가져올수있는지 bool값
    public bool doesGatherResources;

    [Header("Combat")]//공격이 가능한지를 설정
    public bool doesDealDamage; //데미지를 줄수잇는지?
    public int damage; //데미지 지정 변수

    private Animator anim;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if(!attacking)
        {
            if(CharacterManager.Instance.Player.condition.UseStamina(useStamina))//스테미나가 남아있을때(true) 행동가능
            {
                attacking = true;
                anim.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    /// <summary>
    /// 자원 채집 메서드
    /// </summary>
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, attackDistance))//레이캐스트의 attackDistance의 설정된 사거리에 오브젝트가 감지됫다면
        {
            if(doesGatherResources && hit.collider.TryGetComponent(out Resurce resurce))
            {
                resurce.Gather(hit.point, hit.normal);//Gather 에게 레이케스트의 감지된 위치값이랑 방향값 보내기
            }
        }
    }
}
