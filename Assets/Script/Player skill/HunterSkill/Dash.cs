using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dash : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] float chargedSpeed;
    [SerializeField] float minCharge, maxCharge;
    [SerializeField] GameObject chargingUIParent;
    [SerializeField] Image chargingUI;
    bool dashNow;

    [SerializeField] float cooldown;
    [SerializeField] float initialCooldown;
    bool isCanDash = false;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        chargingUIParent.SetActive(false);
        cooldown = initialCooldown;
    }

    private void Update()
    {
        checkCooldown();
        chargedSpeed = Mathf.Clamp(chargedSpeed,minCharge, maxCharge);
        if (Input.GetKey(KeyCode.Q) && isCanDash)
        {
            chargedSpeed += Time.deltaTime * 50;
            chargingUIParent.SetActive(true);
            chargingUI.fillAmount = chargedSpeed / maxCharge;
        }

        if(Input.GetKeyUp(KeyCode.Q))
        {
            dashNow = true;
            cooldown = initialCooldown;
            chargingUIParent.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (dashNow && isCanDash)
        {
            rb.AddForce(rb.transform.forward * force * chargedSpeed, ForceMode.Impulse);
            chargedSpeed = 0;
            dashNow = false;
            Debug.Log("Dash Now : " + dashNow);
        }
    }

    void checkCooldown()
    {
        if(cooldown >= 0)
        {
            cooldown -= Time.deltaTime;
            isCanDash = false;
            if (cooldown <= 0)
            {
                cooldown = 0;
                isCanDash = true;
            }
        }
    }
}