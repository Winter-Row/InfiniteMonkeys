using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject playerCharacter;
    public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        Instantiate(playerCharacter, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
