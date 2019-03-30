using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{

[SerializeField] private Sprite OnSound;
[SerializeField] private Sprite OffSound;

private bool state;
private Image image;

private void Start()
{
image = GetComponent<Image>();
if (Data.Instance.sv.Sound)
{
state = true;
image.sprite = OffSound;
}
else
{
state = false;
image.sprite = OnSound;
}
}

public void Click()
{
if (state)
image.sprite = OnSound;
else
image.sprite = OffSound;
state = !state;

}
}
