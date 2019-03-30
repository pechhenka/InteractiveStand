using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationController : MonoBehaviour
{
private Animation anim;
private bool Last = true;

private void Start()
{
anim = GetComponent<Animation>();
}

public void PlaySettings()
{
if (anim.isPlaying)
return;
if (Last)
anim.Play("OnSettings");
else
anim.Play("OffSettings");
Last = !Last;
}

public void PlayMode()
{
if (anim.isPlaying)
return;
if (Last)
anim.Play("OnMode");
else
anim.Play("OffMode");
Last = !Last;
}
}
