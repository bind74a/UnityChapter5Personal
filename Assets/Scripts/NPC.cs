using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,//임의로 목표지점 지정시 자동으로 움직이게 하는것
    Attacking
}


public class NPC : MonoBehaviour, IDamagalbe
{
    [Header("Stats")]
    public int health;//체력
    public float walkSpeed;//걷는 속도
    public float runSpeed;//뛰는 속도
    public ItemData[] dropOnDeath;//몬스터 드랍 아이템

    [Header("AI")]
    private NavMeshAgent agent; //NavMeshAgent 컴포넌트 변수
    public float detectDistance;//목포지점까지의 최소 거리
    private AIState aiState;//이넘 타입

    [Header("Wandering")] // 이동할때 필요한 값
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;//새로운 목표지점을 찍을때 기다리는시간 랜던값변수 (아마 다음 행동 딜레이)
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;//데미지
    public float attackRate;//공격 딜레이
    private float lastAttackTime;//마지막으로 공격한 시간값
    public float attackDistance;//공격 사거리

    private float playerDistance; //플레이어와의 거리

    public float fieldOfView = 120f;//시야 범위(인식 범위)

    private Animator anim;
    private SkinnedMeshRenderer[] meshRenderers;//몬스터가 갖고 있는 각종 메쉬 들의 정보 배열 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//컴포넌트 연결
        anim = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();//특정 컴포넌트의 자식 객체들까지 포함하여 모두 연결
    }

    void Start()
    {
        SetState(AIState.Wandering);//소환시 다음행동 딜래이(대기 시간)
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position); //플레이어와의 거리값

        anim.SetBool("Moving", aiState != AIState.Idle);//aiState의 값이 Idle 아닐때만 Moving 애니메이션 재생

        switch(aiState)//행동값에 따라 함수 호출예정
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();//기본값
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }
    /// <summary>
    /// 상태 마다 수치설정
    /// </summary>
    /// <param name="state"></param>
    public void SetState(AIState state)
    {
        aiState = state;

        switch(aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;//정지상태의 유무
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

        anim.speed = agent.speed / walkSpeed; // agent.speed 값에 따라 애니메이션 속도 변경
    }
    /// <summary>
    /// 기본 상태 (폭표 지점 탐색)
    /// </summary>
    void PassiveUpdate()
    {
        
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)//remainingDistance 목표 지점까지의 남은 거리
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));//다음 목표 설정
        }

        if(playerDistance < detectDistance)//플레이어의 거리가 가까워질때
        {
            SetState(AIState.Attacking);
        }
    }
    #region 다음 목표 로직
    /// <summary>
    /// 다음 목표 지점 생성
    /// </summary>
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;//몬스터가 대기 상태일때는 실행안돼게 하는 방어코드

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());//다음 목표 지점 세팅
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;//목표 까지의 최단경로 도출
        
        //움직일수 있는 영역 설정 과 목표까지의 최단 거리
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
            out hit, 
            maxWanderDistance, NavMesh.AllAreas); //onUnitSphere 반지름 1인 구체 

        
        int i = 0;
        //do while 문을쓰면 코드 줄이 줄어들수잇슴
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)//다음목표지점과 현재지점과의 거리가 가까울때 다시 경로 도출
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
            out hit,
            maxWanderDistance, NavMesh.AllAreas);

            i++;
            if (i == 30) break;//30번이나 시도햇는데도 거리가 가까우면 그냥 실행
        }

        return hit.position;
    }
    #endregion

    #region 공격 로직
    /// <summary>
    /// 공격 과 추적
    /// </summary>
    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && isPlayerInFieldOfView())//플레이어와의 거리가 가깝고 몬스터 인식 범위 안에 있다면
        {
            agent.isStopped = true;//공격 모션을 위해 정지
            if(Time.time - lastAttackTime > attackRate)//공격 딜레이로 설정한 시간이 지낫는지 체크
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.condition.GetComponent<IDamagalbe>().TakePhysicaIDamage(damage);//데미지 함수 호출
                anim.SetTrigger("Attack");//애니메이션 재생
            }
        }
        else
        {
            if(playerDistance < detectDistance)//플레이어가 도망가면서 거리가 멀어질때 추적
            {
                agent.isStopped= false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))//CalculatePath 는 경로 계산
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);//다음 목표 설정(목표를 플레이어로 지정)
                }
                else//추적중 추적 목표가 이동불가지역으로 갓을때 목표 재설정
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else//추적중지
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }
    /// <summary>
    /// 몬스터 인식 범위
    /// </summary>
    /// <returns></returns>
    bool isPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position; //목표 지점 - 몬스터의 지점 (거리값)
        float angle = Vector3.Angle(transform.forward, directionToPlayer);//플레이어과 몬스터 사이의 각도 값
        return angle < fieldOfView * 0.5f;//fieldOfView 는 120도 0.5를 곱해서 60도 오른쪽 왼쪽 60도씩이기때문에 원래 설정 데로 120도이다
    }

    public void TakePhysicaIDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //죽는다
            Die();
        }

        //데미지 효과
        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        for(int i = 0; i< dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);//아이템 드롭
        }

        Destroy(gameObject);
    }
    #endregion

    /// <summary>
    /// 피격 효과 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 6.0f, 6.0f);

        }
        yield return new WaitForSeconds(0.1f);//0.1초뒤에 다음 코드 실행

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white; //재질의 색을 하얀색으로
        }
    }
    
}
