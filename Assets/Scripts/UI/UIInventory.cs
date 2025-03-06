using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;//아이템 슬롯의 정보

    public GameObject inventoryWindow;//아이템 창 정보
    public Transform slotPanel;//아이템 슬롯의 위치
    public Transform dropPosition;//아이템 버리는 위치

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;//아이템 이름
    public TextMeshProUGUI selectedItemDescription;//아이템 설명
    public TextMeshProUGUI selectedStatName;//아이템의 효과 이름
    public TextMeshProUGUI selectedStatValue;//적용돼는 아이템의 효과값
    public GameObject useButton;//사용버튼
    public GameObject equipButton;//장착버튼
    public GameObject unequipButton;//해체버튼
    public GameObject dropButton;//버리기 버튼

    private PlayerController controller;//플레이어 컨트롤 정보
    private PlayerCondition condition;//플레이어 컨지션 정보

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;//연결 작업
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;//델리게이트로 함수 보내기
        CharacterManager.Instance.Player.addItem += AddItem;//델리게이트

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount]; //slotPanel의 자식 오브젝트의 게수많큼 배열 생성

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();//각각 슬롯에 있는 아이템의 정보을 slots변수 배열에 저장
            slots[i].index = i;//슬롯에 번호 부여
            slots[i].inventory = this; //slots의inventory변수에 아이템 정보전달
        }

        ClearSelctedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// text의 정보 초기화
    /// </summary>
    void ClearSelctedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    /// <summary>
    /// 인벤토리창 활성화
    /// </summary>
    public void Toggle()
    {
        if(IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }
    /// <summary>
    /// 인벤토리창 활성화 플래그
    /// </summary>
    /// <returns></returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;//activeInHierarchy은 오브젝트활성화로 bool값을 정하는것
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData; //itemData에 저장되있는 정보를 data변수에 넣는다

        //아이템 묶음소지가 가능한지 canStack bool 값 체크
        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);//받은 아이템정보를 slot에 넣는다
            if (slot != null)//슬롯이 비어있지않다면
            {
                slot.quantity++;//아이템 개수 증가
                //ui업데이트
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        //비어있는 슬롯들을 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        //비어있는 슬롯이 잇다면 그슬롯에 넣고
        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //비어있는 슬롯이 없다면 아이템 드롭
        ThorwItem(data);
        CharacterManager.Instance.Player.itemData = null;//델리게이트 변수 초기화
    }

    /// <summary>
    /// 인벤토리에 들어간 아이템 정리 메서드
    /// </summary>
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Set();//슬롯이 비어있을때 아이템 등록
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    /// <summary>
    /// 아이템의 갯수 증가
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0;i < slots.Length ;i++)
        {
            if(slots[i].item == data && slots[i].quantity < data.maxStackAmount)//넣을려고 하는 아이템의 정보가 같고 현재 갯수가 묶음의 최대갯수보다 작을시
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThorwItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        //먹은 프리펩을 복제하고 360도 랜덤하게 회전하면서 떨어지게 한다 
    }
}
