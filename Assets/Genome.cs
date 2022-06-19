using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Genome
{
    public float max_speed = 4;
    public float steer_strength = 6f;
    public float wander_strength = 0.1f;
    public float PH_duration = 100;
    public float PF_duration = 100;
    public float PH_weakness_rate = 0.01f;
}
