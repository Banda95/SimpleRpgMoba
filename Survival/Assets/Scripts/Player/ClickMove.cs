using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClickMove : NetworkBehaviour
{
    public float speed;
    public GameObject projectile;

    private Animator anim;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetedEnemy;
    
    private BasicStats stats;
    private PlayerController playerController;
    private Vector3 point;
    private InputHandler inputHandler;

    private float distance;
    private bool running;
    private bool idling;
    private bool attacking;
    private bool enemyClicked;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stats = GetComponent<BasicStats>();
        playerController = GetComponent<PlayerController>();
        inputHandler = FindObjectOfType<InputHandler>();
    }

    [Client]
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButton(1) && inputHandler.checkSelection())
            {
                CmdActionRequest();
            }

            CmdMove();
            
            if (enemyClicked)
            {
                CmdGetCloseAndAttack();
            }

            anim.SetBool("isRunning", running);
            anim.SetBool("isIdle", idling);
        }
    }
    [Command]
    private void CmdMove()
    {
        if (point != Vector3.zero)
        {
            navMeshAgent.destination = point;
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                running = false;
                idling = true;
            }
            else
            {
                running = true;
                idling = false;
            }
        }
    }

    [Command]
    private void CmdActionRequest()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Creature")
            {
                if (hit.transform.GetComponent<BasicStats>().Team == stats.Team)
                    return;
                targetedEnemy = hit.transform;
                point = Vector3.zero;
                enemyClicked = true;
                navMeshAgent.destination = GetNearPosition(hit);
                distance = (navMeshAgent.destination - transform.position).magnitude;
            }
            else
            {
                enemyClicked = false;
                targetedEnemy = null;
                point = hit.point;
                navMeshAgent.destination = hit.point;
            }
            running = true;
            idling = false;

            navMeshAgent.Resume();
        }
    }

    [Command]
    private void CmdGetCloseAndAttack()
    {
        if (!targetedEnemy)
        {
            attacking = false;
            running = false;
            idling = true;
            enemyClicked = false;
            return;
        }
        distance = (navMeshAgent.destination - transform.position).magnitude;
        if (distance <= stats.AttackRange)
        {
            navMeshAgent.Stop();
            transform.LookAt(targetedEnemy);
            if (!attacking)
            {
                StartCoroutine(RangedAttack());                     
            }
        }
        else
        {
            navMeshAgent.Resume();
            running = true;
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        running = false;
        int lr = Random.Range(0, 2);
        if (lr == 0)
            anim.SetBool("isLeft", true);
        else
            anim.SetBool("isRight", true);

        yield return new WaitForSeconds(stats.AttackSpeed);
        targetedEnemy.GetComponent<IDamageable>().DealDamage(stats.AttackDamage,true);
        if (lr == 0)
            anim.SetBool("isLeft", false);
        else
            anim.SetBool("isRight", false);
        attacking = false;
        idling = true;
    }

    IEnumerator RangedAttack()
    {
        attacking = true;
        running = false;
        int lr = Random.Range(0, 2);
        if (lr == 0)
            anim.SetBool("isLeft", true);
        else
            anim.SetBool("isRight", true);

        GameObject obj;
        if (!projectile)
            yield break;

        obj = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
        obj.GetComponent<ProjectileHandler>().SetTarget(targetedEnemy, stats.AttackDamage,true);
        NetworkServer.Spawn(obj);
        yield return new WaitForSeconds(stats.AttackSpeed);

        if (lr == 0)
            anim.SetBool("isLeft", false);
        else
            anim.SetBool("isRight", false);
        attacking = false;
        idling = true;
    }

    private Vector3 GetNearPosition(RaycastHit hit)
    {
        Vector3[] points = new Vector3[4];
        BoxCollider r = hit.collider.GetComponentInParent<BoxCollider>();

        points[0] = hit.transform.position; //UP
        points[0].z += r.bounds.size.z;

        points[2] = hit.transform.position; //BOT
        points[2].z -= r.bounds.size.z;

        points[1] = hit.transform.position; //RIGHT
        points[1].x += r.bounds.size.x;

        points[3] = hit.transform.position; //LEFT
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
}
