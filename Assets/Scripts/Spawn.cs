using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject playerCharacter;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerCharacter, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
