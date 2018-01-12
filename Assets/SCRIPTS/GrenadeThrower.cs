using UnityEngine;

public class GrenadeThrower : MonoBehaviour {

    public float throwForce = 40f;
    public GameObject grenadePrefab;
	
    public void ThrowGrenade()
    {
        GameObject grenade =  Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0.8f, 0.7f, 1.5f)* throwForce, ForceMode.VelocityChange);
    }
}
