using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//OBSOLETE SCRIPT
public class EnemySpawn : NetworkBehaviour {

    public GameObject enemyPrefab;

    public override void OnStartServer() {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        NetworkServer.Spawn(enemy);
    }
}
//OBSOLETE SCRIPT
