using UnityEngine;

public class GrenadeDelayer : MonoBehaviour {

    public float delay = 1f;
    float countdown;
    //bool hasExploded = false;
    
    void Start () {
        countdown = delay;
	}
	
	void Update () {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            Explode();
            //hasExploded = true;
        }
	}
   public void Explode()
    {
        DestructibleObj explode  = GetComponent<DestructibleObj>();
        explode.DestroyMech();
        Destroy(gameObject);
    }
}
