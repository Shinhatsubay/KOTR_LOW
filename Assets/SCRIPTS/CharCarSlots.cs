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

    public int space = 13;

    public List<Character> chars = new List<Character>();

    public void Add (Character charact)
    {
        if (!charact.isDefaultChar)
            if(chars.Count >= space)
            {
                Debug.Log("Not enough space for " + charact.name);
                return;
            }
        chars.Add(charact);
        if(onCharChangedCallback != null)
            onCharChangedCallback.Invoke();
    }

    public void Remove(Character charact)
    {
        chars.Remove(charact);
        if (onCharChangedCallback != null)
            onCharChangedCallback.Invoke();
    }
	
}
