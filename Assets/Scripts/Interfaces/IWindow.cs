using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class IWindow:MonoBehaviour
{
    [HideInInspector] public CanvasGroup canvasGroup;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SetActive(false);
    }

    abstract public void PrimaryFill();
    abstract public void Refill();
    abstract public void Fill();
    abstract public void Fill(int id);
    abstract public void Fill(GameObject gameObject);

    public void SetActive(bool Open)
    {
        if (Open)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}