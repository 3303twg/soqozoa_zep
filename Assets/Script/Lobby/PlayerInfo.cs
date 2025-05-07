using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string nick_name;
    public int skin_index;
    public int vehicle_index;
    public bool isGame = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
