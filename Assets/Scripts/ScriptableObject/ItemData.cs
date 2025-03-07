using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType//������ ����
{
    Equipable,//��� ������
    Consumable,//�Һ� ������
    Resource//�ڿ�, ��Ÿ ������
}

public enum ConsumableType//�Һ� ������ ����
{
    Health,//ü�� ȸ�� ������
    Hunger//�Ű��� ȸ�� ������
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;//�Һ� ������ ����
    public float value; //�Һ� ������ ��� ��ȭ�� 
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]//������ ������ ���� �ɼ� �޴�â���߰� 

public class ItemData : ScriptableObject
{
    [Header("Info")] //������ ����
    public string displayName;//������ �̸�
    public string description;//������ ����
    public ItemType type;//������ ����
    public Sprite icon;
    public GameObject dropPrefab;//������ ������ ����ϴ°� 

    [Header("Stacking")]//������ ���� �ִ� ���� ����
    public bool canStack;//������ �������ִ� �������ΰ�?
    public int maxStackAmount;//������ ���� �ִ� ����

    [Header("Consumable")]
    public ItemDataConsumable[] Consumables;//������ ȸ�� ��ġ

    [Header("Equip")]
    public GameObject equipPrefab;//����� ������ ������ ����
}
