using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    public Transform target;
    public LayerMask targetLayer;

    private NavMeshAgent agent;
    private Animator monsterAnimator;

    public float traceDist = 10f;   
    public float attackDist = 2f;
    private float attackInterval = 0.5f;
    private float lastAttackTime;
    private float damage;

    private bool isSinking = false;
    private float sinkingSpeed = 2f;

    public AudioClip deathClip;
    public AudioClip hitClip;

    private AudioSource monsterAudioPlayer;

    private Status currentStatus;
    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var preStatus = currentStatus;  
            currentStatus = value;

            switch(currentStatus)
            {
                case Status.Idle:
                    monsterAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    monsterAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    monsterAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    monsterAudioPlayer.PlayOneShot(deathClip);  
                    monsterAnimator.SetTrigger("Die");
                    agent.isStopped = true; 
                    break;
            }
        }
    }

    public void Setup(MonsterData data)
    {
        gameObject.SetActive(false);

        startingHealth = data.maxHP;
        damage = data.damage;
        agent.speed = data.speed;
        
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<Animator>();
        monsterAudioPlayer = GetComponent<AudioSource>();
        
        monsterAnimator.SetBool("HasTarget", true);
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;
        agent.isStopped = false;
        agent.ResetPath();  

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
       // monsterCollider.enabled = true; 
        CurrentStatus = Status.Idle;    
    }

    private void Update()
    {
        if (isSinking)
        {
            transform.Translate(Vector3.down * sinkingSpeed * Time.deltaTime);
            return;
        }

        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttck();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateDie()
    {
    }

    private void UpdateAttck()
    {
        if (target == null || Vector3.Distance(target.position, transform.position) > attackDist)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (Time.time > lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;

            var livingEntity = target.GetComponent<LivingEntity>();
            if (livingEntity != null)
            {
                if (!livingEntity.IsDead)
                {
                    livingEntity.OnDamage(damage, transform.position, -transform.forward);
                }
            }
        }

    }

    private void UpdateTrace()
    {
        if (target == null || Vector3.Distance(target.position, transform.position) > traceDist)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        if (Vector3.Distance(target.position, transform.position) <= attackDist)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(target.position, transform.position) < traceDist)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDist); 

    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer); // 구의 중점, 구의 반경, 타겟 레이어 (복수 개 및 전체 레이어로 설정 가능)
                                                                                               // 특정한 레이어들을 넘겨 받음
        if (colliders.Length == 0) return null;

        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First(); // 제일 앞에 있는 콜라이더 반환

        return target.transform;
    }

    public override void Die()
    {
        base.Die();
        CurrentStatus = Status.Die;
    }
     public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        Debug.Log($"Monster OnDamage: {damage}");
        monsterAudioPlayer.PlayOneShot(hitClip);
    }

    public void StartSinking()
    {
        agent.enabled = false;
        isSinking = true;
        Destroy(gameObject, 2f);    
    }
}
