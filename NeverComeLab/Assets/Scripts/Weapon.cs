using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� ����, �ǰ� �ʿ�  

public class Weapon : MonoBehaviour
{
    //���� id, ������ ID, ������, �ӵ�
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
        switch (id) //���� ���̵𺰷� ������� ���� 
        {
            case 0: //���� �Ѿ��� ��� 
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
        //id�� ���� �ٸ���..
        switch (id)
        {
            case 0: //���� �Ѿ��� ��� 
                speed = 0.3f;   //����ӵ� 
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
        //dir = targetPos(���콺 Ŭ���� ����) - transform.position(�÷��̾� ���� ��ġ)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        //bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);  //FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ� 
        bullet.GetComponent<Bullet>().Init(damage, dir);

    }

    
}
