using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Genome genes = new Genome();
    public int score;

    [SerializeField] GameObject[] pheromone_prefabs;

    [SerializeField] Vector2[] sensor_offsets;
    float[] sensors = { 0, 0, 0 };

    [SerializeField] LayerMask LM_food;
    [SerializeField] LayerMask LM_p_H;
    [SerializeField] LayerMask LM_p_F;
    public Transform nest;

    Vector2 pos;
    Vector2 vel;
    Vector2 wish_dir;

    public bool carrying = false;

    private void Start()
    {
        wish_dir = Random.insideUnitCircle.normalized;
        InvokeRepeating("dropPhero", Random.Range(0, 1.0f), 0.5f);
        //InvokeRepeating("updateSensors", Random.Range(0, 1.0f), 0.02f);
    }

    private void Update()
    {
        pheroSteer();
        wish_dir = (wish_dir + Random.insideUnitCircle * genes.wander_strength).normalized;
        seeFood();


        Vector2 wish_vel = wish_dir * genes.max_speed;
        Vector2 wish_steer = (wish_vel - vel) * genes.steer_strength;
        Vector2 accel = Vector2.ClampMagnitude(wish_steer, genes.steer_strength);

        vel = Vector2.ClampMagnitude(vel + accel * Time.deltaTime, genes.max_speed);
        pos += vel * Time.deltaTime;

        float rot = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg - 90;
        transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, rot));
        if ((transform.position - nest.position).sqrMagnitude < 16 && carrying)
        {
            carrying = false;
            nest.GetComponent<Nest>().deposit();
            ++score;
        }
    }


    [SerializeField] float vision_dist;
    public Transform target_food;
    void seeFood()
    {
        if (carrying)
            return;
        if (target_food == null)
        {
            //LayerMask lm = LayerMask.NameToLayer("Food");
            //lm = ~(lm);
            Collider2D[] foods = Physics2D.OverlapCircleAll(transform.position, vision_dist, LM_food);
            Transform closest = null;
            float best_dist = vision_dist * vision_dist;
            foreach (Collider2D food in foods)
            {
                Debug.DrawLine(transform.position, food.transform.position);
                float dist = (food.transform.position - transform.position).sqrMagnitude;
                // check if in FOV
                Vector2 dir = (food.transform.position - transform.position).normalized;
                float dot = Vector2.Dot(transform.up, dir);
                if (dot > 0.5f && dist < best_dist)
                {
                    best_dist = dist;
                    closest = food.transform;
                }
            }
            target_food = closest;
        }
        else
        {
            wish_dir = (target_food.position - transform.position).normalized;
            if (Vector2.Distance(target_food.position, transform.position) < 0.25f)
            {
                Destroy(target_food.gameObject);
                target_food = null;
                carrying = true;
                vel *= -1;
            }
        }
    }

    void dropPhero()
    {
        Pheromone p = Instantiate(pheromone_prefabs[(carrying) ? 1 : 0], transform.position, Quaternion.identity).GetComponent<Pheromone>();
        p.init(carrying ? genes.PF_duration : genes.PH_duration);

        Vector2 dist = transform.position - nest.position;
        if (Mathf.Abs(dist.x) > 200 || Mathf.Abs(dist.y) > 200)
            Destroy(gameObject); // got lost and died :pensive:
    }

    float sense(int s_id)
    {
        float value = 0;
        LayerMask lm = (carrying) ? LM_p_H : LM_p_F;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + transform.rotation * sensor_offsets[s_id], 1, lm);
        foreach(Collider2D hit in hits)
        {
            value += hit.GetComponent<Pheromone>().c.a;
        }

        return value;
    }

    void pheroSteer()
    {
        for (int i = 0; i < 3; ++i)
            sensors[i] = sense(i);
        if (sensors[1] > Mathf.Max(sensors[0], sensors[2]))
            wish_dir = transform.up;
        if (sensors[0] > Mathf.Max(sensors[1], sensors[2]))
            wish_dir = -transform.right;
        if (sensors[2] > Mathf.Max(sensors[1], sensors[0]))
            wish_dir = transform.right;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, 45) * transform.up * vision_dist);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -45) * transform.up * vision_dist);
        if(target_food != null)
        {
            Gizmos.DrawSphere(target_food.position, 0.25f);
        }

        foreach(Vector2 so in sensor_offsets)
        {
            Gizmos.DrawSphere(transform.position + transform.rotation * so, 1);
        }
    }
}
