using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using TMPro;

using UnityEngine;

public class InteractWithShit : MonoBehaviour
{

    public GameObject triggerer;
    public List<GameObject> CollectionList = new List<GameObject>();
    public static int supplies { get; set; }
    public static int gathered { get; set; }
    public static int remaining { get; set; }
    public GameObject onScreenTimer;
    private double elapsedTime;
    private double lastTime;
    private string elapsedMins;
    private string elapsedHrs;
    private string elapsedScnds;
    public string timeString;


    // Start is called before the first frame update
    void Start()
    {
        //System.Timers.Timer timer = new System.Timers.Timer();
        //timer.Interval = 1000;
        //timer.Start();
        //lastTime = 0;
        //elapsedTime = DateTime.Now.ToLocalTime().Second - lastTime;
        //lastTime = elapsedTime;
        ////elapsedTimer.Instantiate((elapsedTimer.guid = new System.Guid()));
        //timeString = elapsedTime.ToString();
        //elapsedScnds = "00";
        //elapsedMins = "00";
        //elapsedHrs = "00";
        //timer.BeginInit();
        //timer.Elapsed += Timer_Elapsed;
        //timer.Start();
    }

    //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    //{
    //    UpdateTimer();
    //}


    Vector3 GetTriggererPosition()
    {
        return triggerer.transform.position;
    }

    Vector3 GetThisPosition()
    {
        return gameObject.transform.position;
    }

    bool IsInteracting()
    {
        Vector3 a = GetTriggererPosition();
        Vector3 b = GetThisPosition();
        bool c = a.x >= b.x - 0.5 && a.x <= b.x + 0.5;
        //bool d = 0 <= b.y - a.y && 0 >= a.y - b.y;
        bool e = a.z >= b.z - 0.5 && a.z <= b.z + 0.5;
        return e == c;

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}",
            Mathf.FloorToInt((float)elapsedTime / 3600),
            Mathf.FloorToInt((float)elapsedTime / 60) % 60,
            Mathf.FloorToInt((float)elapsedTime) % 60);

        onScreenTimer.GetComponentInChildren<TMP_Text>().text = formattedTime;

    }

        private void UpdateTimer()
    {
        //Timer timer = new ();
       // timer.Instantiate(GraphReference.New(null, true));
        var timesinceload = Time.timeSinceLevelLoadAsDouble / 60;
        Console.Write(timesinceload);
        //elapsedTime = elapsedTime + (DateTime.Now.ToLocalTime().Second - lastTime);
        //if (elapsedTime >= 60)
        //{
        double huh = timesinceload <= ((double)(Time.renderedFrameCount) * 60.0) + 60 ? 1.0 : 0.0;
        double wut = timesinceload >= ((double)(Time.renderedFrameCount) * 60.0) - 60 ? 1.0 : 0.0;
        if (huh == 0.0 || wut == 0.0)
        {
            Debug.Log("huh");
        }
        var tElapsed = (int.Parse(elapsedScnds) + 1);
        if (tElapsed < 10)
        {
            elapsedScnds = "0" + tElapsed;
        }
        else
        {
            if (tElapsed < 60 && tElapsed >= 10)
            {
                elapsedScnds = tElapsed.ToString();
            }
            if (tElapsed >= 60)
            {
                elapsedScnds = "00";
                elapsedMins = (int.Parse(elapsedMins) + 1).ToString();
                tElapsed = int.Parse(elapsedMins);
                if (tElapsed < 10)
                {
                    elapsedMins = "0" + tElapsed.ToString();
                }
                else
                {
                    if (tElapsed >= 60)
                    {
                        elapsedMins = "00";
                        elapsedHrs = (int.Parse(elapsedHrs) + 1).ToString();
                        tElapsed = int.Parse(elapsedHrs);
                        if (tElapsed < 10)
                        {
                            elapsedHrs = "0" + tElapsed.ToString();
                        }
                    }
                }
            }
        }
       // var timeformatted = ((int)elapsedTime).ToString() + ":" +((double)elapsedTime - (int)elapsedTime).ToString();
        lastTime = elapsedTime;
        //var timeUnits = timeformatted.Split(':');
        //foreach (string tUnit in timeUnits)
        //{
        //    int t = 0;
        //    string tString = "";
        //    if (int.Parse(tUnit) < 10)
        //    {
        //        if (tUnit.Length <= 1)
        //        {
        //            tString = "0" + tUnit;
        //        }
        //        else
        //        {
        //            if (tUnit.Length >= 2)
        //            {
        //                continue;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (int.Parse(tUnit) >= 10 && int.Parse(tUnit) <= 59)
        //        {
        //            if (tUnit.Length > 2)
        //            {
        //                tString = Math.Round(decimal.Parse(tUnit), 0).ToString();
        //            }
        //        }
        //    }
        //    timeString = timeString + ":" + tString;
        //}
        //elapsedTimer.Instantiate((elapsedTimer.guid = new System.Guid()));
        timeString = elapsedHrs + ":" + elapsedMins + ":" + elapsedScnds;
        onScreenTimer.GetComponentInChildren<TMP_Text>().textInfo.textComponent.text = timeString;
        //if (IsInteracting())
        //{
        //    InteractWithShit interact = new InteractWithShit();
        //    gameObject.SetActive(false);
        //    CollectionList.Add(gameObject);
        //    InteractWithShit.supplies = gathered + remaining;
        //    InteractWithShit.gathered = supplies - remaining - 1;
        //    InteractWithShit.remaining = supplies - gathered - 1;
        //}
        
    }

}
