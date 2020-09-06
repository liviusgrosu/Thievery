using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameStateController : NetworkBehaviour {

    //Get all the players
    //Check if all of them are in the jail
    //Win the game if they get to the win region
    //Lose the game if both players are in jail

    [SyncVar] public GameObject player1;
    [SyncVar] public GameObject player2;

    [SyncVar] public bool player1InJail;
    [SyncVar] public bool player2InJail;

    [SyncVar] public bool player1InWinRegion;
    [SyncVar] public bool player2InWinRegion;

    [SyncVar] public int currentScore = 0;

    public int scoreToWin = 0;

    bool gameFinish = false;
    bool gameLoss = false;

    public int maxPlayerLives = 3;
    static int currPlayerLives;

    private void Start()
    {
        //Set players lives 
        currPlayerLives = maxPlayerLives;
    }

    private void Update() {
        //If the player is back at spawn and the game is not finished...
        if (player1InJail)
        {
            if (!gameFinish)
            {
                //Restart the game but end if it they lost all their lives
                gameFinish = true;
                if(currPlayerLives > 0) gameLoss = true;
                currPlayerLives--;
                player1.GetComponent<PlayerController>().PlayerLoseTrigger();
            }
        }
        //If the player is at the winning region and has enough points then they win the game and fades off
        if (player1InWinRegion && currentScore >= scoreToWin)
        {
            if (!gameFinish)
            {
                gameFinish = true;
                player1.GetComponent<PlayerController>().PlayerWinTrigger();
            }
        }
    }

    //Get Score
    public int GetLootScore() {
        return currentScore;
    }

    //Increase Score
    [Command]
    public void CmdIncreaseScorePool(int points) {
        currentScore += points;
    }

    //OBSOLETE FUNCTION
    [Command]
    public void CmdAddPlayer(string name) {
        if (player1 == null) {
            player1 = GameObject.Find(name);
        }
        else {
            player2 = GameObject.Find(name);
            print(player2.name);
        }
    }

    //Check if the player is back at spawn
    [Command]
    public void CmdAddPlayerToJail(string name) {
        if (name == player1.name) player1InJail = true;
        else player2InJail = true;
    }

    //Check if the player is at the winning region
    [Command]
    public void CmdAddPlayerToWinRegions(string name, bool state) {
        if (name == player1.name) {
            player1InWinRegion = state;
        }
        else {
            player2InWinRegion = state;
        }
    }

    //Delete object
    [Command]
    public void CmdDestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    //Get the score to win
    public int GetScoreToWin()
    {
        return scoreToWin;
    }

    //Get the max lives
    public int GetMaxPlayersLives()
    {
        return maxPlayerLives;
    }

    //Get the current amount of lives
    public int GetCurrPlayersLives()
    {
        return currPlayerLives;
    }

    //Check if the game has been lost
    public bool IsGameLost()
    {
        return gameLoss;
    }

    //Restart the game
    public void RestartGame(bool state)
    {
        player1InJail = state;
        gameLoss = state;
        gameFinish = state;
    }
}
