using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    private float lerpspeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            Vector3 target_vector = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target_vector, lerpspeed * Time.deltaTime);
        }
    }
}
