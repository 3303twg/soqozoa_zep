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
    public string nick_name = "이름";


    public PhotonView photon_view;

    public bool isGame = false;
    public bool isChat = false;
    public bool isDialog = false;
    public bool isRiding = false;

    //미리 정수 해시값으로 변환해놓기
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
            stream.SendNext(flipX); // 내 flipX 값을 보냄
        }
        else
        {
            flipX = (bool)stream.ReceiveNext(); // 상대 flipX 값을 받음
        }
    }


    //클라매니저는 내꺼만 관리한단말이야
    //그래서 모든 클라한테 명령을 보낼친구가 필요함
    //이거를 네트워크 매니저에서 호출을 하는쪽?
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
    //얘는 무조건 호출함
    public void Init_player()
    {

        animator = GetComponent<Animator>();


        photon_view.RPC("Set_parent", RpcTarget.All);
        //카메라 설정

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
            Debug.Log("왜안대??");
            GetComponent<BoxCollider2D>().isTrigger = true;
        }


        
        //모든 클라플레이어가 사용해야함
        skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);
        //스킨매니저 참고해서 동작시킬듯
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
        //클라이언트 소유의 오브젝트만 컨트롤 가능하게하기
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



    //ㅈ댓다 이걸 왜 플레이어가 들고있어야해 ㅋㅋ
    // 모든 플레이어의 스킨을 새로고침하는 메서드
    //그렇다면 해당 플레이어 오브젝트가 이걸 불러야하지않을까? 자기자신을 매개변수로 넣어주고?
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

        //? 근데 이거 내꺼잖아 내껀데 왜 굳이 이렇게 힘들게 가져오는거임? ㅋㅋ....
        //동작상 중요하진 않니까 나중에 고쳐야겠다 ㅋㅋㅋㅋㅋ
        gameObject.transform.Find("Canvas/chat_ballon/text").gameObject.GetComponent<Text>().text = text;
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(true);
    }

    [PunRPC]
    void Destroy_chat_ballon()
    {
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(false);
    }

}
