using System.Collections.Generic;
using UnityEngine;
using System;


public class HotkeyHandlers : MonoBehaviour
{
    public Canvas minimap;
    public Canvas overlayContainer;
    public GameObject manimapCam;
    public GameObject exitButton;
    public GameObject onscreenInputs;
    public GameObject infoText;
    public KeyCode hotKey;
    public Act Actions;

    public static Dictionary<KeyCode, Action> keyMethods = new Dictionary<KeyCode, Action>();

    public List<Action> HotkeyActions = new List<Action>();

    // Use this for initialization
    void Start()
    {
        keyMethods.Clear();
        keyMethods.Add(KeyCode.T, ToggleOnScreenInputsOverlayVisible);
        keyMethods.Add(KeyCode.P, ToggleMinimapVisible);
        keyMethods.Add(KeyCode.Mouse1, ToggleCursorState);
        keyMethods.Add(KeyCode.M, ToggleCursorState);
        keyMethods.Add(KeyCode.U, ToggleInfoText);
       // keyMethods.Add(hotKey, Actions.ConvertTo<Action>());

        FillActionsList(); // Call the method to populate HotkeyActions
        //Action action = new Action(ActActs[Actions]);
        //keyMethods.Add(hotKey, action);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in keyMethods.Keys)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    keyMethods[keyCode]?.Invoke();

                }
            }
            if (Input.GetKeyDown(hotKey))
            {
                foreach (Act thing in ActActs.Keys)
                {
                    if (thing == Actions)
                    {
                        keyMethods.Add(hotKey, ActActs[thing]);
                    }
                }
            }
        }
    }

    public enum Act
    {
        None,
        ToggleOnScreenInputsOverlayVisible,
        ToggleMinimapVisible,
        ToggleCursorState,
        ToggleInfoText
    }

    public List<Act> Acts = new List<Act>();

    public Dictionary<Act, Action> ActActs = new();



    void FillActionsList()
    {
        Acts.Add(Act.ToggleOnScreenInputsOverlayVisible);
        Acts.Add(Act.ToggleMinimapVisible);
        Acts.Add(Act.ToggleCursorState);
        Acts.Add(Act.ToggleInfoText);
        Acts.Add(Act.None);

        HotkeyActions.Clear(); // Clear the list to avoid duplicates if called again

        foreach (Action act in keyMethods.Values)
        {
            HotkeyActions.Add(act);

        }
        for (int i = 0; i < HotkeyActions.Count; i++)
        {
            if (Acts[i] == Actions)
            {
                if (ActActs.ContainsKey(Actions))
                {
                    ActActs.Remove(Actions);
                }
                ActActs.Add(Actions, HotkeyActions[i]);
            };
        }
    }

void ToggleOnScreenInputsOverlayVisible()
    {
        if (onscreenInputs != null)
        {
            onscreenInputs.SetActive(onscreenInputs.activeSelf ? false : true);
        }
    }

    void ToggleInfoText()
    {
        if (infoText != null)
        {
            if (onscreenInputs != null || onscreenInputs.active)
            {
                infoText.SetActive(infoText.activeSelf ? false : true);
            }
            if (!onscreenInputs.active)
            {
                infoText.transform.parent.GetChild(infoText.transform.GetSiblingIndex());
            }
        }
    }

    void ToggleCursorState()
    {
        Cursor.visible = Cursor.visible ? false : true;

        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void ToggleMinimapVisible()
    {
        if (minimap == null || manimapCam == null) { return; }
        minimap.enabled = minimap.enabled ? false : true;
        manimapCam.SetActive(minimap.enabled);
    }

    //void FillActionsList()
    //{
    //    foreach (Action act in keyMethods.Values)
    //    {
    //        HotkeyActions.Add(act);
    //    }
    //}


    //public Enum EnumerateActionsList()
    //{
    //    return keyMethods.Values.ConvertTo<Enum>();
    //}
}