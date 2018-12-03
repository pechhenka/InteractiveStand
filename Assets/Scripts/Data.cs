using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : Singleton<Data>
{

    public Sprite Background;
    public Sprite BTNBackground;
    public Sprite BTNBack;

    public string Season = null;

    void Awake()
    {
        Refresh(false);
    }

    public void Refresh(bool BlindMode)
    {
        var month = System.DateTime.Now.Month;
        var day = System.DateTime.Now.Day;

        if (!BlindMode)
            if (month == 12 || month < 3) // Зима
                Season = "Winter";
            else if (month > 2 && month < 6) // Весна
                Season = "Spring";
            else if (month > 5 && month < 8) // Лето
                Season = "Summer";
            else // Иначе всегда осень :)
                Season = "Autumn";
        else // Режим слепого
            Season = "Blind";

        if (Season == null)
            Season = "Winter";

        Background = Resources.Load<Sprite>(Season + "/" + "Background");
        BTNBackground = Resources.Load<Sprite>(Season + "/" + "BTNBackground");
        BTNBack = Resources.Load<Sprite>(Season + "/" + "Back");
    }
}
