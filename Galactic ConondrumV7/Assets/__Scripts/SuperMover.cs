using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMover : MonoBehaviour
{
    Rigidbody rb;
    Vector3 Direction;

    public float moveSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Direction = (transform.rotation * transform.position).normalized * moveSpeed;

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
