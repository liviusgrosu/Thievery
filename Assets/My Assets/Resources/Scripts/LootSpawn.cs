using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//OBSOLETE SCRIPT
public class LootSpawn : NetworkBehaviour {

    public GameObject lootPrefab;

    public override void OnStartServer() {
        
    }

    private void Start()
    {
        CmdSpawn();
    }

    [Command]
    void CmdSpawn()
    {
        GameObject bullet = (GameObject)Instantiate(lootPrefab, gameObject.transform.position, gameObject.transform.rotation);
        NetworkServer.Spawn(bullet);
    }
}
//OBSOLETE SCRIPT