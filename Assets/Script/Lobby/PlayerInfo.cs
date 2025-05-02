using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string nick_name;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
