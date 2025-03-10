using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public static Coroutine BuffCoroutine(IEnumerator routine)//코루틴 받아서 실행
    {
        return Instance.StartCoroutine(routine);
    }
}
