using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum roll
{
    buff1,
    buff2,
    debuff1,
    debuff2,
}

public class Slots : MonoBehaviour {
    roll[] table = new roll[20];

    void PopulateTable()
    {
        for (int i = 0; i < table.Length; i++)
        {
            
        }
    }
}
