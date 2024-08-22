using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    [SerializeField] ItemChecker checker1 = null;
    [SerializeField] ItemChecker checker2 = null;
    [SerializeField] ItemChecker checker3 = null;
    [SerializeField] Animator[] anim;
    [SerializeField] GameObject gasi;

    void Update()
    {
        if (checker1.isOn && checker2.isOn && checker3.isOn)
        {
            anim[0].SetBool("isAttack", false);
            anim[1].SetBool("isAttack", false);
            anim[2].SetBool("isAttack", false);
            gasi.SetActive(false);
        }
        else
        {
            anim[0].SetBool("isAttack", true);
            anim[1].SetBool("isAttack", true);
            anim[2].SetBool("isAttack", true);
            gasi.SetActive(true);
        }
    }
}
