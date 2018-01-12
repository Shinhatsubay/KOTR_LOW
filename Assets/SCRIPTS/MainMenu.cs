using UnityEngine;
using USM = UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {
        USM.SceneManager.LoadScene(USM.SceneManager.GetActiveScene().buildIndex + 1);    
    }

    public void QuitGame()
    {
        //Debug.Log("QuitGame");
        Application.Quit();
    }

}
