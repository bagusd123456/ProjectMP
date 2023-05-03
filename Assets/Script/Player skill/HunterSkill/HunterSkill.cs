using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSkill : MonoBehaviour
{
    bool isSkillReady = false;
    bool isView360;
    [SerializeField] FieldOfView fov;
    Animator anim;

    [SerializeField] float timerToSkillReady, cooldown, skillDuration, fixedSkillDuration;

    private void Start()
    {
        skillDuration = fixedSkillDuration;
        anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        #region Timer Script
        if (cooldown <= timerToSkillReady && !isView360)
            cooldown += Time.deltaTime;

        if (cooldown >= timerToSkillReady)
        {
            cooldown = timerToSkillReady;
            isSkillReady = true;
        }

        if (isView360)
        {
            skillDuration -= Time.deltaTime;
            if (skillDuration <= 0)
            {
                skillDuration = fixedSkillDuration;
                isView360 = false;
            }
        }

        #endregion

        #region Check if player in 360 view
        if (isView360)
            fov.viewAngle = Mathf.Lerp(fov.viewAngle, 360, 0.125f);
        else
            fov.viewAngle = Mathf.Lerp(fov.viewAngle, 90, 0.125f);
        #endregion

        if (Input.GetKeyDown(KeyCode.F) && isSkillReady)
        {
            isView360 = true;
            //StartCoroutine(FullAngleView());
            isSkillReady = false;
            cooldown = 0;
            anim.SetTrigger("Jump");
            //StartCoroutine(Landing());

            //IEnumerator Landing()
            //{
            //    yield return new WaitForSeconds(.5f);
            //    anim.SetTrigger("Landing");
            //    anim.ResetTrigger("Jump");
            //}
        }
    }

    //IEnumerator FullAngleView()
    //{
    //    fov.viewAngle = Mathf.Lerp(fov.viewAngle, 360, 0.125f);
    //    yield return new WaitForSeconds(skillDuration);
    //    fov.viewAngle = Mathf.Lerp(fov.viewAngle, 90, 0.125f);
    //}
}
