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
    public LayerMask gronudLeyerMask;//레이 캐스트 플레이어 레이어 예외시키는 변수 생성

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
        Cursor.lockState = CursorLockMode.Locked; //게임 시작시 마우스 커서가 보이지안게된다
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DrawDebugRays();//디버그 레이 
        Move();//Update 함수는 연속적으로 호출하는곳이여서 움직임에 적합한곳
    }

    private void LateUpdate()
    {
        if (canLook)//마우스 커서가 잠길때만 (인벤토리창이 활성화 안됄때만)
        {
            CameraLook();//카메라 시야 메서드
        }
    }
    /// <summary>
    /// 캐릭터 움직임 적용 메서드
    /// </summary>
    void Move()
    {
        Vector3 dir = transform.forward * curMovementtinput.y + transform.right * curMovementtinput.x; //forward, right은 1 * 키입력값 연산뒤 dir 변수에 저장 ( 1 * -1 = -1 이다 )
        dir *= moveSpeed;
        dir.y = rigid.velocity.y; //나중에 중력에 힘을 가할때(점프) 필요한 변수

        rigid.velocity = dir;//입력값을 오브젝트에 적용하는곳
    }
    /// <summary>
    /// 캐릭터 시야값 적용 메서드
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity; //입력값 * 마우스 민감도
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);//camCurXRot 변수의 수치를 최소값, 최대값을 정하는곳

        //시야 각도 지정
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);//수평 x축 원기둥으로 회전
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);//수직 y축 원기둥으로 회전
    }

    /// <summary>
    /// 캐릭터 움직임 연산 메서드
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)//InputAction에서 설정한 키가 눌렷을때 Performed 는 누르고 있어도 작동하는 함수
        {
            curMovementtinput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)//InputAction에서 설정한 키가 떨어졋을때
        {
            curMovementtinput = Vector2.zero;
        }
    }
    /// <summary>
    /// 플레이어 마우스 조작 인식 메서드
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();//마우스는 플레이어의 시야여서 위에 캐릭터 움직임 처럼 조건값이 없다
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())//키가 눌리고 IsGround 메서드에서 true가 나올때
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);//벡터의 y축 값으로 중력의 힘을 가한다
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
            if (Physics.Raycast(rays[i], 0.1f, gronudLeyerMask))//위에 레이에서 gronudLeyerMask 에 설정된 레이어가 닿을시
            {
                return true;//캐릭터가 땅에 있을시
            }
        }
        return false;
    }
    /// <summary>
    /// 인벤토리 열기
    /// </summary>
    /// <param name="context"></param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();//델리게이트로 전달받은 Toggle 함수 실행
            ToggleCursor();
        }
    }
    /// <summary>
    /// 마우스 커서 잠금 매서드
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;//커서가 사용불가능 할때 true
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;//toggle이 (true 일때 None / false일때 Locked) 삼항연산자
        canLook = !toggle;//여기서 반대값이 설정돼면서 canLook true 일때 마우스커서가 잠긴다
    }

    /// <summary>
    /// 위에 레이캐스트를 디버그 레이로 표기 한것
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

        // 각 레이를 디버그로 표시
        foreach (Ray ray in rays)
        {
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red); // 길이 1, 빨간색
        }
    }
}
