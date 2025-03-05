using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    /// <summary>
    /// 클래스 밖에서 Instance 에 접근시 _instance가 null경우 
    /// 새 오브젝트 "CharacterManager" 를 생성뒤 CharacterManager 컴포넌트추가해서 _instance에 넣는다
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
        //싱글톤
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);//씬을 이동시 파괴하지않기
        }
        /*_instance 가 null이 아닐시 
        [if] 현재 CharacterManager 와 새로운 CharacterManager 비교하여
        다르다면 현재 CharacterManager 을 삭제하고 새로운 CharacterManager로 교체한다 (이른바 업데이트)
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
