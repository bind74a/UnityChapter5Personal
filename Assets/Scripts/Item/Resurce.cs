using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurce : MonoBehaviour
{
    public ItemData itemToGive;//오브젝트를 상호작용시 플레이어에게 주는 아이템정보
    public int quantityPerHit = 1;//나오는 아이템의 개수
    public int capacy;//총 몇번 때릴수 있는지

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacy <= 0) break;
            capacy -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));//자원 캘때마다 itemToGive변수에 저장된 프리펩을 복제해서 드랍
        }
    }
}
