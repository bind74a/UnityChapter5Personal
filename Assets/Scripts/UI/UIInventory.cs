using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;//������ ������ ����

    public GameObject inventoryWindow;//������ â ����
    public Transform slotPanel;//������ ������ ��ġ
    public Transform dropPosition;//������ ������ ��ġ

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;//������ �̸�
    public TextMeshProUGUI selectedItemDescription;//������ ����
    public TextMeshProUGUI selectedStatName;//�������� ȿ�� �̸�
    public TextMeshProUGUI selectedStatValue;//����Ŵ� �������� ȿ����
    public GameObject useButton;//����ư
    public GameObject equipButton;//������ư
    public GameObject unequipButton;//��ü��ư
    public GameObject dropButton;//������ ��ư

    private PlayerController controller;//�÷��̾� ��Ʈ�� ����
    private PlayerCondition condition;//�÷��̾� ������ ����

    //�κ��丮 �� ����
    ItemData selecteditem;//�κ��丮 ������ ����
    int selecteditemIndex = 0;//�κ��丮 ���� ��ȣ

    int curEquipIndex;//������ �ش�������� ���� ��ȣ

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;//���� �۾�
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;//��������Ʈ�� �Լ� ������
        CharacterManager.Instance.Player.addItem += AddItem;//��������Ʈ

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount]; //slotPanel�� �ڽ� ������Ʈ�� �Լ���ŭ �迭 ����

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();//���� ���Կ� �ִ� �������� ������ slots���� �迭�� ����
            slots[i].index = i;//���Կ� ��ȣ �ο�
            slots[i].inventory = this; //slots��inventory������ ������ ��������
        }

        ClearSelctedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// text�� ���� �ʱ�ȭ
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
    /// �κ��丮â Ȱ��ȭ
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
    /// �κ��丮â Ȱ��ȭ �÷���
    /// </summary>
    /// <returns></returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;//activeInHierarchy�� ������ƮȰ��ȭ�� bool���� ���ϴ°�
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData; //itemData�� ������ִ� ������ data������ �ִ´�

        //������ ���������� �������� canStack bool �� üũ
        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);//���� ������������ slot�� �ִ´�
            if (slot != null)//������ ��������ʴٸ�
            {
                slot.quantity++;//������ ���� ����
                //ui������Ʈ
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        //����ִ� ���Ե��� �����´�.
        ItemSlot emptySlot = GetEmptySlot();

        //����ִ� ������ �մٸ� �׽��Կ� �ְ�
        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //����ִ� ������ ���ٸ� ������ ���
        ThorwItem(data);
        CharacterManager.Instance.Player.itemData = null;//��������Ʈ ���� �ʱ�ȭ
    }

    /// <summary>
    /// �κ��丮�� �� ������ ���� �޼���
    /// </summary>
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();//������ ��������� ������ ���
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    /// <summary>
    /// �������� ���� ����
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0;i < slots.Length ;i++)
        {
            if(slots[i].item == data && slots[i].quantity < data.maxStackAmount)//�������� �ϴ� �������� ������ ���� ���� ������ ������ �ִ밹������ ������
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
        //���� �������� �����ϰ� 360�� �����ϰ� ȸ���ϸ鼭 �������� �Ѵ� 
    }

    /// <summary>
    /// ������ �������� ������ ����
    /// </summary>
    /// <param name="index"></param>
    public void Selectitem(int index)
    {
        if (slots[index].item == null) return;

        selecteditem = slots[index].item; //������ ���ڷ� ������ ��ȣ�� ������ ������ ������ selecteditem������ �ִ´�
        selecteditemIndex = index;

        selectedItemName.text = selecteditem.displayName;//�̸� �ؽ�Ʈ�� �������̸� ���� ������
        selectedItemDescription.text = selecteditem.description;//���� �ؽ�Ʈ�� ������ ���� ���� ������

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i=0; i < selecteditem.Consumables.Length; i++)//������ ��ȭ�� ��ŭ
        {
            selectedStatName.text += selecteditem.Consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selecteditem.Consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selecteditem.type == ItemType.Consumable);//�������� �Һ�������ϰ�쿡�� ��ư Ȱ��ȭ
        equipButton.SetActive(selecteditem.type == ItemType.Equipable && !slots[index].equipped);//�������� ���������̰� �������� equipped ���� false �϶�
        unequipButton.SetActive(selecteditem.type == ItemType.Equipable && slots[index].equipped);//�������� ���������̰� �������� equipped ���� true �϶�
        dropButton.SetActive(true);
    }

    /// <summary>
    /// ����ϱ� ��ư
    /// </summary>
    public void OnUseButton()
    {
        if(selecteditem.type == ItemType.Consumable)
        {
            for(int i = 0; i < selecteditem.Consumables.Length; i++)
            {
                switch (selecteditem.Consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selecteditem.Consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selecteditem.Consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    /// <summary>
    /// ������ ��ư
    /// </summary>
    public void OnDropButton()
    {
        ThorwItem(selecteditem);
        RemoveSelectedItem();
    }

    /// <summary>
    /// ���� ������ �κ��丮���� ����
    /// </summary>
    void RemoveSelectedItem()
    {
        slots[selecteditemIndex].quantity--;

        if (slots[selecteditemIndex].quantity <= 0)//�������� ������ 0�̰ų� �׺��� �Ʒ��� 
        {
            //�������� ���� �ʱ�ȭ
            selecteditem = null;
            slots[selecteditemIndex].item = null;
            selecteditemIndex = -1;
            ClearSelctedItemWindow();
        }

        UpdateUI();
    }

    /// <summary>
    /// �����ϱ� ��ư
    /// </summary>
    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)//������ ������ �������� �����̰����Ѱ�?
        {
            //UnEquip
            Debug.Log(curEquipIndex);
            UnEquip(curEquipIndex);
        }

        slots[selecteditemIndex].equipped = true;
        curEquipIndex = selecteditemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selecteditem);
        UpdateUI();

        Selectitem(selecteditemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if(selecteditemIndex == index)
        {
            Selectitem(selecteditemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selecteditemIndex);
    }
}
