using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterSprint : MonoBehaviour
{
    bool isSkillReady = false;
    bool isSprinting;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Animator anim;

    [SerializeField] float timerToSkillReady, cooldown, skillDuration, fixedSkillDuration,
        sprintSpeed;

    private void Start()
    {
        skillDuration = fixedSkillDuration;
    }
    // Update is called once per frame
    void Update()
    {
        #region Timer Script
        if (cooldown <= timerToSkillReady && !isSprinting)
            cooldown += Time.deltaTime;

        if (cooldown >= timerToSkillReady)
        {
            cooldown = timerToSkillReady;
            isSkillReady = true;
        }

        if (isSprinting)
        {
            skillDuration -= Time.deltaTime;
            if (skillDuration <= 0)
            {
                skillDuration = fixedSkillDuration;
                isSprinting = false;
            }
        }

        #endregion

        #region Check if player is sprinting
        if (isSprinting)
        {
            playerMovement.runSpeed = sprintSpeed;
            playerMovement.speed = sprintSpeed;
        }
        else
        {
            playerMovement.runSpeed = playerMovement.initialRunSpeed;
            playerMovement.speed = playerMovement.initialSpeed;
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.F) && isSkillReady)
        {
            isSprinting = true;
            //StartCoroutine(FullAngleView());
            anim.SetTrigger("Skill");
            isSkillReady = false;
            cooldown = 0;
        }
    }
}
