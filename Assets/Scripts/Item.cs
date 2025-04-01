using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class Item : MonoBehaviour, IStoreable
{


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(transform.position.x + 1, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void storeItem(PlayerBehaviour player)
    {
        Debug.Log("storeItem called for: " + gameObject.name);

        if (gameObject.name.Contains("ItemOne"))
        {
            player.AddLife(1);
        }
        else if (gameObject.name.Contains("ItemTwo"))
        {
            player.DoubleDmg();
        }

        Debug.Log("Picked up: " + gameObject.name);
        Destroy(gameObject);
    }
}
