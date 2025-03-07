using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;//������ ������ ����

    public Button button;//��ư ����
    public Image icon;//������ ������
    public TextMeshProUGUI quantityText;//������ ���� �ؽ�Ʈ
    private Outline outline;//�κ��丮 ���� ���� ȿ��

    public UIInventory inventory;//�κ��丮 ����

    public int index;//������ ���Ե��� ��ȣ
    public bool equipped;//��������
    public int quantity;//�������� ����

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()//������Ʈ Ȱ��ȭ
    {
        outline.enabled = enabled;//������ â���� ���� ���� ȿ��(outline�� Ȱ��ȭ�� enabled�� bool ������ ���Ѵ�)
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;//ItemData : ScriptableObject �� ����޴� ������ �̹����� �����´�
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        /*���� ������
        quantity > 1 �� bool ��
        true �� quantity.ToString()
        false �� string.Empty;
         */

        //�ƿ����� ����ڵ�
        if(outline != null)
        {
            outline.enabled = enabled;
        }
    }

    /// <summary>
    /// �������� ����ϰų� ������ ȣ���ϴ� �޼���
    /// </summary>
    public void Clear()
    {
        item = null;//������ ���� �ʱ�ȭ
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    /// <summary>
    /// �κ��丮 ������ Ŭ���� ������ ���� ��� �޼���
    /// </summary>
    public void OnClickButton()
    {
        inventory.Selectitem(index);
    }
}
