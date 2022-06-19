using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField] GameObject ant_prefab;
    [SerializeField] float count;
    [SerializeField] float birth_cost;

    List<Genome> genepool;

    public int current_food = 0;
    public int total_food = 0;

    void Start()
    {
        genepool = new List<Genome>();
        for (int i = 0; i < count; ++i)
        {
            createAnt();
        }
    }

    public void deposit()
    {
        ++current_food;
        ++total_food;
        if(current_food == birth_cost)
        {
            current_food = 0;
            createAnt();
        }
    }

    void createAnt()
    {
        Genome g = new Genome();
        if(genepool.Count > 0)
        {

        }
        Ant a = Instantiate(ant_prefab, transform.position, Quaternion.identity).GetComponent<Ant>();
        a.nest = transform;
        a.genes = g;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(400,400,400));
    }


    void onDayEnd()
    {

    }
}
