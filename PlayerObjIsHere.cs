using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObjIsHere : MonoBehaviour
{
    PlayerScript playerScript;
    public enum ActionType
    {
        Action1,
        Action2,
    }

    private Vector3 pCapPos;
    private Vector3 thisPos;

    void Start()
    {
        playerScript = playerCapsule.GetComponent<PlayerScript>();
        var thisPosX = gameObject.transform.position.x;
        var thisPosY = gameObject.transform.position.z;
        thisPos = new Vector3(thisPosX, 0, thisPosY);
    }

    void Update()
    {
        var capPosX = playerCapsule.transform.position.x;
        var capPosY = playerCapsule.transform.position.z;
        pCapPos = new Vector3(capPosX, 0, capPosY);
        if (thisPos.x + 1 >= pCapPos.x && thisPos.x - 1 <= pCapPos.x)
        {
            if (thisPos.z + 1 >= pCapPos.z && thisPos.z - 1 <= pCapPos.z)
            {
                TriggerEnter(playerCapsule);
            }
        }

    }

    public GameObject playerCapsule; 
    public ActionType selectedAction;


    //Crates
    public void Action1()
    {

        gameObject.SetActive(gameObject.activeSelf ? false : true);
        playerScript.score++; // This assumes 'playerScore' is an integer variable in the PlayerScript
        playerScript.suppliesCollected += 10;
        playerScript.cratesCollected++;
        Debug.Log("Action1 Done");
    }

    //Set Self Inactive
    public void Action2()
    {
        gameObject.SetActive(false);
    }

    private void TriggerEnter(GameObject other)
    {
        if (other == playerCapsule)
        {
            switch (selectedAction)
            {
                case ActionType.Action1:
                    {   
                        Action1();
                        Debug.Log("Performing Action1");
                        break;
                    }
                case ActionType.Action2:
                    {
                        Action2();
                        Debug.Log("Performing Action2");
                        break;
                    }

                default:
                    {
                        Debug.Log("No action selected");
                        break;
                    }
            }
        }
    }
}
