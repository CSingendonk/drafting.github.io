using UnityEngine;

public class MenuOverlay : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isAnyShiftKeyDown;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        ShowMenu();
        //ShowCursor();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    // The input is not intended for this, return.
        //    isAnyShiftKeyDown = true;
        //    return;
        //}
        //else if (!Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift))
        //{
        //    isAnyShiftKeyDown = false;
        //}
        //// Pressing 'M' toggles the menu
        //if (isAnyShiftKeyDown == true && Input.GetKeyDown(KeyCode.M))
        //{
        //    if (Cursor.visible == true)
        //    {
        //        HideCursor();
        //    }
        //    else if (Cursor.visible == false)
        //    {
        //        ShowCursor();
        //    }
        //}
        //else
        //{
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleMenu();
            }
        //}
    }
    void ToggleMenu()
    {
        // Toggle the alpha of the canvas group (0 for invisible, 1 for visible)
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.enabled = canvasGroup.enabled == true ? false : true;
        canvasGroup.interactable = canvasGroup.interactable == true ? false : true;

        // Show or hide the cursor based on canvas visibility
        //if (canvasGroup.alpha > 0)
        //{
        //    ShowCursor();
        //}
        //else
        //{
        //    HideCursor();
        //}
    }

    //void ShowCursor()
    //{
    //    // Unlock and show the cursor
    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;
    //}

    //void HideCursor()
    //{
    //    // Lock and hide the cursor
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}

    void ShowMenu()
    {
        canvasGroup.alpha = 1;
        //ShowCursor();
    }

    void HideMenu()
    {
        canvasGroup.alpha = 0;
        //HideCursor();
    }
}
