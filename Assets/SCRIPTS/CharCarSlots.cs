using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCarSlots : MonoBehaviour {

#region Singleton

    public static CharCarSlots instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of CharCarSlots found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnCharChanged();
    public OnCharChanged onCharChangedCallback;

    public int space = 10;
    //public List<Character> StartChars = new List<Character>();
    public List<Character> chars = new List<Character>();

    //private void Start()
    //{

    //    for (int i = 0; i < StartChars.Count; i++)
    //    {
    //        Add(StartChars[i]);
    //        Debug.Log("Added " + StartChars[i].name + " to ");
            
    //    }

    //}

    public void Add (Character charact)
    {
        if (!charact.isDefaultChar)
            if(chars.Count >= space)
            {
                Debug.Log("Not enough space for " + charact.name);
                return;
            }
        chars.Add(charact);
        
        if (onCharChangedCallback != null)
            onCharChangedCallback.Invoke();
    }

    public void Remove(Character charact)
    {
        chars.Remove(charact);
        if (onCharChangedCallback != null)
            onCharChangedCallback.Invoke();
    }

    public void Change(Character charact)
    {
        Debug.Log("Changing!");
    }


}
