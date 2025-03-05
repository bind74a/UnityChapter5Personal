using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    /// <summary>
    /// Ŭ���� �ۿ��� Instance �� ���ٽ� _instance�� null��� 
    /// �� ������Ʈ "CharacterManager" �� ������ CharacterManager ������Ʈ�߰��ؼ� _instance�� �ִ´�
    /// </summary>
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;

    public Player Player { get { return _player; } set { _player = value; } }
    private void Awake()
    {
        //�̱���
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);//���� �̵��� �ı������ʱ�
        }
        /*_instance �� null�� �ƴҽ� 
        [if] ���� CharacterManager �� ���ο� CharacterManager ���Ͽ�
        �ٸ��ٸ� ���� CharacterManager �� �����ϰ� ���ο� CharacterManager�� ��ü�Ѵ� (�̸��� ������Ʈ)
        */
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
