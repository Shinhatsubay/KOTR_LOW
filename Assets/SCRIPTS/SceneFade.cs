using UnityEngine;

public class SceneFade : MonoBehaviour {

    public Texture2D FadeOutTexture;
    public float FadeSpeed = 0.8f;
    public int DrawDepth = -1000; //low = render on top
    private float alpha = 1.0f;
    private int FadeDir = -1;

    private void OnGUI()
    {
        alpha += FadeDir * FadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = DrawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeOutTexture);
    }

    public float BeginFade (int direction){
        FadeDir = direction;
        return (FadeSpeed);
    }

    //void OnLevelWasLoaded()
    //{
    //    BeginFade(-1);
    //}




}
