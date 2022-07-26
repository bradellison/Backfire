using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public int level;

    int minLevel;
    int maxLevel;

    void Start()
    {
        minLevel = 1;
        maxLevel = 5;
        level = 1;
    }

    public void IncreaseLevel() {
        if(level < maxLevel) {
            level += 1;
        }
    }

    public void DecreaseLevel() {
        if(level > minLevel) {
            level -= 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
