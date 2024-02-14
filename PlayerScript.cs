using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public PlayerScript thisShit { get => gameObject.GetComponent<PlayerScript>(); }
    public GameObject scoreboard;
    public GameObject lootDisplay;
    public GameObject levelCountDisplay;
    public MazeGenerator generator;
    private int crates { get => generator.deadEnds.Count; }
    public int suppliesTotal { get => suppliesRemaining + suppliesCollected; set { } }
    public int suppliesRemaining { get; set; }
    public int suppliesCollected { get; set; }
    public int cratesCollected { get; set; }
    public int cratesRemaining { get; set; }
    public int cratesTotal { get; set; }
    public int levelsCompleted { get; set; }
    public int resets { get; set; }
    public int fails { get; set; }
    public float score { get => (float)((int)(1000 / ((suppliesCollected * crates * (levelsCompleted + 1f)) - (float)((resets + 1) * (1 / (fails + 1))) / (Time.realtimeSinceStartupAsDouble / 60)))*0.01); set { } }

    public PlayerScript(float score)
    {
        this.score = score;
    }

    public static void SetCratesTotal(int crates, PlayerScript script)
    {
        script.cratesTotal = crates;
    }

    private Dictionary<string, int> vars;

    // Start is called before the first frame update
    void Start()
    {

        foreach (var property in typeof(PlayerScript).GetFields())
        {
            string key = property.Name;
            var x = property.FieldType;
            var value = typeof(PlayerScript).GetField(key);

            if (x == typeof(int))
            {
                vars.Add(key, (int)value.GetValue(x));
            }
            if (x == typeof(float))
            {
                if (key.Contains("score"))
                {
                    scoreboard.GetComponent<TMP_Text>().textInfo.textComponent.text = score.ToString();
                }
            }
            Debug.Log(key + " " + value.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreboard.GetComponent<TMP_Text>().textInfo.textComponent.text = ((int)(float)System.Math.Round(score)).ToString();
    }
}
