using UnityEngine;
using UnityEngine.UI;

public class SkinShell : MonoBehaviour {

public ShellSkin ss;

private GuiseChanger gc;

void Start()
{
gc = GameObject.Find("Main Camera").GetComponent<GuiseChanger>();
GetComponent<Button>().onClick.AddListener(Click);
}

void Click()
{
gc.SetSkin(ss.id);
}
}
