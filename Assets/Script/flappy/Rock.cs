using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Transform top_rock;
    public Transform bottom_rock;
    float top_rock_pos_y;
    float bottom_rock_pos_y;
    public float speed = 5f;

    //얼마만큼 움직일건지
    public float hole_pos;


    public Transform spawn_point;


    void Start()
    {
        top_rock_pos_y = top_rock.position.y;
        bottom_rock_pos_y = bottom_rock.position.y;

        hole_pos = Random.RandomRange(-2f, 2f);
        top_rock.position = new Vector3(top_rock.position.x, top_rock_pos_y + hole_pos, top_rock.position.z);
        bottom_rock.position = new Vector3(bottom_rock.position.x, bottom_rock_pos_y + hole_pos, bottom_rock.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Trigger"))
        {
            hole_pos = Random.RandomRange(-2f , 2f);
            transform.position = spawn_point.position;
            top_rock.position = new Vector3(top_rock.position.x, top_rock_pos_y + hole_pos, top_rock.position.z);
            bottom_rock.position = new Vector3(bottom_rock.position.x, bottom_rock_pos_y + hole_pos, bottom_rock.position.z);

        }
    }
}
