using UnityEngine;

public class RaycastShooting : MonoBehaviour {

    LogPlayer eventLog;
    
    private Transform target;
    private bool rayDelay = false;

    public Transform PartToRotate;
    public GameObject muzzleFlash;
    public GameObject impactEffect;

    public GameObject Gun;
    public float rayDelayTime = 0.4f;
    public float MinDmg;
    public float MaxDmg;
    public float range;
    public float turnSpeed = 10f;
    public float impactForce;

    private float damage;
    public string enemyTag = "Enemy"; //TODO: Enemy categories, searching later in TargetUpdate() 

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        eventLog = FindObjectOfType<LogPlayer>().GetComponent<LogPlayer>();
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
            target = nearestEnemy.transform;
        else target = null;
    }

	void Update ()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

		//if (Input.GetButtonDown("Fire3"))
        if(!rayDelay && dir.magnitude < range)
        {
            Invoke("Shoot", rayDelayTime);
            rayDelay = true;       
        }
	}

    public void Autofire()
    {
        target = null;
        if (IsInvoking("UpdateTarget"))
            CancelInvoke("UpdateTarget");
        else InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    private float RandomDmg()
    {
        return Random.Range(MinDmg, MaxDmg);
    }

    void Shoot()
    {  
        RaycastHit hit;
        GameObject effectIns = Instantiate(muzzleFlash, transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("Shot1");
        Destroy(effectIns, 1f);
        rayDelay = false;
        if (Physics.Raycast(Gun.transform.position, Gun.transform.forward, out hit, range))
        {      
            DestructibleObj enemy =  hit.transform.GetComponent<DestructibleObj>();

            if (enemy != null)
            {
                damage = (int)RandomDmg();
                eventLog.NewActivity(("You hit " + enemy.name + " for " + damage));
                enemy.DamageIncome(damage);
            }
            if(hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal* impactForce);

            GameObject ImpactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactGO, 1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
