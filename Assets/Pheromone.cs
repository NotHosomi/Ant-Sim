using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    public int p_type;

    float decay_rate;

    SpriteRenderer sr;
    public Color c;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("decay", Random.Range(0, 1.0f), 1);
        sr = GetComponent<SpriteRenderer>();
        c = sr.color;
    }

    public void init(float decay_time)
    {
        Destroy(gameObject, decay_time);
        decay_rate = 1 / decay_time;
    }

    void decay()
    {
        c.a -= decay_rate;
        sr.color = c;
    }

}
