using UnityEngine;

public class Menu : MonoBehaviour
{

[SerializeField] private GameObject menu;
[SerializeField] private GameObject DeadHandlingZone;

public void OpenVK()
{
Application.OpenURL("https://vk.com/pechhenka");
}

public void SwapActive(bool active)
{
if (active)
{
menu.SetActive(true);
DeadHandlingZone.SetActive(true);
}
else
{

menu.SetActive(false);
DeadHandlingZone.SetActive(false);
}
}
}
