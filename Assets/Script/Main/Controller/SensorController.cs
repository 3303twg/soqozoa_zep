using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gameObject = new List<GameObject>();

    [SerializeField]
    GameObject target;

    [SerializeField]
    GameObject player;

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
                if (player.GetComponent<PlayerController>().photon_view.IsMine == true)
                {
                    if (target.gameObject.GetComponent<NPCController>().dialog_text.Length == 0)
                    {
                        target.gameObject.GetComponent<MiniGame>().Play_game();
                    }

                    else
                    {
                        if (player.GetComponent<PlayerController>().isDialog == false)
                        {
                            player.GetComponent<PlayerController>().isDialog = true;
                            target.gameObject.GetComponent<NPCController>().Show_dialog();

                        }
                        else
                        {
                            target.gameObject.GetComponent<NPCController>().Next_dialog();
                            if (target.gameObject.GetComponent<NPCController>().dialog_index == 0)
                            {
                                player.GetComponent<PlayerController>().isDialog = false;
                            }
                        }
                    }
                }
                //
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
                collision.gameObject.GetComponent<NPCController>().Outline_off();
                gameObject.Remove(collision.gameObject);
                
            }
        }
    }

    void Target_select()
    {
        if (gameObject.Count != 0)
        {
            foreach (GameObject obj in gameObject)
            {

                //으아 시간없다
                short_distance = 20;

                if (short_distance > Vector3.Distance(obj.transform.position, transform.position))
                {
                    if (target != null)
                    {
                        target.GetComponent<NPCController>().Outline_off();
                    }
                    short_distance = Vector3.Distance(obj.transform.position, transform.position);
                    target = obj;
                    target.GetComponent<NPCController>().Outline_on();
                }

            }
        }
    }


    void Accent_target()
    {

    }
}
