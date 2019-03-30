using Stand;
using UnityEngine;
//using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("TimerOnGame")]
    public Text TimerOnGame;

    [Header("Parameters")]
    public int BorderSeries = 2;
    public int BorderAdsCount = 2;

    [Header("Audio Clips")]
    public AudioClip ClipFinish;
    public AudioClip ClipLedge;
    public AudioClip ClipSpan;
    public AudioClip ClipStand;
    public AudioClip ClipPenetration;

    [Header("Particle Systems")]
    public ParticleSystem PSDead;
    public ParticleSystem PSPower;
    public ParticleSystem PSHit;

    [Header("[UI]")]
    public GameObject SliderBall;
    public Slider SliderLevel;
    public Text LeftCircleSlider;
    public Text RightCircleSlider;
    public Text Score;
    public Image BestScore;
    public Image Ring;

    [Header("Components&Prefabs")]
    [SerializeField] Handling handling;
    [SerializeField] Generator generator;
    public SpriteRenderer sp;
    public Animation SPAnim;
    public GameObject GOPowerScore;
    public GameObject ResurrectionGO;
    public GuiseChanger guiseChanger;

    public Image HealthImage;
    public Sprite HealthNoADS;

#if UNITY_ANDROID
    readonly string GooglePlayStore = "2805136";
#elif UNITY_IOS
    readonly string AppleAppStore = "2805134";
#endif

    int CountGames = 0;
    bool ClickResurrection = false;
    Transform psTransform;

    bool Sound = true;

    GameObject DeadLoop;

    uint CountLips = 0;
    int AmountLips;
    int PowerScore = 1;
    int Series = 0;
    int Level = 1;

    int score = 0;
    int bestScore;

    public static Transform Canvas;

    Viewer viewController;
    Rigidbody2D rb;
    AudioSource _as;

    bool IAmDead = false;
    float TimeDeath = 0f;
    int DestroyRingsRaw = 0;
    bool TouchRing = false;

    float IEResurrectionStartTime;
    bool CompleteTimeIEResurrection = false;

    float IERespawnStartTime;
    float IERespawnTimeSize;
    bool CompleteTimeIERespawn = false;

    public void FixedUpdate()
    {
        if (ApplicationController.Instance.StartTimeInGame + ApplicationController.Instance.TimeInGame > Time.time)
        {
            TimerOnGame.text = "" + (int)(ApplicationController.Instance.TimeInGame - (Time.time - ApplicationController.Instance.StartTimeInGame));
        }
        else
        {
            TimerOnGame.text = "";
        }
    }

    void Start()
    {
        Load();

       /*if (!Data.Instance.sv.NoADSpurchased && Advertisement.isSupported && !Advertisement.isInitialized)
#if UNITY_ANDROID
            Advertisement.Initialize(GooglePlayStore);
#elif UNITY_IOS
            Advertisement.Initialize(AppleAppStore);
#endif
*/
        if (Level > 13)
            guiseChanger.GetAchievement(8);

        if (Level > 1)
            guiseChanger.GetAchievement(7);

        int[] DIG = Data.Instance.sv.DayInGame;
        if (DIG[0] + 1 == DIG[1] && DIG[1] + 1 == DIG[2])
            guiseChanger.GetAchievement(4);
        else
        {
            int now = System.DateTime.Now.Year * 365 + System.DateTime.Now.Month * 30 + System.DateTime.Now.Day;
            if (DIG[2] != now)
                for (int i = 0; i < 2; i++)
                    DIG[i] = DIG[i + 1];
            DIG[2] = now;
        }
        Data.Instance.sv.DayInGame = DIG;

       /*if (!Data.Instance.sv.NoADSpurchased)
            Advertisement.Banner.Show("banner");
        else
            Advertisement.Banner.Hide(true);*/
    }

    void Update()
    {
        if (CompleteTimeIEResurrection)
            if (Time.time - IEResurrectionStartTime > 1)
            {
                CompleteTimeIEResurrection = false;
                ResurrectionGO.SetActive(true);
                IAmDead = true;
                TimeDeath = Time.time;
            }

        if (CompleteTimeIERespawn)
            if (Time.time - IERespawnStartTime > IERespawnTimeSize)
            {
                CompleteTimeIERespawn = false;
               /*  if (!Data.Instance.sv.NoADSpurchased && (CountGames > BorderAdsCount) && Advertisement.IsReady("video"))
                {
                    CountGames = 0;
                    Advertisement.Show("video");
                }*/
                Data.Instance.sv.CountGames = CountGames;
                SceneManager.LoadScene(2);
            }

        if (IAmDead)
        {
            float delta = Time.time - TimeDeath;
            Ring.fillAmount = 1f - (delta / 5f);
            if (delta > 5f)
                Respawn(0);
        }
    }

    public void BuyNoAds()
    {
        ApplicationController.Instance.QuitGame();
    }

    public void RestorePurchases()
    {
        //IAPmanager.Instance.RestorePurchases();
    }

    public void StartHandling()
    {
        SliderBall.SetActive(false);
        Score.text = "0";
        BestScore.enabled = false;
        TouchRing = false;
    }

    public void SwapSound()
    {
        _as.mute = Sound;
        Data.Instance.sv.Sound = Sound;
        Sound = !Sound;
    }

    public void ButSkipTimeResurrection()
    {
        TimeDeath -= 1.6f;
    }

    public void ButResurrection()
    {
        if (Data.Instance.sv.NoADSpurchased)
        {
            TouchRing = false;
            ClickResurrection = true;
            ResurrectionGO.SetActive(false);
            IAmDead = false;
            handling.Resume();
            Transform loop = Instantiate(Data.Instance.Loop[0]).transform;
            loop.localScale = DeadLoop.transform.localScale;
            loop.GetComponent<LipController>().Init(0);
            Destroy(DeadLoop);
            rb.simulated = true;
        }
        else
        {
            /* var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);*/
        }
    }

    public void WatchADSkin()
    {
       /*var options = new ShowOptions { resultCallback = ADSkin };
        Advertisement.Show("buySkin", options);*/
    }
