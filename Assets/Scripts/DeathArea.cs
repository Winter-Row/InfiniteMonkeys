using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip deathSound2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //play the sound effect with a 50/50 chance to play either sound effect
            if(Random.Range(0, 2) == 0)
            {
                SFXController.instance.PlaySoundFXClip(deathSound, transform, 1f);
            }
            else
            {
                SFXController.instance.PlaySoundFXClip(deathSound2, transform, 1f);
            }
            collision.gameObject.GetComponent<PlayerBehaviour>().OnDeath();
        }
    }
}
