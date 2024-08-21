using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    [SerializeField] ItemChecker checker1 = null;
    [SerializeField] ItemChecker checker2 = null;
    [SerializeField] ItemChecker checker3 = null;
    [SerializeField] Animator anim1;
    [SerializeField] Animator anim2;
    [SerializeField] Animator anim3;
    [SerializeField] GameObject gasi;

    void Update()
    {
        if (checker1.isOn && checker2.isOn && checker3.isOn)
        {
            anim1.SetBool("isAttack", false);
            anim2.SetBool("isAttack", false);
            anim3.SetBool("isAttack", false);
            gasi.SetActive(false);
        }
        else
        {
            anim1.SetBool("isAttack", true);
            anim2.SetBool("isAttack", true);
            anim3.SetBool("isAttack", true);
            gasi.SetActive(true);
        }
    }
}
