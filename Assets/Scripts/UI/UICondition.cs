using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;


    // Start is called before the first frame update
    void Start()
    {
        //방어코드 
        CharacterManager.Instance.Player.condition.uiCondition = this;
        //CharacterManager에 등록되있는 Player 오브젝브안 condition > uiCondition 을 현재 스크립트로 지정
        //이과정은 플레이어 에게 필요한 모든기능을 Player 스크립트를 한번 거치고 가서 관리하기 편하게 한것
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
