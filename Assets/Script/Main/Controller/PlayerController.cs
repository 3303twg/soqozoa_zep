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

    public string nick_name = "이름";


    public PhotonView photon_view;

    public bool isGame = false;
    public bool isChat = false;

    //미리 정수 해시값으로 변환해놓기
    private static readonly int isMove_hash = Animator.StringToHash("isMove");
    bool isMove = false;
    



    public Animator animator;





    //클라매니저는 내꺼만 관리한단말이야
    //그래서 모든 클라한테 명령을 보낼친구가 필요함
    //이거를 네트워크 매니저에서 호출을 하는쪽?
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
    //얘는 무조건 호출함
    public void Init_player()
    {
        rb = GetComponent<Rigidbody2D>();

        player_info = GameObject.Find("PlayerInfo");

        animator = GetComponent<Animator>();

        

        //카메라 설정

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


        
        //모든 클라플레이어가 사용해야함
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
        //클라이언트 소유의 오브젝트만 컨트롤 가능하게하기
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

        //이건 나중에 할까
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



    //ㅈ댓다 이걸 왜 플레이어가 들고있어야해 ㅋㅋ
    // 모든 플레이어의 스킨을 새로고침하는 메서드
    //그렇다면 해당 플레이어 오브젝트가 이걸 불러야하지않을까? 자기자신을 매개변수로 넣어주고?
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

        //? 근데 이거 내꺼잖아 내껀데 왜 굳이 이렇게 힘들게 가져오는거임? ㅋㅋ....
        //동작상 중요하진 않니까 나중에 고쳐야겠다 ㅋㅋㅋㅋㅋ
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
