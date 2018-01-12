using System.Collections.Generic;
using UnityEngine;

public class LogPlayer : MonoBehaviour
{
    public int maxLines = 6;
    private Queue<string> queue = new Queue<string>();
    private string Mytext = "";

    public void NewActivity(string activity)
    {
        if (queue.Count >= maxLines)
            queue.Dequeue();

        queue.Enqueue(activity);

        Mytext = "";
        foreach (string st in queue)
            Mytext = Mytext + st + "\n";
    }


    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 320, // x, left offset // was 5f
                     (Screen.height - 120), // y, bottom offset
                     300,// width
                     100), Mytext, GUI.skin.textArea); // height, text, Skin features
    }
}


