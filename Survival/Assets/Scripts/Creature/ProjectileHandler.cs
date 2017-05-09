using UnityEngine;
using UnityEngine.Networking;

public class ProjectileHandler : NetworkBehaviour
{

    private Transform target = null;
    private int damage = 0;
    private bool canGo = false;
    private bool playerFired;

    public float speed = 1f;
    public float stopRange = 1;

    [Server]
    void FixedUpdate()
    {
        if (!canGo)
            return;

        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 targetDir = target.position - transform.position;
        if (targetDir.magnitude <= stopRange)//Target hitted.
        {
            if(target)
                target.GetComponent<IDamageable>().DealDamage(damage,playerFired);
            Destroy(this.gameObject);
        }
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

    }

    [Server]
    public void SetTarget(Transform t, int d,bool isPlayer)
    {
        canGo = true;
        target = t;
        damage = d;
        playerFired = isPlayer;
    }
}
