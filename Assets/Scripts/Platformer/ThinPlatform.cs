using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatform : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    public Collider col;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    //((PlatformerMovement)(playerController.movementSystem)).playerHeight/2f
    void Update()
    {
        if (!((PlatformerMovement)(playerController.movementSystem)).holdingDown && (player.GetComponent<Rigidbody>().velocity.y <= 0 && transform.position.y <= player.transform.position.y - ((PlatformerMovement)(playerController.movementSystem)).playerHeight/2f))
        {
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }
}
