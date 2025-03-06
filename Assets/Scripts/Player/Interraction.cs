using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interraction : MonoBehaviour
{
    public float checkRate = 0.05f; //0.05�ʸ��� ����
    private float lastCheckTime;//������Ʈ�� ���������� ������ �ð�
    public float maxCheckDistance;//������Ʈ ���� ����
    public LayerMask layerMask;//������ ���̾� ����

    public GameObject curInteractGameObject;//��ȣ�ۿ��� ������Ʈ�� ��� ����
    private IInteractable curInteractable;//�������̽� �Լ�

    public TextMeshProUGUI promptText;//������ �޴�â
    private Camera camera;//����ĳ��Ʈ�� ����� ī�޶� ����

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate) //checkRate�� ��ϵ� ������ �ð��� ���������� (������) 
        {
            lastCheckTime = Time.time;

            //ScreenPointToRay ������Ʈ�� �������� ����ĳ��Ʈ����
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //ī�޶� ���߾�
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            /*�۵�����
            ray������ �����Ȱ����� ���� ���� ������ ��ü�� hit ������ ����,
            maxCheckDistance������ ������ ������ ��������,
            layerMask ������ ���̾�
            */
            {
                if (hit.collider.gameObject != curInteractGameObject)//������ ������Ʈ�� ��ȣ�ۿ��� ������Ʈ�� �ٸ���
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();//���⼭ �������̽��� �޼ҵ尪�� �����´�
                    SetPrompText();//������â �������̽�
                }
            }
            else
            {
                //�����Ѱ� ������ ���� ���� �ʱ�ȭ
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPrompText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();//�������̽����� ������ �޼ҵ� ���
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)//������ ��ȣ�ۿ�Ű�� ������ curInteractable������ ������Ʈ������ ��������
        {
            curInteractable.OnInteract();//�������� ������ �÷��̾� ������ �ѱ��

            //��ȣ�ۿ��� �������� �ʱ�ȭ
            curInteractGameObject = null;
            curInteractable = null ;
            promptText.gameObject.SetActive(false);
        }
    }
}
