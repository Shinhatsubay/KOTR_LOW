using UnityEngine;
using UnityEngine.UI;

public class Disable : MonoBehaviour
{
    private bool Cooldown = false;
    public Button ButtR;
    public float Cooldwn = 0f; 

    public void OnMouseDown()
    {
        if (Cooldown == false)
        {
            ButtR.interactable = false;
            Invoke("ResetCooldown", Cooldwn);
            Cooldown = true;
        }
    }

    void ResetCooldown()
    {
        Cooldown = false;
        ButtR.interactable = true;
    }

}

