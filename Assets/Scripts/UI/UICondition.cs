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
        //����ڵ� 
        CharacterManager.Instance.Player.condition.uiCondition = this;
        //CharacterManager�� ��ϵ��ִ� Player ��������� condition > uiCondition �� ���� ��ũ��Ʈ�� ����
        //�̰����� �÷��̾� ���� �ʿ��� ������� Player ��ũ��Ʈ�� �ѹ� ��ġ�� ���� �����ϱ� ���ϰ� �Ѱ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
