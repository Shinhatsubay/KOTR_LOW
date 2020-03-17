using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class YOUTUBE : MonoBehaviour
{
    public const string audioName = "https://www.youtube.com/watch?v=eEBaO8l83n0";
    float start_time = 0;
    public AudioClip stream_music;
    //public AudioClip musicSource;

    public IEnumerator LoadAudio(string audioName, float start_time)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(
                                        //musicPath.Replace("#", "%23") +
                                        //musicPath + 
                                        audioName, AudioType.OGGVORBIS))
        {

            var synRes = www.SendWebRequest();
            yield return synRes;

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
               yield return stream_music = DownloadHandlerAudioClip.GetContent(www);

                //musicSource.clip = stream_music;
               //musicSource.time = start_time;
                ////current_track_num = track_id;
                ////current_track = GetCurrentTrack();
                //musicSource.Play();
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
       // LoadAudio("https://www.youtube.com/watch?v=eEBaO8l83n0", 0); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
