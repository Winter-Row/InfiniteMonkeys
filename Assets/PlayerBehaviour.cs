using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private CheckPoint checkPoint;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = GameObject.FindGameObjectWithTag("CheckPoint").GetComponent<CheckPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RespawnPlayer()
    {
        if (checkPoint.checkCheckPointed())
        {
            gameObject.transform.position = new Vector2(checkPoint.getCheckPointPos().x, checkPoint.getCheckPointPos().y);
        }
    }
}
