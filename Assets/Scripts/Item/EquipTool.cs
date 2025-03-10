using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;//���� �ֱ�
    private bool attacking;//���� �÷���
    public float attackDistance;//���� ��Ÿ�
    public float useStamina;// ���� ���׹̳�

    [Header("Resource Gathering")]//���ҽ�(�ڿ�)�� �����ü��ִ��� bool��
    public bool doesGatherResources;

    [Header("Combat")]//������ ���������� ����
    public bool doesDealDamage; //�������� �ټ��մ���?
    public int damage; //������ ���� ����

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
            if(CharacterManager.Instance.Player.condition.UseStamina(useStamina))//���׹̳��� ����������(true) �ൿ����
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
    /// �ڿ� ä�� �޼���
    /// </summary>
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, attackDistance))//����ĳ��Ʈ�� attackDistance�� ������ ��Ÿ��� ������Ʈ�� �����̴ٸ�
        {
            if(doesGatherResources && hit.collider.TryGetComponent(out Resurce resurce))
            {
                resurce.Gather(hit.point, hit.normal);//Gather ���� �����ɽ�Ʈ�� ������ ��ġ���̶� ���Ⱚ ������
            }
        }
    }
}
