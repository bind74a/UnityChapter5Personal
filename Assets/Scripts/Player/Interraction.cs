using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interraction : MonoBehaviour
{
    public float checkRate = 0.05f; //0.05초마다 감지
    private float lastCheckTime;//오브젝트를 마지막으로 감지한 시간
    public float maxCheckDistance;//오브젝트 감지 범위
    public LayerMask layerMask;//감지할 레이어 변수

    public GameObject curInteractGameObject;//상호작용할 오브젝트가 담길 변수
    private IInteractable curInteractable;//인터페이스 함수

    public TextMeshProUGUI promptText;//아이템 메뉴창
    private Camera camera;//레이캐스트를 사용할 카메라 변수

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate) //checkRate에 등록된 값보다 시간이 지나갈때만 (딜레이) 
        {
            lastCheckTime = Time.time;

            //ScreenPointToRay 오브젝트의 기준으로 레이캐스트생성
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //카메라 정중앙
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            /*작동원리
            ray변수로 설정된값으로 레이 생성 감지된 물체를 hit 변수에 저장,
            maxCheckDistance변수의 값으로 레이의 길이지정,
            layerMask 감지할 레이어
            */
            {
                if (hit.collider.gameObject != curInteractGameObject)//감지한 오브젝트가 상호작용할 오브젝트랑 다를시
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();//여기서 인터페이스의 메소드값을 가져온다
                    SetPrompText();//아이템창 인터페이스
                }
            }
            else
            {
                //감지한게 없을때 감지 상태 초기화
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPrompText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();//인터페이스에서 지정한 메소드 출력
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)//지정된 상호작용키를 누르고 curInteractable변수에 오브젝트정보가 있을떄만
        {
            curInteractable.OnInteract();//아이템의 정보를 플레이어 쪽으로 넘긴다

            //상호작용후 감지상태 초기화
            curInteractGameObject = null;
            curInteractable = null ;
            promptText.gameObject.SetActive(false);
        }
    }
}
