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

		livesText.color = Color.blue;
		visLivesText.color = Color.blue;
	}

	void Update()
	{
		livesText.text = currentLives.ToString();
		visLivesText.text = new string('|', currentLives);

		if (currentLives < 15 && currentLives > 10)
		{
			livesText.color = Color.green;
			visLivesText.color = Color.green;
		}

		if (currentLives < 11 && currentLives > 5)
		{
			livesText.color = Color.yellow;
			visLivesText.color = Color.yellow;
		}

		if (currentLives < 6 && currentLives > 0)
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
            UpdateLives(currentLives);
        }
	}


	public void UpdateLives(int livesCount)
	{
        currentLives = livesCount;  // Update currentLives to prevent UI resetting
        livesText.text = currentLives.ToString();
        visLivesText.text = new string('|', currentLives);
    }
}
