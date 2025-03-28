using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour, IStoreable
{


    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
