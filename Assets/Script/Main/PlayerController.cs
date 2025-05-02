using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    private Vector3 direction;
    private Vector3 look_direction;

    private Rigidbody2D rb;

    public float speed = 5f;

    public Text name;



    PhotonView photon_view;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        photon_view = GetComponent<PhotonView>();
        DontDestroyOnLoad(this.gameObject);

    }
    private void Start()
    {
        camera = Camera.main;

        

        GameObject chatmanager = GameObject.Find("ChatManager");
        chatmanager.GetComponent<ChatManager>().Init_chat_manager();
        //Ÿ�ֻ̹� �������� �����ϱ� start���� ó���ϱ�

        if (photon_view.IsMine)
        {
            name.text = PhotonNetwork.NickName;
        }
    }


    private void Update()
    {
        //Ŭ���̾�Ʈ ������ ������Ʈ�� ��Ʈ�� �����ϰ��ϱ�
        if (photon_view.IsMine == true)
        {
            move();
            
        }


    }

    protected void move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertial = Input.GetAxisRaw("Vertical");
        direction = new Vector2(horizontal, vertial).normalized;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);

        //�̰� ���߿� �ұ�
        look_direction = (worldPos - (Vector2)transform.position);

        if (look_direction.magnitude < 0.9f)
        {
            look_direction = Vector2.zero;
        }
        else
        {
            look_direction = look_direction.normalized;
        }

        transform.position += direction * speed * Time.deltaTime;
    }
}
