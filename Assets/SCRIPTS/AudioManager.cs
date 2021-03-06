﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//To find sound in AudioManager use this
//FindObjectOfType<AudioManager>().Play("PlayerDeath");
//
public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public Sound[] soundsYoutube;
    public static AudioManager instance;

	// Like Start() but before Start()
	void Awake () {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        //foreach (Sound s in soundsYoutube)
        //{
        //    s.source = gameObject.AddComponent<AudioSource>();

        //    s.source.clip = FindObjectOfType<YOUTUBE>().LoadAudio("https://www.youtube.com/watch?v=eEBaO8l83n0", 0);


        //    s.source.clip = s.clip;

        //    s.source.volume = s.volume;
        //    s.source.pitch = s.pitch;
        //    s.source.loop = s.loop;
        //}
    }

    private void Start()
    {
        Play("BGmusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }          
        s.source.Play();
    }
}

