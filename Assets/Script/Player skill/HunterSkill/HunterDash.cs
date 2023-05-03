using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterDash : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float maxForce;
    bool dashNow;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            force += Time.deltaTime * 5;
            if (force >= maxForce) force = maxForce;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            dashNow = true;
        }
    }

    private void FixedUpdate()
    {
        if (dashNow)
        {
            rb.AddForce(rb.transform.forward * force, ForceMode.Impulse);
            force = 1500;
            dashNow = false;
            Debug.Log("Dash Now : " + dashNow);
        }
    }
}
