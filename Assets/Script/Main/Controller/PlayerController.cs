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
    public string nick_name = "戚硯";


    public PhotonView photon_view;

    public bool isGame = false;
    public bool isChat = false;
    public bool isDialog = false;
    public bool isRiding = false;

    //耕軒 舛呪 背獣葵生稽 痕発背兜奄
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
            stream.SendNext(flipX); // 鎧 flipX 葵聖 左蛙
        }
        else
        {
            flipX = (bool)stream.ReceiveNext(); // 雌企 flipX 葵聖 閤製
        }
    }


    //適虞古艦煽澗 鎧襖幻 淫軒廃舘源戚醤
    //益掘辞 乞窮 適虞廃砺 誤敬聖 左馨庁姥亜 琶推敗
    //戚暗研 革闘趨滴 古艦煽拭辞 硲窒聖 馬澗楕?
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
    //剰澗 巷繕闇 硲窒敗
    public void Init_player()
    {

        animator = GetComponent<Animator>();


        photon_view.RPC("Set_parent", RpcTarget.All);
        //朝五虞 竺舛

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
            Debug.Log("訊照企??");
            GetComponent<BoxCollider2D>().isTrigger = true;
        }


        
        //乞窮 適虞巴傾戚嬢亜 紫遂背醤敗
        skinmanager.photon_view.RPC("RPC_Refresh_skin", RpcTarget.All);
        //什轍古艦煽 凧壱背辞 疑拙獣迭牛
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
        //適虞戚情闘 社政税 神崎詮闘幻 珍闘継 亜管馬惟馬奄
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



    //じ奇陥 戚杏 訊 巴傾戚嬢亜 級壱赤嬢醤背 せせ
    // 乞窮 巴傾戚嬢税 什轍聖 歯稽壱徴馬澗 五辞球
    //益係陥檎 背雁 巴傾戚嬢 神崎詮闘亜 戚杏 災君醤馬走省聖猿? 切奄切重聖 古鯵痕呪稽 隔嬢爽壱?
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

        //? 悦汽 戚暗 鎧襖摂焼 鎧黄汽 訊 瓜戚 戚係惟 毘級惟 亜閃神澗暗績? せせ....
        //疑拙雌 掻推馬遭 省艦猿 蟹掻拭 壱団醤畏陥 せせせせせ
        gameObject.transform.Find("Canvas/chat_ballon/text").gameObject.GetComponent<Text>().text = text;
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(true);
    }

    [PunRPC]
    void Destroy_chat_ballon()
    {
        gameObject.transform.Find("Canvas/chat_ballon").gameObject.SetActive(false);
    }

}
