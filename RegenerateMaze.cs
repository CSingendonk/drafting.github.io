using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RegenerateMaze : MonoBehaviour
{

    private GameObject[] children;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        var kids = gameObject.GetComponentsInChildren<Transform>();
        int kidCount = kids.Length;
        children = new GameObject[kidCount + 1];
        var i = 0;
        foreach (var kid in kids)
        {
            i++;
            children[i] = kid.gameObject;
        }
    }

    public GameObject confirmhost;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            confirmhost.SetActive(true);
            //if (Input.GetKeyDown(KeyCode.R) == true)
            //{
            //    return;
            if (children != null)
            {
                if (children.Length > 0)
                {
                    foreach (GameObject child in children)
                    {
                        Destroy(child);
                    }
                    var mg = gameObject.GetComponent<MazeGenerator>();
                    mg.Invoke("MazeGenerator.GenerateMaze((int)(mg.mazeHeight * 1.4), (int)(mg.mazeWidth * 1.4) + 1)", 0f);
                    mg.Invoke("MazeGenerator.PresentMaze", 1f);
                    player.transform.position = new Vector3(0.5f, 2f, 0.5f);
                }
            }
        }
    }
}
