using UnityEngine;

public class DestroySkidMarks : MonoBehaviour {

    float destroyAfter = 5f;
    float Timer = 0;

	void Update () {

        Timer += Time.deltaTime;
        if(destroyAfter <= Timer)
        {
            Destroy(gameObject);
        }
	}
}