/*
    private void ADSkin(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            guiseChanger.WatchAdComplete();
        }
    }
    private void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            TouchRing = false;
            ClickResurrection = true;
            ResurrectionGO.SetActive(false);
            IAmDead = false;
            handling.Resume();
            Transform loop = Instantiate(Data.Instance.Loop[0]).transform;
            loop.localScale = DeadLoop.transform.localScale;
            loop.GetComponent<LipController>().Init(0);
            Destroy(DeadLoop);
            rb.simulated = true;
        }
    }
*/
    void Load()
    {
        Canvas = GameObject.Find("[UI]").transform;
        viewController = GameObject.FindWithTag("MainCamera").GetComponent<Viewer>();
        rb = GetComponent<Rigidbody2D>();
        _as = GetComponent<AudioSource>();

        CountGames = Data.Instance.sv.CountGames;
        CountGames++;

        Level = Data.Instance.sv.Level;
        LeftCircleSlider.text = Level.ToString();
        RightCircleSlider.text = (Level + 1).ToString();
        SliderLevel.value = 0;

        if (Level > 1)
            SliderBall.SetActive(false);

        Sound = Data.Instance.sv.Sound;
        SwapSound();

        Normalize();

        AmountLips = generator.Init(Level);
        bestScore = Data.Instance.sv.BestScore;
        Score.text = bestScore.ToString();
    }

    void OnLedge(GameObject go)
    {
        TouchRing = true;
        rb.velocity = new Vector2(0, 6.5f * Mathf.Pow(2, CountLips % 8));
        handling.Stop();
        Normalize();
        PSDead.Play();
        _as.PlayOneShot(ClipLedge);
        handling.Stop();
        rb.simulated = false;
        DeadLoop = go;
        if (/*(Advertisement.IsReady("rewardedVideo") || */Data.Instance.sv.NoADSpurchased/*)*/ && !ClickResurrection)
        {
            IEResurrection();
        }
        else
        {
            Respawn();
        }
    }

    void Respawn(float time = 1.2f)
    {
        IERespawn(time);
    }

    void OnFinish()
    {
        Normalize();
        _as.PlayOneShot(ClipFinish);
        handling.Stop();
        rb.simulated = false;
        Level++;
        if (score > bestScore)
            Data.Instance.sv.BestScore = score;
        if (!TouchRing)
            guiseChanger.GetAchievement(3);
        Data.Instance.sv.Level = Level;
        Respawn();
    }

    void OnStand(bool Sound = true)
    {
        TouchRing = true;
        Normalize();
        PSHit.Play();
        if (Sound)
            _as.PlayOneShot(ClipStand);
    }


    void OnHole()
    {
        _as.PlayOneShot(ClipSpan);
        Series++;
        if (Series > BorderSeries)
        {
            if (Series > 8)
                guiseChanger.GetAchievement(1);
            PSPower.Play();
            sp.color = Data.Instance.Ledge.color;
        }
        NewScale();
    }

    void NewScale()
    {
        IncreaseScore();
        if (CountLips % 8 == 0)
        {
            transform.position /= 256;
            rb.gravityScale /= 128;
            rb.velocity /= 256;
            transform.localScale /= 128;
            viewController.NewScale();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Lip"))
                go.transform.localScale /= 256;
        }
        else
        {
            transform.localScale *= 2;
            GetComponent<Rigidbody2D>().gravityScale *= 2;
        }
        if (score > bestScore)
            BestScore.enabled = true;
        generator.AddLip();
    }

    void IncreaseScore()
    {
        PowerScore += Level > 500 ? 500 : Level;
        Text ScorePower = Instantiate(GOPowerScore, Canvas).GetComponent<Text>();
        ScorePower.color = GuiseChanger.ScoreColor;
        ScorePower.text = "+" + PowerScore;
        score += PowerScore;
        Score.text = score.ToString();
        CountLips++;
        SliderLevel.value = CountLips / (AmountLips * 1.0f);
    }

    void Normalize()
    {
        PSPower.Stop();
        sp.color = Color.white;
        Series = 0;
        PowerScore = 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        LipController lp = collision.transform.parent.GetComponent<LipController>();
        if (!lp)
            lp = collision.transform.parent.parent.GetComponent<LipController>();
        switch (lp.Fallen())
        {
            case TypeStand.Ledge:
                if (Series > BorderSeries)
                {
                    DestroyRingsRaw++;
                    SPAnim.Play();
                    _as.PlayOneShot(ClipPenetration);
                    lp.Destroy(); //!!!
                    //Destroy(collision.transform.parent.gameObject);
                    rb.velocity = new Vector2(0, 6.5f * Mathf.Pow(2, CountLips % 8));
                    NewScale();
                    OnStand(false);
                }
                else
                {
                    DestroyRingsRaw = 0;
                    OnLedge(collision.transform.parent.gameObject);
                }
                break;
            case TypeStand.Hole:
                lp.Destroy();//!!!
                //Destroy(collision.transform.parent.gameObject);
                OnHole();
                break;
            case TypeStand.Stand:
                SPAnim.Play();
                rb.velocity = new Vector2(0, 6.5f * Mathf.Pow(2, CountLips % 8));
                if (Series > BorderSeries)
                {
                    DestroyRingsRaw++;
                    _as.PlayOneShot(ClipPenetration);
                    lp.Destroy();
                    //Destroy(collision.transform.parent.gameObject);
                    NewScale();
                    OnStand(false);
                }
                else
                {
                    DestroyRingsRaw = 0;
                    OnStand();
                }
                break;
            case TypeStand.Finish:
                SPAnim.Play();
                collision.GetComponent<Finish>().IAmHere();
                OnFinish();
                break;
        }
        if (DestroyRingsRaw > 4)
            guiseChanger.GetAchievement(6);
    }

    void IEResurrection()
    {
        if (Data.Instance.sv.NoADSpurchased)
            HealthImage.sprite = HealthNoADS;
        // wait 1 second
        IEResurrectionStartTime = Time.time;
        CompleteTimeIEResurrection = true;
        // complete wait

    }

    void IERespawn(float time)
    {
        IERespawnTimeSize = time;
        IERespawnStartTime = Time.time;
        CompleteTimeIERespawn = true;
        //wait time seconds
    }
}