using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ExitButtonHandler : MonoBehaviour
{
    public UnityEngine.UI.Button quitButton;
    private void Start()
    {
        quitButton.onClick.AddListener(QuitApplication);

    }

    

    private void Update()
    {
        // Attach the Quit function to the button click event
        if (quitButton != null)
        {
            var click = quitButton.onClick;
            Debug.LogError(click.ToString());
            quitButton.enabled = true;
            click.AddListener(QuitApplication);
            if (quitButton)
            {
                QuitApplication();
            }
            else
            {
                Debug.LogError("QuitButton script is attached to an object without a Button component!");
            }
        }
    }



    public void QuitApplication()
    {
        // Log a message to indicate the attempt to quit
        Debug.Log("Quitting application...");

        if (Application.isPlaying)
        {
            // Quit the application works in standalone builds)
            Application.Quit();
        }
    }
}
    
