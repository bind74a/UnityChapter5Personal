using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType//아이템 종류
{
    Equipable,//장비 아이템
    Consumable,//소비 아이템
    Resource//자원, 기타 아이템
}

public enum ConsumableType//소비 아이템 종류
{
    Health,//체력 회복 아이템
    Hunger//매고픔 회복 아이템
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;//소비 아이템 종류
    public float value; //소비 아이템 사용 변화값 
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]//아이템 데이터 생성 옵션 메뉴창에추가 

public class ItemData : ScriptableObject
{
    [Header("Info")] //아이템 설정
    public string displayName;//아이템 이름
    public string description;//아이템 설명
    public ItemType type;//아이템 종류
    public Sprite icon;
    public GameObject dropPrefab;//아이템 프리펩 등록하는곳 

    [Header("Stacking")]//아이템 묶음 최대 개수 설정
    public bool canStack;//여러개 가질수있는 아이템인가?
    public int maxStackAmount;//아이템 묶음 최대 개수

    [Header("Consumable")]
    public ItemDataConsumable[] Consumables;//아이템 회복 수치

    [Header("Equip")]
    public GameObject equipPrefab;//장비할 아이템 프리펩 연결
}
