using UnityEngine;

[CreateAssetMenu(fileName = "Scin")]
public class ShellSkin : ScriptableObject {

public Sprite Skin;
public int id;

public Modus lol;

public enum Modus
{
Achievement = 0,
Available = 1,
Advertisement = 2,
}
}