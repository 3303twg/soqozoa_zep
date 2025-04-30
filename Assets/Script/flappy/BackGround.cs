using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed = 2f;

    public Transform top_ground;
    public Transform bottom_ground;

    public Transform spawn_point;

    public GameObject zz;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trigger"))
        {
            transform.position = new Vector3(transform.position.x + 60f, transform.position.y, transform.position.z);

        }
    }
}
