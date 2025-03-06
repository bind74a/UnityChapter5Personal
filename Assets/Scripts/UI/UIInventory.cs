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
            if (slots[i] != null)
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
}
