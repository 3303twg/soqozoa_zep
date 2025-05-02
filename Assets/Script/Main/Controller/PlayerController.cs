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
    [SerializeField]

    private Camera camera;


    private Vector3 direction;
    private Vector3 look_direction;

    private Rigidbody2D rb;

    public float speed = 5f;

    public Text name;

    public string nick_name = "이름";


    PhotonView photon_view;

    //미리 정수 해시값으로 변환해놓기
    private static readonly int isMove_hash = Animator.StringToHash("isMove");
    bool isMove = false;
    



    public Animator animator;
    



    private void Awake()
    {

        

        rb = GetComponent<Rigidbody2D>();

        photon_view = GetComponent<PhotonView>();
        DontDestroyOnLoad(this.gameObject);

        player_info = GameObject.Find("PlayerInfo");

        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        camera = Camera.main;
        GameObject.Find("Main Camera").GetComponent<CameraController>().target = gameObject;


        GameObject chatmanager = GameObject.Find("ChatManager");

        //클라이언트의 오브젝트만 동작
        if (photon_view.IsMine)
        {
            //이거 진짜 오래걸리는 작업임
            //ㅋㅋ 이건 진짜 어떻게 해결해야하냐 ㅋㅋ.....
            networkmanager = GameObject.Find("Manager").transform.Find("NetWorkManager").gameObject;
            networkmanager.GetComponent<NetworkManager>().player = gameObject;

            chatmanager.GetComponent<ChatManager>().Init_chat_manager();
            PhotonNetwork.NickName = player_info.GetComponent<PlayerInfo>().nick_name;
            nick_name = PhotonNetwork.NickName;
            name.text = nick_name;

            Debug.Log(name.text + "+ 동작함");

            networkmanager.GetComponent<NetworkManager>().RPC_sync_nick_name();
            //RPC_change_nick_name();
        }

        
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
        Debug.Log("RPC 정상으로 옴");
    }


    
    public void RPC_change_nick_name()
    {
        photon_view.RPC("change_nick_name",RpcTarget.All, nick_name);
    }
}
