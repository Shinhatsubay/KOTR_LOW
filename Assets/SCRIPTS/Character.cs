using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class Character : ScriptableObject {

    new public string name = "New Char";
    public Sprite icon = null;
    public bool isDefaultChar = false;

}
