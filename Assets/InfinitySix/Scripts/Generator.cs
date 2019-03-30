using UnityEngine;

public class Generator : MonoBehaviour
{
public int StartGenerate = 5;
public int AmountLoops = 30;

[SerializeField] GameObject FinishLoop;

private Transform LastObject;
private uint CountLoops = 0;

private int Level;

int AmountLoopsForLevel(int _level)
{
return (int)(3 * Mathf.Cos(_level / 2) + 28);
}

float Lip()
{
float PlusSin = Level / 7;
if (PlusSin > 1f)
PlusSin = 0.9f;
return (-Mathf.Sin(CountLoops) + PlusSin);
}

public void AddLip()
{
float Power = LastObject.localScale.x * 2;
if (CountLoops < AmountLoops + 1)
{
if (CountLoops != AmountLoops)
{
int type = UnityEngine.Random.Range(0, 7);
LastObject = Instantiate(Data.Instance.Loop[type]).transform;
LastObject.localScale = new Vector2(Power, Power);
LastObject.GetChild(0).eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0.0f, 360.0f));
LastObject.GetComponent<LipController>().Init(Lip());
}
else
Instantiate(FinishLoop).transform.localScale = new Vector2(Power, Power);
}
CountLoops++;
}

public int Init(int level)
{
Level = level;
AmountLoops = AmountLoopsForLevel(level);
uint i = 1;
LastObject = Instantiate(Data.Instance.Loop[0]).transform;
LastObject.localScale = new Vector2(1, 1);
LastObject.GetChild(0).localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-150f, 50f));
LastObject.GetComponent<LipController>().Init(0);
for (; i < StartGenerate; i++)
AddLip();
return AmountLoops + 1;
}
}
