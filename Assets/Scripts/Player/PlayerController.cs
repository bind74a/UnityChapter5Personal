using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moverment")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementtinput;
    public LayerMask gronudLeyerMask;//���� ĳ��Ʈ �÷��̾� ���̾� ���ܽ�Ű�� ���� ����

    [Header("Look")]
    public Transform cameraContainer;
    public float minLook;
    public float maxLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody rigid;


    //public Action buff;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //buff?.Invoke();
        Cursor.lockState = CursorLockMode.Locked; //���� ���۽� ���콺 Ŀ���� �������ȰԵȴ�
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DrawDebugRays();//����� ���� 
        Move();//Update �Լ��� ���������� ȣ���ϴ°��̿��� �����ӿ� �����Ѱ�
    }

    private void LateUpdate()
    {
        if (canLook)//���콺 Ŀ���� ��涧�� (�κ��丮â�� Ȱ��ȭ �ȉƶ���)
        {
            CameraLook();//ī�޶� �þ� �޼���
        }
    }
    /// <summary>
    /// ĳ���� ������ ���� �޼���
    /// </summary>
    void Move()
    {
        Vector3 dir = transform.forward * curMovementtinput.y + transform.right * curMovementtinput.x; //forward, right�� 1 * Ű�Է°� ����� dir ������ ���� ( 1 * -1 = -1 �̴� )
        dir *= moveSpeed;
        dir.y = rigid.velocity.y; //���߿� �߷¿� ���� ���Ҷ�(����) �ʿ��� ����

        rigid.velocity = dir;//�Է°��� ������Ʈ�� �����ϴ°�
    }
    /// <summary>
    /// ĳ���� �þ߰� ���� �޼���
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity; //�Է°� * ���콺 �ΰ���
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);//camCurXRot ������ ��ġ�� �ּҰ�, �ִ밪�� ���ϴ°�

        //�þ� ���� ����
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);//���� x�� ��������� ȸ��
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);//���� y�� ��������� ȸ��
    }

    /// <summary>
    /// ĳ���� ������ ���� �޼���
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)//InputAction���� ������ Ű�� �������� Performed �� ������ �־ �۵��ϴ� �Լ�
        {
            curMovementtinput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)//InputAction���� ������ Ű�� �������
        {
            curMovementtinput = Vector2.zero;
        }
    }
    /// <summary>
    /// �÷��̾� ���콺 ���� �ν� �޼���
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();//���콺�� �÷��̾��� �þ߿��� ���� ĳ���� ������ ó�� ���ǰ��� ����
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())//Ű�� ������ IsGround �޼��忡�� true�� ���ö�
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);//������ y�� ������ �߷��� ���� ���Ѵ�
        }
    }

    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, gronudLeyerMask))//���� ���̿��� gronudLeyerMask �� ������ ���̾ ������
            {
                return true;//ĳ���Ͱ� ���� ������
            }
        }
        return false;
    }
    /// <summary>
    /// �κ��丮 ����
    /// </summary>
    /// <param name="context"></param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();//��������Ʈ�� ���޹��� Toggle �Լ� ����
            ToggleCursor();
        }
    }
    /// <summary>
    /// ���콺 Ŀ�� ��� �ż���
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;//Ŀ���� ���Ұ��� �Ҷ� true
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;//toggle�� (true �϶� None / false�϶� Locked) ���׿�����
        canLook = !toggle;//���⼭ �ݴ밪�� �����Ÿ鼭 canLook true �϶� ���콺Ŀ���� ����
    }

    /// <summary>
    /// ���� ����ĳ��Ʈ�� ����� ���̷� ǥ�� �Ѱ�
    /// </summary>
    void DrawDebugRays()
    {
        Ray[] rays = new Ray[4]
        {
        new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // �� ���̸� ����׷� ǥ��
        foreach (Ray ray in rays)
        {
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red); // ���� 1, ������
        }
    }
}
