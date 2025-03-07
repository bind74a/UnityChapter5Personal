using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;//현재 장착 정보
    public Transform equipParent;//장비가 화면에 보이는 위치

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
        //장착 해제
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    /// <summary>
    /// 장착 해제
    /// </summary>
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);//1인칭 시점의 아이템을 삭제
            curEquip = null;//장착을 해제 해서 초기화
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook) 
            /*조건문 설명
            인풋으로 지정한 키를 누르고있고
            curEquip의 현재 장착아이템에 정보가 있으며
            인벤토리창이 꺼져일때만
            */
        {
            curEquip.OnAttackInput();
        }
    }
}
