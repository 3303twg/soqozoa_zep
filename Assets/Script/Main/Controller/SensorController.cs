using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gameObject = new List<GameObject>();

    [SerializeField]
    GameObject target;

    float short_distance;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Target_select();

        if(gameObject.Count == 0)
        {
            target = null;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (target != null)
            {
                target.gameObject.GetComponent<MiniGame>().Play_game();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("NPC"))
            {
                gameObject.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("NPC"))
            {
                gameObject.Remove(collision.gameObject);
            }


        }
    }

    void Target_select()
    {
        if (gameObject.Count != 0)
        {
            Debug.Log("머임?");
            foreach (GameObject obj in gameObject)
            {
                short_distance = 20;

                if (short_distance > Vector3.Distance(obj.transform.position, transform.position))
                {
                    Debug.Log("진짜머임?");
                    short_distance = Vector3.Distance(obj.transform.position, transform.position);
                    target = obj;
                }

            }
        }
    }


    void Accent_target()
    {

    }
}
