using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    [SerializeField] float zoomSpeed;
    [SerializeField] GameObject food_prefab;

    Vector2 prev_pos;

    // Update is called once per frame
    void Update()
    {
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed;


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Instantiate(food_prefab, pos, Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(2))
        {
            prev_pos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            Vector2 pos = (Vector2)Input.mousePosition - prev_pos;
            Camera.main.transform.position -= (Vector3)(pos) * 0.002f * Camera.main.orthographicSize;
            prev_pos = Input.mousePosition;

        }

        if(Input.GetKeyDown(KeyCode.LeftBracket))
            Time.timeScale /= 2;
        if (Input.GetKeyDown(KeyCode.RightBracket))
            Time.timeScale *= 2;
        if (Input.GetKeyDown(KeyCode.P))
            Time.timeScale = 1;

    }
}
