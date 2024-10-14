using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 center;
    public float deltaY = 1;
    public int timer;
    public float moveSpeed = 1;
    public bool isGoingUp, isGoingDown;
    public GameObject mesh;
    public GameObject powerUp;
    void Start()
    {
        isGoingUp = true;
        center = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(transform.up * moveSpeed * 10000);
        if(isGoingUp)
        {
            isGoingUp = GlideUp();
        } else
        {
            isGoingDown = GlideDown();
        }

        if(isGoingDown)
        {
            isGoingDown = GlideDown();
        }
        else
        {
            isGoingUp = GlideUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject powUp = Instantiate(powerUp, other.transform.position, other.transform.rotation);
            other.transform.parent.GetComponent<PlayerInventory>().AddPowerUp(powUp);
            Destroy(gameObject);
        }
    }

    void UpdateTime()
    {
        
        
    }

    bool GlideDown()
    {
        if (transform.position.y > center.y - deltaY)
        {
            transform.Translate(Vector3.down * Mathf.Sqrt(Mathf.Abs(transform.position.y/(center.y - deltaY)) * Mathf.Abs(center.y/transform.position.y) * moveSpeed));
            return true;
        }
        return false;
    }

    bool GlideUp()
    {
        if (transform.position.y < center.y + deltaY)
        {
            transform.Translate(Vector3.up * Mathf.Sqrt(Mathf.Abs(transform.position.y / (center.y + deltaY)) * Mathf.Abs(center.y / transform.position.y) * moveSpeed));
            return true;
        }
        return false;


        
    }
}
