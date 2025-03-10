using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;//���� ���� ����
    public Transform equipParent;//��� ȭ�鿡 ���̴� ��ġ

    private PlayerController controller;
    private PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        condition = GetComponent<PlayerCondition>();
        controller = GetComponent<PlayerController>();
    }

    public void EquipNew(ItemData data)
    {
        //���� ����
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);//1��Ī ������ �������� ����
            curEquip = null;//������ ���� �ؼ� �ʱ�ȭ
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook) 
            /*���ǹ� ����
            ��ǲ���� ������ Ű�� �������ְ�
            curEquip�� ���� ���������ۿ� ������ ������
            �κ��丮â�� �����϶���
            */
        {
            curEquip.OnAttackInput();
        }
    }
}
