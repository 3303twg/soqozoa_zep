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

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public GameObject player_info;
    public GameObject networkmanager;

    public SkinManager skinmanager;
    public VehicleManager vehicleManager;

    [SerializeField]
    private Camera camera;

    public GameObject chatmanager;

    private Rigidbody2D rb;
    private Vector3 direction;

    public float speed = 5f;

    public RectTransform canvas;
    public Text name;
    public string nick_name = "�̸�";


    public PhotonView photon_view;

    public bool isGame = false;
    public bool isChat = false;
    public bool isDialog = false;
    public bool isRiding = false;

    //�̸� ���� �ؽð����� ��ȯ�س���
    private static readonly int isMove_hash = Animator.StringToHash("isMove");
    bool isMove = false;

    public Animator animator;
    public GameObject player_list;

    public bool flipX = false;
    public GameObject vehicle_pivot;




    [PunRPC]
    public void Flip(bool flip)
    {
        Debug.Log(flip);
        flipX = flip;

        GetComponent<SpriteRenderer>().flipX = flip;
        if (flipX)
        {
            vehicle_pivot.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            vehicle_pivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(flipX); // �� flipX ���� ����
        }
        else
        {
            flipX = (bool)stream.ReceiveNext(); // ��� flipX ���� ����
        }
    }


    //Ŭ��Ŵ����� ������ �����Ѵܸ��̾�
    //�׷��� ��� Ŭ������ ����� ����ģ���� �ʿ���
    //�̰Ÿ� ��Ʈ��ũ �Ŵ������� ȣ���� �ϴ���?
    [PunRPC]
    public void Get_SkinManager()
    {

        if (GameObject.Find("Manager") != null)
        {
            skinmanager = GameObject.Find("Manager").transform.Find("SkinManager").GetComponent<SkinManager>();
            if (photon_view.IsMine)
            {
                skinmanager.player = gameObject;
                skinmanager.player_info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
            }


            vehicleManager = GameObject.Find("Manager").transform.Find("VehicleManager").GetComponent<VehicleManager>();
            if (photon_view.IsMine)
            {
                vehicleManager.player = gameObject;
                vehicleManager.Get_player_vehicle();
            }

            photon_view.RPC("Change_isRiding", RpcTarget.All, isRiding);

        }


    }




    [PunRPC]
    public void Set_parent()
    {
        transform.SetParent(player_list.transform);
    }
    //��� ������ ȣ����
    public void Init_player()
    {

        animator = GetComponent<Animator>();


        photon_view.RPC("Set_parent", RpcTarget.All);
        //ī�޶� ����

        if (photon_view.IsMine)
        {
            camera = Camera.main;
            GameObject.Find("Main Camera").GetComponent<CameraController>().target = gameObject;

            networkmanager = GameObject.Find("NetWorkManager");
            networkmanager.GetComponent<NetworkManager>().player = gameObject;
            networkmanager.GetComponent<NetworkManager>().photon_view.RPC("RPC_sync_player", RpcTarget.All);
            networkmanager.GetComponent<NetworkManager>().RPC_sync_player();

            chatmanager = GameObject.Find("ChatManager");
            chatmanager.GetComponent<ChatManager>().player = gameObject;
            chatmanager.GetComponent<ChatManager>().Init_chat_manager();


            nick_name = player_info.GetComponent<PlayerInfo>().nick_name;
            name.text = nick_name;
            networkmanager.GetComponent<NetworkManager>().RPC_sync_nick_name();

            
        }
        else if(photon_view.IsMine == false)
        {
            Debug.Log("�־ȴ�??");
            GetComponent<BoxCollider2D>().isTrigger = true;
        }


        
        //��� Ŭ���÷��̾ ����ؾ���
        skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);
        //��Ų�Ŵ��� �����ؼ� ���۽�ų��
        vehicleManager.photon_view.RPC("RPC_Refresh_vehicle", RpcTarget.All);
    }

    private void Awake()
    {
        photon_view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        player_list = GameObject.Find("PlayerList");
        player_info = GameObject.Find("PlayerInfo");
        vehicle_pivot = transform.Find("VehiclePivot").gameObject;
    }
    private void Start()
    {
        if(player_info.GetComponent<PlayerInfo>().isGame == true)
        {
            gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        //Ŭ���̾�Ʈ ������ ������Ʈ�� ��Ʈ�� �����ϰ��ϱ�
        if (photon_view.IsMine == true)
        {
            if (isChat == false && isDialog == false)
            {
                move();
            } 
        }
    }

    protected void move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertial = Input.GetAxisRaw("Vertical");
        direction = new Vector2(horizontal, vertial).normalized;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.position;

        Vector3 canvas_scale = canvas.localScale;
        if (photon_view.IsMine)
        {
            if (mouseWorldPos.x < playerPos.x)
            {

                //canvas_scale.x = -Mathf.Abs(canvas_scale.x);
                //transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                photon_view.RPC("Flip", RpcTarget.All, true);
            }
            else
            {
                //canvas_scale.x = Mathf.Abs(canvas_scale.x);
                //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                photon_view.RPC("Flip", RpcTarget.All, false);
            }
        }



        transform.position += direction * speed * Time.deltaTime;
        canvas.localScale = canvas_scale;

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
            if (skinmanager != null)
            {
                skinmanager.Change_skin(gameObject, index);
            }
        }
    }

    [PunRPC]
    public void Refresh_vehicle(int index)
    {
        if (isGame == false)
        {
            if (vehicleManager != null)
            {
                if (isRiding == true)
                {
                    vehicleManager.Change_vehicle(gameObject, index);
                    vehicleManager.Get_on(gameObject, index);
                }
                else
                {
                    vehicleManager.Get_off(gameObject);
                }
            }
        }
    }

    [PunRPC]
    public void Change_isRiding(bool flag)
    {
        isRiding = flag;
    }


    [PunRPC]
    void Write_text(string text)
    {

        //? �ٵ� �̰� �����ݾ� ������ �� ���� �̷��� ����� �������°���? ����....
        //���ۻ� �߿����� �ʴϱ� ���߿� ���ľ߰ڴ� ����������
        gameObject.transform.Find("Canvas/chat_ballon/text").gameObject.GetComponent<Text>().text = text;
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(true);
    }

    [PunRPC]
    void Destroy_chat_ballon()
    {
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(false);
    }

}
