using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesSystem : MonoBehaviour
{
	public Image lifePrefab;  // Assign a UI Image prefab in the Inspector
	public Transform livesParent;  // Assign the parent GameObject (LivesPanel)
	public int maxLives = 3;
	public float spacing = 50f;  // Adjust spacing between images

	private List<Image> livesList = new List<Image>();

	void Start()
	{
		InitializeLives();
	}

	void InitializeLives()
	{
		for (int i = 0; i < maxLives; i++)
		{
			Image life = Instantiate(lifePrefab, livesParent);
			RectTransform rt = life.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(i * spacing, 0);
			livesList.Add(life);
		}
	}

	public void LoseLife()
	{
		if (livesList.Count > 0)
		{
			Image lifeToRemove = livesList[livesList.Count - 1];
			livesList.Remove(lifeToRemove);
			Destroy(lifeToRemove.gameObject); // Destroy the GameObject holding the Image
		}
	}
}
