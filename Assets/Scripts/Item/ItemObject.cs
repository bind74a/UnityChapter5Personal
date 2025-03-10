using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable//모든 아이템들이 쓸 메서드
{
    public string GetInteractPrompt();//오브젝트 설명창 메서드

    public void OnInteract();//상호 작용 메서드
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()//호출한곳의 오브젝트 설명창에 아이템의 데이터를 보낸다
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()//플레이어가 상오작용키를 입력시 호출
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        Destroy(gameObject);
    }
}
