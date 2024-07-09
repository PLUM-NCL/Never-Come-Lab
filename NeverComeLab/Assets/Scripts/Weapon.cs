using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 공격 관련, 피격 필요  

public class Weapon : MonoBehaviour
{
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
        switch (id) //무기 아이디별로 무기로직 실행 
        {
            case 0: //공격 총알인 경우 
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        timer = 0;
                        Fire();
                    }
                }
                break;
            default:
                break;
        }
    }

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
            default:
                break;
        }
    }

    

    void Fire()
    {
        //dir = targetPos(마우스 클릭한 지점) - transform.position(플레이어 현재 위치)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        //bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);  //FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수 
        bullet.GetComponent<Bullet>().Init(damage, dir);

    }

    
}
