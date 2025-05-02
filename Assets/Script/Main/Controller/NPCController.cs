using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public interface MiniGame
{
    //���� ���� ����..
    public abstract void Play_game();

}


public class NPCController : MonoBehaviour
{

    public Material normal_material;

    public Material outlineMaterial;

    private Renderer renderer;


    private bool isOutlineVisible = true;

    // �ܰ����� ���� ũ��
    public float outlineSizeOn = 1.0f;



    


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        //renderer.material = normal_material;
    }


    public void ToggleOutline()
    {

        if (renderer.material == outlineMaterial)
        {
            renderer.material = normal_material;
        }
        else
        {
            
            renderer.material = outlineMaterial;
        }
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            ToggleOutline();


        }
    }
}
