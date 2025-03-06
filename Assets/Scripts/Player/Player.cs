using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem; //��������Ʈ ����

    public Transform dropPosition;
    private void Awake()
    {
        CharacterManager.Instance.Player = this; //CharacterManager ����  Player ��ũ��Ʈ�� ������ CharacterManager�ȿ� �ִ� Player ������ �ִ´�
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
