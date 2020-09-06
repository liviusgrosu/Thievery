using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour {

	private GameStateController gameSC;
	public GameObject[] healthContainers;
	public Sprite fullHealthSprite, emptyHealthSprite;

	// Use this for initialization
	void Start () {
		gameSC = GameObject.Find("Game State Controller").GetComponent<GameStateController>();
	}

    //Display the right amount of player life icons to the UI depending on the current lives the player has 
	void Update()
	{
		if(gameSC.GetCurrPlayersLives() == 3)
		{
			healthContainers[0].GetComponent<Image>().sprite = fullHealthSprite;
			healthContainers[1].GetComponent<Image>().sprite = fullHealthSprite;
			healthContainers[2].GetComponent<Image>().sprite = fullHealthSprite;
		}

		if(gameSC.GetCurrPlayersLives() == 2)
		{
			healthContainers[0].GetComponent<Image>().sprite = fullHealthSprite;
			healthContainers[1].GetComponent<Image>().sprite = fullHealthSprite;
			healthContainers[2].GetComponent<Image>().sprite = emptyHealthSprite;
		}

		if(gameSC.GetCurrPlayersLives() == 1)
		{
			healthContainers[0].GetComponent<Image>().sprite = fullHealthSprite;
			healthContainers[1].GetComponent<Image>().sprite = emptyHealthSprite;
			healthContainers[2].GetComponent<Image>().sprite = emptyHealthSprite;
		}

		if(gameSC.GetCurrPlayersLives() == 0)
		{
			healthContainers[0].GetComponent<Image>().sprite = emptyHealthSprite;
			healthContainers[1].GetComponent<Image>().sprite = emptyHealthSprite;
			healthContainers[2].GetComponent<Image>().sprite = emptyHealthSprite;
		}
	}
}
