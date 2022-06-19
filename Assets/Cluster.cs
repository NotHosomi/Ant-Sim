using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] GameObject food_prefab;
    float radius;
    float count;


    // Start is called before the first frame update
    void Start()
    {
        radius = Random.Range(0.5f, 3.0f);
        count = radius * 25;
        for(int i = 0; i < count; ++i)
        {
            Instantiate(food_prefab, (Vector2)transform.position + Random.insideUnitCircle * radius, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 2);
    }
}
