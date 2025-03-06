using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;//아이템 데이터 정보

    public UIInventory inventory;//인벤토리 정보

    public int index;//아이템 슬롯의 갯수 번호
    public bool equipped;//장착여부
    public int quantity;//아이템의 수량

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set()
    {

    }

    /// <summary>
    /// 아이템을 사용하거나 버릴때 호출하는 메서드
    /// </summary>
    public void Clear()
    {

    }
}
