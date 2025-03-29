using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    private GameObject prompt;
    private Animator animator;
    private List<GameObject> itemList = new List<GameObject>();
    private bool opened;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOpened(bool flag)
    {
        opened = flag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(opened == false)
        {
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        prompt.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F) && !opened)
            {
                animator.SetBool("Break", true);
                prompt.SetActive(false);
                setOpened(true);
                Debug.Log("Open");
                SpawnRandomItem();
            }
        }
    }
    private void SpawnRandomItem()
    {
        if (itemList.Count > 0)
        {
            // Pick a random item from the list
            int randomIndex = Random.Range(0, itemList.Count);
            GameObject randomItem = itemList[randomIndex];

            // Instantiate the random item at the cabinet's position
            Instantiate(randomItem, transform.position, Quaternion.identity);
            Debug.Log("Item spawned: " + randomItem.name);
        }
    }
}
