using UnityEngine;

public class LipController : MonoBehaviour
{
    struct Zone
    {
        public int Begin;
        public int End;
        public string tag;
    }

    Rigidbody2D rg;
    Zone[] zons;

    private bool destroy = false;
    private float StartDestroy;

    void Awake()
    {
        rg = GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        rg.MoveRotation(rg.rotation + Handling.Result);

        if (destroy)
        {
            float delta = (Time.time - StartDestroy) * 3.6f;
            if (delta > 0.34f)
            {
                Destroy(gameObject);
                return;
            }
            Data.Instance.DestroyLedge.SetFloat("_Level", delta);
            Data.Instance.DestroyStand.SetFloat("_Level", delta);
        }
    }

    public TypeStand Fallen()
    {
        TypeStand Lowner = TypeStand.Hole;
        if (transform.GetChild(1).tag != "Finish")
        {
            foreach (Transform child in transform.GetChild(0))
            {
                if (child.tag != "Mark")
                {
                    Vector3 rot = child.eulerAngles;
                    float Under;
                    if (child.tag != "Money")
                    {
                        Under = Near(rot.z, rot.z - int.Parse(child.name.Substring(0, 2)));
                    }
                    else
                    {
                        Under = Near(rot.z + 20, rot.z - 20);
                    }
                    if (Under > 180)
                        Under = 360 - Under;
                    if (Under == 180)
                        if (child.tag == "Ledge")
                        {
                            Lowner = TypeStand.Ledge;
                            break;
                        }
                        else if (child.tag == "Stand")
                            Lowner = TypeStand.Stand;
                }
            }
        }
        else
            Lowner = TypeStand.Finish;
        if (Lowner == TypeStand.Stand)
            AddMark();

        return Lowner;
    }

    void AddMark()
    {
        Transform mark = Instantiate(Data.Instance.Mark[Random.Range(0, Data.Instance.Mark.Length)], transform.GetChild(0)).transform;
        mark.eulerAngles = Vector3.zero;
        mark.GetComponent<SpriteRenderer>().flipX = Random.Range(0, 2) == 1;
    }

    float Near(float a, float b)
    {
        float point = 180f;
        if (point <= a && point >= b)
            return point;
        else
        {
            if (point > a)
                return a;
            else
                return b;
        }
    }

    void AddLedges(float Dangers)
    {
        if (Dangers > 0.3f)
            Dangers = 0.3f;

        float Fullnes = 0f;
        int Length = zons.Length - 1;

        while (Fullnes < Dangers)
        {
            float Eyler = Random.Range(0f, 360f);
            Zone zone = zons[Random.Range(0, Length)];
            zone.tag = "Ledge";
            GameObject Angle = null;
            int Size = 0;
            int Type = UnityEngine.Random.Range(0, 5);

            IdeaLedge(Type, zone, out Size, out Eyler, out Angle);
            while (Size > zone.Begin - zone.End)
            {
                Type--;
                IdeaLedge(Type, zone, out Size, out Eyler, out Angle);
            }

            zone.Begin = (int)Eyler;
            zone.End = zone.Begin - Size;

            zons = Add(zons, zone);
            Transform lol = Instantiate(Angle, transform.GetChild(0)).transform;
            lol.localEulerAngles = new Vector3(0, 0, Eyler);
            Fullnes += Size / 360f;
        }

    }

    Zone[] Add(Zone[] array, Zone newValue)
    {
        int newLength = array.Length + 1;

        Zone[] result = new Zone[newLength];

        for (int i = 0; i < array.Length; i++)
            result[i] = array[i];

        result[newLength - 1] = newValue;

        return result;
    }

    private void IdeaLedge(int Type, Zone zone, out int Size, out float Eyler, out GameObject Angle)
    {
        Size = 0;
        Eyler = 0;
        Angle = null;
        switch (Type)
        {
            case 0:
                Size = 15;
                Eyler = UnityEngine.Random.Range(zone.Begin, zone.End + Size);
                Angle = Data.Instance.Angle15Ledge;
                break;
            case 1:
                Size = 25;
                Eyler = UnityEngine.Random.Range(zone.Begin, zone.End + Size);
                Angle = Data.Instance.Angle25Ledge;
                break;
            case 2:
                Size = 45;
                Eyler = UnityEngine.Random.Range(zone.Begin, zone.End + Size);
                Angle = Data.Instance.Angle45Ledge;
                break;
            case 3:
                Size = 60;
                Eyler = UnityEngine.Random.Range(zone.Begin, zone.End + Size);
                Angle = Data.Instance.Angle60Ledge;
                break;
            case 4:
                Size = 90;
                Eyler = UnityEngine.Random.Range(zone.Begin, zone.End + Size);
                Angle = Data.Instance.Angle90Ledge;
                break;
        }
    }

    int NormalizedAngle(int angle)
    {
        angle %= 360;
        if (angle < 0)
            angle = 360 + angle;
        angle %= 360;
        return angle;
    }

    bool ComparisonAngle(int a, int b)
    {
        return (NormalizedAngle(a) == NormalizedAngle(b));
    }

    public void Init(float Dangers)
    {
        Zone zone;
        zons = new Zone[transform.GetChild(0).childCount];
        int i = 0;
        foreach (Transform child in transform.GetChild(0))
        {
            zone.Begin = (int)(child.localEulerAngles.z);
            zone.End = zone.Begin - int.Parse(child.name.Substring(0, 2));
            zone.tag = child.tag;
            zons[i] = zone;
            i++;
        }

        if (Dangers > 0)
            AddLedges(Dangers);

    }

    public void Destroy()
    {
        Destroy(transform.GetChild(1).gameObject);
        foreach (Transform child in transform.GetChild(0))
        {
            var sp = child.GetComponent<SpriteRenderer>();
            if (sp == null)
                break;

            if (sp.transform.name[2] == 'L')
            {
                sp.material = Data.Instance.DestroyLedge;
            }
            else if (sp.transform.name[2] != 'M')
            {
                sp.material = Data.Instance.DestroyStand;
            }
            else
                break;
        }
        StartDestroy = Time.time;
        destroy = true;
    }
}

public enum TypeStand
{
    Stand = 0,
    Hole = 1,
    Ledge = 2,
    Finish = 3,
}
