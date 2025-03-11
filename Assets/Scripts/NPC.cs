using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,//���Ƿ� ��ǥ���� ������ �ڵ����� �����̰� �ϴ°�
    Attacking
}


public class NPC : MonoBehaviour, IDamagalbe
{
    [Header("Stats")]
    public int health;//ü��
    public float walkSpeed;//�ȴ� �ӵ�
    public float runSpeed;//�ٴ� �ӵ�
    public ItemData[] dropOnDeath;//���� ��� ������

    [Header("AI")]
    private NavMeshAgent agent; //NavMeshAgent ������Ʈ ����
    public float detectDistance;//�������������� �ּ� �Ÿ�
    private AIState aiState;//�̳� Ÿ��

    [Header("Wandering")] // �̵��Ҷ� �ʿ��� ��
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;//���ο� ��ǥ������ ������ ��ٸ��½ð� ���������� (�Ƹ� ���� �ൿ ������)
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;//������
    public float attackRate;//���� ������
    private float lastAttackTime;//���������� ������ �ð���
    public float attackDistance;//���� ��Ÿ�

    private float playerDistance; //�÷��̾���� �Ÿ�

    public float fieldOfView = 120f;//�þ� ����(�ν� ����)

    private Animator anim;
    private SkinnedMeshRenderer[] meshRenderers;//���Ͱ� ���� �ִ� ���� �޽� ���� ���� �迭 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//������Ʈ ����
        anim = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();//Ư�� ������Ʈ�� �ڽ� ��ü����� �����Ͽ� ��� ����
    }

    void Start()
    {
        SetState(AIState.Wandering);//��ȯ�� �����ൿ ������(��� �ð�)
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position); //�÷��̾���� �Ÿ���

        anim.SetBool("Moving", aiState != AIState.Idle);//aiState�� ���� Idle �ƴҶ��� Moving �ִϸ��̼� ���

        switch(aiState)//�ൿ���� ���� �Լ� ȣ�⿹��
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();//�⺻��
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }
    /// <summary>
    /// ���� ���� ��ġ����
    /// </summary>
    /// <param name="state"></param>
    public void SetState(AIState state)
    {
        aiState = state;

        switch(aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;//���������� ����
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        anim.speed = agent.speed / walkSpeed; // agent.speed ���� ���� �ִϸ��̼� �ӵ� ����
    }
    /// <summary>
    /// �⺻ ���� (��ǥ ���� Ž��)
    /// </summary>
    void PassiveUpdate()
    {
        
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)//remainingDistance ��ǥ ���������� ���� �Ÿ�
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));//���� ��ǥ ����
        }

        if(playerDistance < detectDistance)//�÷��̾��� �Ÿ��� ���������
        {
            SetState(AIState.Attacking);
        }
    }
    #region ���� ��ǥ ����
    /// <summary>
    /// ���� ��ǥ ���� ����
    /// </summary>
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;//���Ͱ� ��� �����϶��� ����ȵŰ� �ϴ� ����ڵ�

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());//���� ��ǥ ���� ����
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;//��ǥ ������ �ִܰ�� ����
        
        //�����ϼ� �ִ� ���� ���� �� ��ǥ������ �ִ� �Ÿ�
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
            out hit, 
            maxWanderDistance, NavMesh.AllAreas); //onUnitSphere ������ 1�� ��ü 

        
        int i = 0;
        //do while �������� �ڵ� ���� �پ����ս�
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)//������ǥ������ ������������ �Ÿ��� ����ﶧ �ٽ� ��� ����
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
            out hit,
            maxWanderDistance, NavMesh.AllAreas);

            i++;
            if (i == 30) break;//30���̳� �õ��޴µ��� �Ÿ��� ������ �׳� ����
        }

        return hit.position;
    }
    #endregion

    #region ���� ����
    /// <summary>
    /// ���� �� ����
    /// </summary>
    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && isPlayerInFieldOfView())//�÷��̾���� �Ÿ��� ������ ���� �ν� ���� �ȿ� �ִٸ�
        {
            agent.isStopped = true;//���� ����� ���� ����
            if(Time.time - lastAttackTime > attackRate)//���� �����̷� ������ �ð��� �������� üũ
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.condition.GetComponent<IDamagalbe>().TakePhysicaIDamage(damage);//������ �Լ� ȣ��
                anim.SetTrigger("Attack");//�ִϸ��̼� ���
            }
        }
        else
        {
            if(playerDistance < detectDistance)//�÷��̾ �������鼭 �Ÿ��� �־����� ����
            {
                agent.isStopped= false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))//CalculatePath �� ��� ���
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);//���� ��ǥ ����(��ǥ�� �÷��̾�� ����)
                }
                else//������ ���� ��ǥ�� �̵��Ұ��������� ������ ��ǥ �缳��
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else//��������
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }
    /// <summary>
    /// ���� �ν� ����
    /// </summary>
    /// <returns></returns>
    bool isPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position; //��ǥ ���� - ������ ���� (�Ÿ���)
        float angle = Vector3.Angle(transform.forward, directionToPlayer);//�÷��̾�� ���� ������ ���� ��
        return angle < fieldOfView * 0.5f;//fieldOfView �� 120�� 0.5�� ���ؼ� 60�� ������ ���� 60�����̱⶧���� ���� ���� ���� 120���̴�
    }

    public void TakePhysicaIDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //�״´�
            Die();
        }

        //������ ȿ��
        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        for(int i = 0; i< dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);//������ ���
        }

        Destroy(gameObject);
    }
    #endregion

    /// <summary>
    /// �ǰ� ȿ�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 6.0f, 6.0f);

        }
        yield return new WaitForSeconds(0.1f);//0.1�ʵڿ� ���� �ڵ� ����

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white; //������ ���� �Ͼ������
        }
    }
    
}
