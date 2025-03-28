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
        if(gameObject.name == "ItemOne")
        {
            player.addLife(1);
        }else if(gameObject.name == "ItemTwo")
        {
            player.doubleDmg();
        }
        Debug.Log("Picked up");
        Destroy(gameObject);
    }
}
