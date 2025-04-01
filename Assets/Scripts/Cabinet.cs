using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    private GameObject prompt;
    private Animator animator;
    private List<GameObject> itemList = new List<GameObject>();
    private bool opened = false;
    private bool playerNearby = false; // Track if player is in range

    void Start()
    {
        animator = GetComponent<Animator>();
        prompt = GameObject.Find("Prompt");
        prompt.SetActive(false);

        // Load all items from the "Items" folder
        Object[] items = Resources.LoadAll("Items", typeof(GameObject));
        for (int i = 0; i < items.Length; i++)
        {
            itemList.Add((GameObject)items[i]);  // Add to the itemList
        }
    }

    void Update()
    {
        // Check for input only when the player is nearby
        if (playerNearby && Input.GetKeyDown(KeyCode.F) && !opened)
        {
            OpenCabinet();
        }
    }

    public void setOpened(bool flag)
    {
        opened = flag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !opened)
        {
            playerNearby = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
            prompt.SetActive(false);
        }
    }

    private void OpenCabinet()
    {
        animator.SetBool("Break", true);
        prompt.SetActive(false);
        setOpened(true);
        Debug.Log("Open");
        SpawnRandomItem();
    }

    private void SpawnRandomItem()
    {
        if (itemList.Count > 0)
        {
            int randomIndex = Random.Range(0, itemList.Count);
            GameObject randomItem = itemList[randomIndex];

            // Instantiate the random item at the cabinet's position
            Instantiate(randomItem, transform.position, Quaternion.identity);
            Debug.Log("Item spawned: " + randomItem.name);
        }
    }
}
