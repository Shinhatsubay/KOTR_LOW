using UnityEngine;
using USM = UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public CarEngine car;
    public Transform ForCanvas;
    public DestructibleObj playerHealth;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 4f;
    bool paused = false;
    bool pausedA = false;

    public void QuitGame() { Application.Quit(); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerHealth.isDead && !pausedA)
        {
            Pause();
        }

        if (!playerHealth.isDead && !paused && !pausedA)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void Pause()
    {
        if (ForCanvas.gameObject.activeInHierarchy == false)
        {
            ForCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            ForCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }

    public void Restarter()
    {
        // loads current scene
        USM.SceneManager.LoadScene(USM.SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ActivePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pausedA = false;
        }  
        else
        {
            Time.timeScale = 0;
            pausedA = true;
        }
    }  
}



