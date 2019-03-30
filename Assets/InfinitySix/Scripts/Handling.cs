using UnityEngine;
using UnityEngine.EventSystems;

public class Handling : MonoBehaviour, IDragHandler
{
public Ball ball;
[SerializeField] private float Speed;
private float LastResult;
public static float Result;

private Menu menu;

private bool Active = true;
private bool stop = true;

void Start()
{
menu = GetComponent<Menu>();
}

public void Resume()
{
stop = true;
}

public void Stop()
{
stop = false;
}

void IDragHandler.OnDrag(PointerEventData eventData)
{
if (Active)
{
ball.StartHandling();
menu.SwapActive(false);
}
Active = false;
if (stop)
{
Result = eventData.delta.x * Speed;
if (LastResult == Result)
Result = 0.0f;
LastResult = Result;
}
}

private void LateUpdate()
{
Result = 0.0f;
}

}
