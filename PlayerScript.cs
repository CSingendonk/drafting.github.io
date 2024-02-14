using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public int suppliesTotal { get; set; }
    public int suppliesRemaining { get; set; }
    public int suppliesCollected { get; set; }
    public int cratesCollected { get; set; }
    public int cratesRemaining { get; set; }
    public int cratesTotal { get; set; }
    public int levelsCompleted { get; set; }
    public int resets { get; set; }
    public int fails { get; set; }
    public float score { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var property in typeof(PlayerScript).GetFields())
        {
            var x = property.FieldType;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
