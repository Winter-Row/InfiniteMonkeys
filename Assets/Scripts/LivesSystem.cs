using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSystem : MonoBehaviour
{
	public Image lifePrefab;  // Assign a UI Image prefab in the Inspector
	public int maxLives = 15;

	private int currentLives;

	public TextMeshProUGUI livesText;

	public TextMeshProUGUI visLivesText;

	void Start()
	{
		currentLives = maxLives;

		livesText.color = Color.green;
		visLivesText.color = Color.green;
	}

	void Update()
	{
		livesText.text = currentLives.ToString();
		visLivesText.text = new string('|', currentLives);

		if (currentLives < 11 && currentLives > 5)
		{
			livesText.color = Color.yellow;
			visLivesText.color = Color.yellow;
		}

		if (currentLives < 5 && currentLives > 0)
		{
			livesText.color = Color.red;
			visLivesText.color = Color.red;
		}
	}

	public void LoseLife()
	{
		if (currentLives > 0)
		{
			currentLives--;
			Debug.Log("Life lost");
		}
	}


	public void UpdateLives(int livesCount)
	{
		livesText.text = livesCount.ToString();
		visLivesText.text = new string('|', livesCount);
	}
}
