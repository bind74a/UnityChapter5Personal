using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem; //델리게이트 변수

    public Transform dropPosition;
    private void Awake()
    {
        CharacterManager.Instance.Player = this; //CharacterManager 에게  Player 스크립트의 정보를 CharacterManager안에 있는 Player 변수에 넣는다
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
