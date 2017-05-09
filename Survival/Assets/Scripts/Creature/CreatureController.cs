using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(BasicStats))]
public class CreatureController : NetworkBehaviour,IDamageable {
    
    public GameObject projectile;
    public int EyeRadius = 20;

    private bool selected;
    private MeshRenderer plane;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator anim;

    [SyncVar]
    private BasicStats stats;

    private Vector3 destination;
    private Transform targetedEnemy;
    private CreatureManager manager;

    private float distance;
    private bool walking;
    private bool idling;
    private bool attacking;

    private bool moveRequest = false;
    private bool attackRequest = false;

    void Start()
    {
        stats = GetComponent<BasicStats>();
        stats.CurrentHealth = stats.BaseHealth;

        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        selected = false;
        plane = transform.Find("Plane").GetComponent<MeshRenderer>();
        plane.enabled = false;
        navMeshAgent.speed = stats.Speed;
    }

    [Server]
    void FixedUpdate()
    {
        if (moveRequest)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.Stop();
                SetIdle();
                SetAnimationStatus();
                moveRequest = false;
            }
        }
        if (attackRequest)
        {
            if (targetedEnemy)
            {
                navMeshAgent.destination = GetNearPosition(targetedEnemy);
                MoveAndAttack();
            }
            else
            {
                FindNewTarget();
            }
        }
    }

    [Server]
    public void DealDamage(int amount,bool isPlayer)
    {
        stats.CurrentHealth -= amount;
        if(stats.CurrentHealth <= 0)
        {
            if(isPlayer)
            {
                //TODO CrowdFavorGenerator(team not me ) += qualcosa.
            }
            manager.removeCreatureById(stats.UniqueId);
            Destroy(this.gameObject);
        }
        if (!targetedEnemy)
            FindNewTarget();
    }

    public void MoveAndAttack()
    {
        if (!targetedEnemy)
        {
            SetIdle();
            attackRequest = false;
            return;
        }
        distance = (navMeshAgent.destination - transform.position).magnitude;
        if (distance <= stats.AttackRange)
        {
            navMeshAgent.Stop();
            transform.LookAt(targetedEnemy);
            if (!attacking)
            {
                if (projectile)
                    StartCoroutine(RangedAttack());
                else
                    StartCoroutine(Attack());
            }
        }
        else
        {
            navMeshAgent.Resume();
            SetWalk();
        }
        SetAnimationStatus();
    }

    IEnumerator Attack()
    {
        SetAttack();
        SetAnimationStatus();
        if (targetedEnemy)
            targetedEnemy.GetComponent<IDamageable>().DealDamage(stats.AttackDamage,false);
        yield return new WaitForSeconds(stats.AttackSpeed);
      

        SetIdle();
        SetAnimationStatus();
    }

    IEnumerator RangedAttack()
    {
        SetAttack();
        SetAnimationStatus();
        GameObject obj;
        if (!projectile)
            yield break;

        obj = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
        obj.GetComponent<ProjectileHandler>().SetTarget(targetedEnemy, stats.AttackDamage,false);
        yield return new WaitForSeconds(stats.AttackSpeed);

        SetIdle();
        SetAnimationStatus();
    }

    [Command]
    public void CmdSetDestination(Vector3 dest)
    {
        destination = dest;
        moveRequest = true;
        attackRequest = false;
        navMeshAgent.destination = dest;
        distance = (navMeshAgent.destination - transform.position).magnitude;
        SetWalk();
        SetAnimationStatus();
        navMeshAgent.Resume();
    }
    [Command]
    public void CmdSetEnemy(uint id)
    {

        targetedEnemy = manager.getCreatureById(id);

        attackRequest = true;
        moveRequest = false;
    }

    private Vector3 GetNearPosition(Transform t)
    {
        Vector3[] points = new Vector3[4];
        BoxCollider r = t.GetComponentInParent<BoxCollider>();

        points[0] = t.position; //UP
        points[0].z += r.bounds.size.z;

        points[2] = t.position; //BOT
        points[2].z -= r.bounds.size.z;

        points[1] = t.position; //RIGHT
        points[1].x += r.bounds.size.x;

        points[3] = t.position; //LEFT
        points[3].x -= r.bounds.size.x;

        Vector3 closest = points[0];
        float minDistance = (transform.position - closest).magnitude;

        for (int i = 1; i < points.Length; i++)
        {
            float dist = (transform.position - points[i]).magnitude;
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = points[i];
            }
        }

        return closest;
    }

    private void FindNewTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, EyeRadius, LayerMask.GetMask("Unit"));
        //Process all enemy in radius, find the most dangerous threat.
        int minScore = 90000;
        Transform bestMatch = null;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            BasicStats entityStats = hitColliders[i].GetComponentInParent<BasicStats>();
            if (entityStats.Team == stats.Team)
            {
                //Skip teammates or dead ones.
                continue;
            }
            int localScore = 0;
            int dist =(int) Vector3.Distance(transform.position, hitColliders[i].transform.position);
            localScore = dist;
            localScore = entityStats.CurrentHealth;

            if (localScore < minScore)
            {
                minScore = localScore;
                bestMatch = hitColliders[i].transform;
            }
        }

        if (bestMatch)
        {
            targetedEnemy = bestMatch;
            attackRequest = true;
            moveRequest = false;
        }
        else
        {
            targetedEnemy = null;
            attackRequest = false;
        }
    }

    void OnGUI()
    {

        if (!selected)
            return;

        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(transform.position);

        GUI.Box(new Rect(targetPos.x-10, Screen.height - targetPos.y - 80, 60, 20), stats.CurrentHealth + "");

    }

    #region Utilities

    public void Select()
    {
        selected = true;
        plane.enabled = true;
    }

    public void Deselect()
    {
        selected = false;
        plane.enabled = false;
    }

    public int GiveExp()
    {
        return 15;
    }

    private void SetWalk()
    {
        walking = true;
        idling = false;
        attacking = false;
    }

    private void SetIdle()
    {
        walking = false;
        idling = true;
        attacking = false;
    }

    private void SetAttack()
    {
        walking = false;
        idling = false;
        attacking = true;
    }

    private void SetAnimationStatus()
    {
        anim.SetBool("isWalking", walking);
        anim.SetBool("isIdle", idling);
        anim.SetBool("isAttacking", attacking);
    }
    #endregion
}
