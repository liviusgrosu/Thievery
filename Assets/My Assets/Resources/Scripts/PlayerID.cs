using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//OBSOLETE SCRIPT
public class PlayerID : NetworkBehaviour {

    [SyncVar] public string playerUniqueIdentity;
    private NetworkInstanceId playerNetID;
    private Transform myTransform;

    public override void OnStartLocalPlayer() {
        GetNetIdentity();
        SetIdentity();
    }

    // Use this for initialization
    void Awake() {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update() {
        if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
            SetIdentity();
        }
    }

    [Client]
    void GetNetIdentity() {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdtellServerMyIdentity(MakeUniqueIdentity());
    }

    void SetIdentity() {
        if (!isLocalPlayer)
            myTransform.name = playerUniqueIdentity;
        else 
            myTransform.name = MakeUniqueIdentity();
    }

    string MakeUniqueIdentity() {
        string uniqueName = "Player " + playerNetID.ToString();
        return uniqueName;
    }

    [Command]
    void CmdtellServerMyIdentity(string name) {
        playerUniqueIdentity = name;
    }
}
//OBSOLETE SCRIPT