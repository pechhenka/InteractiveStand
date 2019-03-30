using UnityEngine;
using UnityEngine.UI;

public class GuiseChanger : MonoBehaviour
{

private Color Ledge = Color.HSVToRGB(349f / 360f, 0.83f, 1);
private Color UIColor = Color.HSVToRGB(180f / 360f, 0.83f, 1);

public Animation AchievementPanel;
public Text AchievementNameBall;
public Image AchievementPanelImage;
public Text blablabla;

public Text NameSkin;
public Text DescriptionSkin;

public ShellSkin[] Balls;
public Transform[] RingsSkins;
public AnimationCurve SC;
public float mnSC;
public float TimeSC;

private RectTransform ActiveRingSkin;

[Space(10)]
public SpriteRenderer BallSprite;
public Image[] imageColor;
public Text[] TextColor;
public static Color ScoreColor;

public int Level;

public GameObject AD_BUTTON;

private int ActiveSkin;
private int KostalActiveSkin;

private static System.Object _lock = new System.Object();

private void Start()
{
AD_BUTTON.SetActive(false);
Level = PlayerPrefs.GetInt("Level", 1) - 1;
ActiveSkin = PlayerPrefs.GetInt("Skin", 0);
BallSprite.sprite = Balls[ActiveSkin].Skin;
KostalActiveSkin = ActiveSkin;

RefreshSkin();

if (Level > 0)
{
float HLedge = (Level * 31);
Ledge = Color.HSVToRGB(HLedge % 360 / 360f, 0.83f, 1f);
UIColor = Color.HSVToRGB((180 + HLedge) % 360 / 360f, 0.83f, 1f);
}

RingsSkins[ActiveSkin].GetComponent<Image>().color = UIColor;
NameSkin.text = Data.Instance.multilingual.NameSkin[ActiveSkin];
DescriptionSkin.text = Data.Instance.multilingual.Description[ActiveSkin];

Data.Instance.Triangle.color = Ledge;
Data.Instance.Ledge.color = Ledge;
Data.Instance.DestroyLedge.color = Ledge;

Data.Instance.DestroyLedge.SetColor("_EdgeColor", UIColor);
Data.Instance.DestroyStand.SetColor("_EdgeColor", UIColor);

foreach (var im in imageColor)
im.color = UIColor;
foreach (var im in TextColor)
im.color = UIColor;
ScoreColor = UIColor;

}

void Update()
{
if (!RingsSkins[ActiveSkin].gameObject.activeInHierarchy)
return;

float sc = 180;
if (Time.time % (TimeSC * 2) > TimeSC)
sc += mnSC * SC.Evaluate(1 - Time.time % TimeSC / TimeSC);
else
sc += mnSC * SC.Evaluate(Time.time % TimeSC / TimeSC);


RingsSkins[ActiveSkin].GetComponent<RectTransform>().sizeDelta = new Vector2(sc, sc);
}

private void RefreshSkin()
{
for (int i = 0; i < 9; i++)
{
if (Data.Instance.sv.UnlockSkin[i])
{
int c = RingsSkins[i].childCount;
if (c > 1)
{
Transform fog = RingsSkins[i].GetChild(1);
if (fog != null)
Destroy(fog.gameObject);
}
if (c > 2)
{
Transform block = RingsSkins[i].GetChild(2);
if (block != null)
Destroy(block.gameObject);
}
}
}
}

public void GetAchievement(int id)
{
lock (_lock)
{
if (!Data.Instance.sv.UnlockSkin[id])
{
Data.Instance.sv.UnlockSkin[id] = true;
Data.Instance.sv.SaveSkin();
RefreshSkin();

AchievementNameBall.text = Data.Instance.multilingual.NameSkin[id];
AchievementPanelImage.sprite = Balls[id].Skin;
blablabla.text = Data.Instance.multilingual.blablabla;
AchievementPanel.Play();
}
}
}

public void SetSkin(int id)
{
NameSkin.text = Data.Instance.multilingual.NameSkin[id];
DescriptionSkin.text = Data.Instance.multilingual.Description[id];

if ((id == 2 || id == 5) && !Data.Instance.sv.UnlockSkin[id])
AD_BUTTON.SetActive(true);
else
AD_BUTTON.SetActive(false);

if (Data.Instance.sv.UnlockSkin[id])
{
RingsSkins[KostalActiveSkin].GetComponent<Image>().color = Color.white;
RingsSkins[id].GetComponent<Image>().color = UIColor;
PlayerPrefs.SetInt("Skin", id);
ActiveSkin = id;
KostalActiveSkin = id;
BallSprite.sprite = Balls[id].Skin;
}
else
{
ActiveSkin = id;
}

}

public void WatchAdComplete()
{
AD_BUTTON.SetActive(false);
GetAchievement(ActiveSkin);
}
}
