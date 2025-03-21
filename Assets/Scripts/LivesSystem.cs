using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivesSystem : MonoBehaviour
{
	public Image lifePrefab;  // Assign a UI Image prefab in the Inspector
	public Transform livesParent;  // Assign the parent GameObject (LivesPanel)
	public int maxLives = 15;
	public float spacing = 50f;  // Adjust spacing between images

	private int currentLives;

	private List<Image> livesList = new();

	public TextMeshProUGUI livesText;

	void Start()
	{
		InitializeLives();
		currentLives = maxLives;
	}

	private void Update()
	{
		livesText.text = currentLives.ToString();
	}

	void InitializeLives()
	{
		for (int i = 0; i <= maxLives; i++)
		{
			Image life = Instantiate(lifePrefab, livesParent);
			RectTransform rt = life.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(i * spacing, 0);
			livesList.Add(life);
			currentLives++;
		}
	}

	public void LoseLife()
	{
		if (livesList.Count > 0)
		{
			Image lifeToRemove = livesList[^(currentLives-1)];
			livesList.Remove(lifeToRemove);
			Destroy(lifeToRemove.gameObject); // Destroy the GameObject holding the Image
			currentLives--;
			Debug.Log("Life lost");
		}
	}
}
