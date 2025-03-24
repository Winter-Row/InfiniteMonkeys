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

	void Start()
	{
		currentLives = maxLives;
	}

	void Update()
	{
		livesText.text = currentLives.ToString();
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
	}
}
