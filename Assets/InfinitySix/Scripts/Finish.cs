using UnityEngine;

public class Finish : MonoBehaviour
{
bool Win = false;

Transform par;
Vector2 NewScale;

private void Start()
{
par = transform.parent;
}

private void Update()
{
if (Win)
par.localScale = Vector2.Lerp(par.localScale, NewScale, Time.deltaTime * 2);
}

public void IAmHere()
{
NewScale = new Vector2(par.localScale.x * 8, par.localScale.x * 8);
Win = true;
}
}
