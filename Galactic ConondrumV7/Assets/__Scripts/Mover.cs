using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    Rigidbody rb;
    Vector3 Direction;
    GameObject[] hero;

    public float moveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectsWithTag("Hero");
        rb = GetComponent<Rigidbody>();
        if (hero.Length != 0)
        {
            Direction = (Hero.S.Location() - transform.position).normalized * moveSpeed;
        }
        else
        {
            Direction = (transform.rotation * transform.position).normalized * moveSpeed;
        }
        rb.velocity = new Vector3(Direction.x, Direction.y);
    }

    // Update is called once per frame
    void Update()
    {
        CheckOffscreen();
    }

    void CheckOffscreen()
    {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}
