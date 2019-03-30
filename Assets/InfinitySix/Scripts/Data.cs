using Stand;
using System;
using UnityEngine;

public class Data : Stand.Singleton<Data>
{
    public Save sv = new Save();

    public GameObject[] Loop = new GameObject[7];

    public GameObject[] Mark = new GameObject[3];

    public GameObject Angle90Ledge;
    public GameObject Angle60Ledge;
    public GameObject Angle45Ledge;
    public GameObject Angle25Ledge;
    public GameObject Angle15Ledge;

    public Material Stand;
    public Material Ledge;
    public Material Triangle;

    public Material DestroyStand;
    public Material DestroyLedge;

    public bool Authenticate = false;

    public Multilingual multilingual;

    void Awake()
    {
        try
        {
            sv.Load();

            SystemLanguage sl = Application.systemLanguage;
            UnityEngine.Object Language = Resources.Load("Languages/" + sl.ToString());
            if (Language == null)
                multilingual = new Multilingual(Resources.Load("Languages/English") as TextAsset);
            else
                multilingual = new Multilingual(Language as TextAsset);

            for (byte i = 0; i < 7; i++)
                Loop[i] = Resources.Load("Loops/Loop" + i) as GameObject;

            for (byte i = 0; i < 3; i++)
                Mark[i] = Resources.Load("Mark" + i) as GameObject;

            Angle90Ledge = Resources.Load("90Ledge") as GameObject;
            Angle60Ledge = Resources.Load("60Ledge") as GameObject;
            Angle45Ledge = Resources.Load("45Ledge") as GameObject;
            Angle25Ledge = Resources.Load("25Ledge") as GameObject;
            Angle15Ledge = Resources.Load("15Ledge") as GameObject;

            Stand = Resources.Load("Stand") as Material;
            Ledge = Resources.Load("Ledge") as Material;
            Triangle = Resources.Load("Triangle") as Material;
            DestroyStand = Resources.Load("DestroyStand") as Material;
            DestroyLedge = Resources.Load("DestroyLedge") as Material;

            Loger.CompleteInitialized<Data>();
        }
        catch(Exception e)
        {
            Loger.ErrorInitialized<Data>(e);
        }
    }
}

public class Save
{
    int countGames = -4;
    public int CountGames
    {
        get
        {
            return countGames;
        }
        set
        {
            countGames = value;
            PlayerPrefs.SetInt("CountGames", value);
        }
    }

    bool sound = false;
    public bool Sound
    {
        get
        {
            return sound;
        }
        set
        {
            sound = value;
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
        }
    }

    bool noADSpurchased = false;
    public bool NoADSpurchased
    {
        get
        {
            return noADSpurchased;
        }
        set
        {
            noADSpurchased = value;
            PlayerPrefs.SetInt("NoADSpurchased", value ? 1 : 0);
        }
    }

    int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            PlayerPrefs.SetInt("Level", value);
        }
    }

    int bestScore = 1;
    public int BestScore
    {
        get
        {
            return bestScore;
        }
        set
        {
            bestScore = value;
            PlayerPrefs.SetInt("BestScore", value);
        }
    }

    bool[] unlockSkin = new bool[9];
    public bool[] UnlockSkin
    {
        get
        {
            return unlockSkin;
        }
        set
        {
            unlockSkin = value;
        }
    }

    int[] dayInGame = new int[3];
    public int[] DayInGame
    {
        get
        {
            return dayInGame;
        }
        set
        {
            for (int i = 0; i < 3; i++)
                PlayerPrefs.SetInt("DayInGame" + i, dayInGame[i]);
        }
    }
    public void SaveSkin()
    {
        string s = "";
        for (int i = 0; i < 9; i++)
            s += (unlockSkin[i]) ? '1' : '0';
        PlayerPrefs.SetString("UnlockSkin", s);
    }

    public void Load()
    {
        countGames = PlayerPrefs.GetInt("CountGames", -2);
        sound = PlayerPrefs.GetInt("Sound", 0) == 1;
        noADSpurchased = PlayerPrefs.GetInt("NoADSpurchased", 0) == 1;
        level = PlayerPrefs.GetInt("Level", 1);
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        string s = PlayerPrefs.GetString("UnlockSkin", "100000000");
        for (int i = 0; i < 9; i++)
            unlockSkin[i] = s[i] == '1';

        for (int i = 0; i < 3; i++)
            dayInGame[i] = PlayerPrefs.GetInt("DayInGame" + i, 0);
    }
}

