using UnityEngine;

public class Multilingual
{
    public string blablabla;

    public string[] NameSkin = new string[9];
    public string[] Description = new string[9];

    public Multilingual(TextAsset text)
    {
        string s = text.text;
        int j = 0;
        string hoarder = "";
        int len = s.Length;
        for (int i = 0; i < len; i++)
            if (s[i] == '\n')
            {
                if (j % 2 == 0)
                    NameSkin[j / 2] = hoarder;
                else
                    Description[(j - 1) / 2] = hoarder;
                hoarder = "";
                j++;
            }
            else
                hoarder += s[i];
        blablabla = hoarder;
    }
}
