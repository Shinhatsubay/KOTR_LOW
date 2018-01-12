using UnityEngine;
using UnityEngine.UI;

public class DestructibleObj : MonoBehaviour
{
    public float ArmorReduction;
    public float CollisionBonusDamage;
    public float DamageThreshold;
    public Slider HealthSlider;
    public TimeManager timeManager;
    
    public GameObject destroyedVersion;
    public GameObject destroyEff;
    public string AudioEff;
    public Texture2D TextureNormal;
    public Texture2D TextureFlick;
    public Renderer objRenderer;

    LogPlayer eventLog;

    public float objHealth;
    public float objCurHealth;
    public float DestroyDamage = 0f;
    public float DestroyRadius = 0f;
    public float DestroyForce = 0f;
    private float CollDamage;
    private float CollDamageToPlayer;
    private float CollDamageToObj;
    public bool DestroyGrenade = false;
    public bool DestroyPlayer = false;
    public bool isDead;


    private void Start()
    {
        objCurHealth = objHealth;
        eventLog = FindObjectOfType<LogPlayer>().GetComponent<LogPlayer>();
    }

    public void DestroyMech()
    {
        gameObject.SetActive(false);
        objCurHealth = objHealth;
        //gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        if (AudioEff != "")
            FindObjectOfType<AudioManager>().Play(AudioEff);

        if (destroyEff != null)
        {
            GameObject be = Instantiate(destroyEff, transform.position, transform.rotation);
            Destroy(be, 3f);
        }

        if (destroyedVersion != null)
        {
            GameObject obj = Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(obj, 8f);
        }

        if (DestroyDamage != 0)
            DamageOutcome(DestroyDamage);
    }

    public void DamageOutcome(float DestroyDamage)
    {
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, DestroyRadius);
        foreach (Collider nearbyObject in collidersToDestroy)
        {
            DestructibleObj dest = nearbyObject.GetComponent<DestructibleObj>();
            if (dest != null)
                dest.DamageIncome(DestroyDamage);
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, DestroyRadius);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(DestroyForce, transform.position, DestroyRadius);
        }
    }

    public void DamageIncome(float damage)
    {
        if (DestroyPlayer)
        {
            TakeDamage((int)damage);
            eventLog.NewActivity(("Damage taken " + (int)damage));
            return;
        }

        objCurHealth -= damage;
        if (TextureFlick != null)
        {
            objRenderer.material.mainTexture = TextureFlick;
            Invoke("NormalTexture", 0.15f);
        }

        if (objCurHealth <= 0 && DestroyGrenade)
        {
            GrenadeDelayer expl = GetComponent<GrenadeDelayer>();
            expl.Explode();
        }

        if (objCurHealth <= 0)
            DestroyMech();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("LightDebris") || coll.gameObject.CompareTag("Terrain"))
            return;

        //CollDamage = (float)Math.Round(coll.relativeVelocity.magnitude, 0, MidpointRounding.AwayFromZero);
        CollDamage = (int)coll.relativeVelocity.magnitude;

        if (CollDamage < 1)
            return;

        CollDamageToPlayer = CollDamage / ArmorReduction;
        CollDamageToObj = CollDamage + CollisionBonusDamage;

        //Debug.Log("to obj: " + CollDamageToObj);
        //Debug.Log("to player: " + CollDamageToPlayer);
        if (coll.gameObject.CompareTag("Enemy"))
            DamageIncome((int)CollDamageToObj);
        else if (coll.gameObject.CompareTag("Player"))
        {
            DamageIncome((int)CollDamageToObj);
            eventLog.NewActivity(("You hit " + gameObject.name + " for " + (int)CollDamageToObj) + " (Collision)");
        }

        if (DestroyPlayer)
        {
            TakeDamage((int)CollDamageToPlayer);
            eventLog.NewActivity(("Collision dmg taken " + (int)CollDamageToPlayer));
        }
    }

    void NormalTexture()
    {
        objRenderer.material.mainTexture = TextureNormal;
    }

    public void TakeDamage(float damage)
    {
        if (damage >= DamageThreshold)
        {
            objCurHealth -= damage;

            if (HealthSlider != null)
                HealthSlider.value = objCurHealth;

            if (damage > 5)
            {
                objRenderer.material.mainTexture = TextureFlick;
                Invoke("NormalTexture", 0.15f);
            }
            if (damage > 100)
                timeManager.DoSlowmotion();

            if (objCurHealth <= 0 && !isDead)
            {
                isDead = true;
                timeManager.DoSlowmotion();
            }
        }
    }
}

