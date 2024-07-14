using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터->플레이어 공격 관련, 피격 효과 필요, 몬스터 공격시 여러번 적용되는 버그 수정 필요 

public class Weapon : MonoBehaviour
{
    public Transform pos;

    //무기 id, 프리펩 ID, 데미지, 속도
    public int id;
    public int prefabId;
    public float damage;
    public float speed;

    float timer;

    private void Start()
    {
        Init();
    }

    private void Update()
    {

        //임시용! 왼쪽마우스시 죽이는 무기, 오른쪽마우스시 기절 무기 
        timer += Time.deltaTime;

        if (timer > speed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                timer = 0;
                id = 0;
                prefabId = 0;
                damage = 2;
                Fire();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                timer = 0;
                id = 1;
                prefabId = 1;
                damage = 0;
                Fire();
            }
        }
    }
        //switch (id) //무기 아이디별로 무기로직 실행 
        //{

            //case 0: //공격 총알인 경우 
            //    timer += Time.deltaTime;

            //    if (timer > speed)
            //    {
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            timer = 0;
            //            Fire();
            //        }
            //    }
            //    break;
            //case 1:
            //    if (timer > speed)
            //    {
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            timer = 0;
            //            Fire();
            //        }
            //    }
            //    break;
            //default:
            //    break;
    //    }
    //}

    public void Init()
    {
        //id에 따라 다르게..
        switch (id)
        {
            case 0: //공격 총알인 경우 
                speed = 0.3f;   //연사속도 
                //Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
                //bullet.parent = transform;
                //bullet.GetComponent<Bullet>().Init(damage, Vector3.zero);
                break;
            case 1:
                speed = 0.3f;
                break;
            default:
                break;
        }
    }

    

    void Fire()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotation = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;    //dir 각 구하기 * 라디안을 도로 바꾸기
        transform.rotation = Quaternion.Euler(0, 0, rotation); //실제겜 오브젝트 회전값으로 설정

        GameObject bullet = GameManager.Instance.pool.Get(prefabId);    //기존 오브젝트 재활용 하기 
        bullet.transform.position = pos.position;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Bullet>().Init(damage);
    }

    
}
