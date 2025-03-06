using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable//��� �����۵��� �� �޼���
{
    public string GetInteractPrompt();//������Ʈ ����â �޼���

    public void OnInteract();//��ȣ �ۿ� �޼���
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()//ȣ���Ѱ��� ������Ʈ ����â�� �������� �����͸� ������
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()//�÷��̾ ����ۿ�Ű�� �Է½� ȣ��
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        Destroy(gameObject);
    }
}
