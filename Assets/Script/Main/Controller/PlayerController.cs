using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;
using System.Drawing;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    public GameObject player_info;
    public GameObject networkmanager;

    public SkinManager skinmanager;
    [SerializeField]

    private Camera camera;

    public GameObject chatmanager;


    private Vector3 direction;
    private Vector3 look_direction;

    private Rigidbody2D rb;

    public float speed = 5f;

    public Text name;

    public string nick_name = "�̸�";


    public PhotonView photon_view;

    public bool isGame = false;
    public bool isChat = false;

    //�̸� ���� �ؽð����� ��ȯ�س���
    private static readonly int isMove_hash = Animator.StringToHash("isMove");
    bool isMove = false;
    



    public Animator animator;





    //Ŭ��Ŵ����� ������ �����Ѵܸ��̾�
    //�׷��� ��� Ŭ������ ����� ����ģ���� �ʿ���
    //�̰Ÿ� ��Ʈ��ũ �Ŵ������� ȣ���� �ϴ���?
    [PunRPC]
    public void Get_SkinManager()
    {
        if (isGame == false)
        {
            skinmanager = GameObject.Find("Manager").transform.Find("SkinManager").GetComponent<SkinManager>();
            if (photon_view.IsMine)
            {
                skinmanager.player = gameObject;
                skinmanager.player_info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
            }


            
        }
    }
    //��� ������ ȣ����
    public void Init_player()
    {
        rb = GetComponent<Rigidbody2D>();

        player_info = GameObject.Find("PlayerInfo");

        animator = GetComponent<Animator>();

        

        //ī�޶� ����

        if (photon_view.IsMine)
        {
            camera = Camera.main;
            GameObject.Find("Main Camera").GetComponent<CameraController>().target = gameObject;

            networkmanager = GameObject.Find("Manager").transform.Find("NetWorkManager").gameObject;
            networkmanager.GetComponent<NetworkManager>().player = gameObject;
            networkmanager.GetComponent<NetworkManager>().photon_view.RPC("RPC_sync_player", RpcTarget.All);


            chatmanager = GameObject.Find("ChatManager");
            chatmanager.GetComponent<ChatManager>().player = gameObject;
            chatmanager.GetComponent<ChatManager>().Init_chat_manager();


            nick_name = player_info.GetComponent<PlayerInfo>().nick_name;
            name.text = nick_name;
            networkmanager.GetComponent<NetworkManager>().RPC_sync_nick_name();

            
        }


        
        //��� Ŭ���÷��̾ ����ؾ���
        skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);
    }

    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
    }
    private void Start()
    {

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

        isMove = direction.magnitude > 0.01f;
        animator.SetBool(isMove_hash, isMove);
    }


    [PunRPC]
    public void change_nick_name(string name_text)
    {
        nick_name = name_text;
        name.text = nick_name;
    }


    
    public void RPC_change_nick_name()
    {
        photon_view.RPC("change_nick_name",RpcTarget.All, nick_name);
    }



    //����� �̰� �� �÷��̾ ����־���� ����
    // ��� �÷��̾��� ��Ų�� ���ΰ�ħ�ϴ� �޼���
    //�׷��ٸ� �ش� �÷��̾� ������Ʈ�� �̰� �ҷ�������������? �ڱ��ڽ��� �Ű������� �־��ְ�?
    [PunRPC]
    public void Refresh_skin(int index)
    {
        if (isGame == false)
        {
            skinmanager.Change_skin(gameObject, index);
        }
    }
    [PunRPC]
    void Write_text(string text)
    {
        isChat = true;

        //? �ٵ� �̰� �����ݾ� ������ �� ���� �̷��� ����� �������°���? ����....
        //���ۻ� �߿����� �ʴϱ� ���߿� ���ľ߰ڴ� ����������
        gameObject.transform.Find("Canvas/chat_ballon/text").gameObject.GetComponent<Text>().text = text;
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(true);
    }

    [PunRPC]
    void Destroy_chat_ballon()
    {
        isChat = false;
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(false);
    }

}
