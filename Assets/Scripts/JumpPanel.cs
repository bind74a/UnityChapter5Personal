using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    Rigidbody nowJump;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            nowJump = collision.gameObject.GetComponent<Rigidbody>();
            nowJump.AddForce(Vector3.up * 160, ForceMode.Impulse);
        }
    }
}
