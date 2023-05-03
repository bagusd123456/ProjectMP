using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Defusing : MonoBehaviour
{
    [SerializeField] Timer time;

    [SerializeField] bool isCanDefuse = false;
    [SerializeField] GameObject defuseUIButton;

    [SerializeField] float timer = 0;
    [SerializeField] float duration;
    [SerializeField] GameObject defuseProgressBarParent;
    [SerializeField] Image defuseProgressBarFill;

    [SerializeField] int timeReward;
    [SerializeField] GameObject timeRewardUI;
    [SerializeField] GameObject timeRewardUIParent;

    LookAtMouse lookAtMouse;
    private void Start()
    {
        lookAtMouse = FindObjectOfType<LookAtMouse>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isCanDefuse && time.durationInSeconds >= timeReward)
        { 
            StartCoroutine(StartDefuse());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hider"))
        {
            isCanDefuse = true;
            defuseUIButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hider"))
        {
            isCanDefuse = false;
            defuseUIButton.SetActive(false);
            ResetDefuseTime();
        }
    }

    //void StartDefuse()
    //{
    //    defuseProgressBarParent.SetActive(true);
    //    while(timer <= duration)
    //    {
    //        timer += Time.deltaTime;
    //        defuseProgressBarFill.fillAmount = timer / duration;
    //    }
        
    //    if(timer >= duration)
    //    {
    //        timer = duration;
    //        time.durationInSeconds -= timeReward;
    //    }
    //}

    IEnumerator StartDefuse()
    {
        while (timer <= duration)
        {
            defuseProgressBarParent.SetActive(true);
            timer++;
            defuseProgressBarFill.fillAmount = timer / duration;

            lookAtMouse.enabled = false;
            if (timer >= duration)
            {
                timer = duration;
                time.durationInSeconds -= timeReward;
                this.gameObject.SetActive(false);
                lookAtMouse.enabled = true;

                //Vector3 textPos = new Vector3(91.0699158f, 516.3172f, 0);
                //GameObject _timeRewardUI = Instantiate(timeRewardUI, textPos, Quaternion.identity);
                //_timeRewardUI.transform.SetParent(timeRewardUIParent.transform);
                //if (_timeRewardUI != null) Destroy(_timeRewardUI, 2f);

                defuseProgressBarParent.SetActive(false);
                defuseUIButton.SetActive(false);


                StopAllCoroutines();
            }
            yield return new WaitForSeconds(1);
        }
    }

    void ResetDefuseTime()
    {
        StopAllCoroutines();
        timer = 0;
        defuseProgressBarFill.fillAmount = timer / duration;
        defuseProgressBarParent.SetActive(false);
        lookAtMouse.enabled = true;
    }
}
