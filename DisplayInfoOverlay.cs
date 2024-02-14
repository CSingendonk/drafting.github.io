using UnityEngine;

public class DisplayInfoOverlay : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private bool prestart;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        ShowInfoOverlay();
        prestart = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleInfoOverlay();
        }
        if (Input.GetKeyDown(KeyCode.Return) && prestart)
        {
            HideInfoOverlay();
            prestart=false;
        }
    }

    void ToggleInfoOverlay()
    {
        if (gameObject.activeInHierarchy)
        {
            canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    void ShowInfoOverlay()
    {
        canvasGroup.alpha = 1;
    }

    void HideInfoOverlay()
    {
        canvasGroup.alpha = 0;
    }
}