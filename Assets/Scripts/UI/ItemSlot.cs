using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;//아이템 데이터 정보

    public Button button;//버튼 정보
    public Image icon;//아이템 아이콘
    public TextMeshProUGUI quantityText;//아이템 갯수 텍스트
    private Outline outline;//인벤토리 슬롯 선택 효과

    public UIInventory inventory;//인벤토리 정보

    public int index;//아이템 슬롯들의 번호
    public bool equipped;//장착여부
    public int quantity;//아이템의 수량

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()//오브젝트 활성화
    {
        outline.enabled = enabled;//아이템 창착시 슬롯 선택 효과(outline의 활성화를 enabled의 bool 값으로 정한다)
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;//ItemData : ScriptableObject 로 등록햇던 아이콘 이미지를 가져온다
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        /*삼항 연산자
        quantity > 1 는 bool 값
        true 는 quantity.ToString()
        false 는 string.Empty;
         */

        //아웃라인 방어코드
        if(outline != null)
        {
            outline.enabled = enabled;
        }
    }

    /// <summary>
    /// 아이템을 사용하거나 버릴때 호출하는 메서드
    /// </summary>
    public void Clear()
    {
        item = null;//아이템 정보 초기화
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    /// <summary>
    /// 인벤토리 아이템 클릭시 아이템 정보 출력 메서드
    /// </summary>
    public void OnClickButton()
    {
        inventory.Selectitem(index);
    }
}
