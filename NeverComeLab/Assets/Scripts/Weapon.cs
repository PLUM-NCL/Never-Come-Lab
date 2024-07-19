using UnityEngine;

//����->�÷��̾� ���� ����, �ǰ� ȿ�� �ʿ�, ���� ���ݽ� ������ ����Ǵ� ���� ���� �ʿ� 

public class Weapon : MonoBehaviour
{
    public Transform pos;
    Player player;
    Vector3 inputVec;

    //���� id, ������ ID, ������, �ӵ�
    public int id;
    public int prefabId;
    public float damage;
    public float speed;

    float timer;

    private void Start()
    {
        Init();
        player = GameManager.Instance.player;
    }

    private void Awake()
    {
        // player = GameManager.Instance.player;
    }

    private void Update()
    {

        //�ӽÿ�! ���ʸ��콺�� ���̴� ����, �����ʸ��콺�� ���� ���� 
        //    if (timer > speed)
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            timer = 0;
        //            id = 0;
        //            prefabId = 0;
        //            damage = 2;
        //            Fire();
        //        }
        //        else if (Input.GetMouseButtonDown(1))
        //        {
        //            timer = 0;
        //            id = 1;
        //            prefabId = 1;
        //            damage = 0;
        //            Fire();
        //        }
        //    }
        //}
        switch (id) //���� ���̵𺰷� ������� ���� 
        {
            case 0: //���� �Ѿ��� ��� 
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    print("���̾�!!");
                    Fire();
                }
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
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
                speed = 1f;   //����ӵ� 
                //Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
                //bullet.parent = transform;
                //bullet.GetComponent<Bullet>().Init(damage, Vector3.zero);
                break;
            case 1:
                speed = 1f;
                break;
            default:
                break;
        }
    }



    void Fire()
    {
        if(player == null || player.anim == null || player.spriter == null)
        {
            print("�� return��");
            return;
        }

        float rotation = 0;
        if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("RightWalk") && player.spriter.flipX == false)
            rotation = 0;
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("RightWalk") && player.spriter.flipX == true)
        {
            rotation = 180;
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("ForwardWalk"))
            rotation = 90;
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("BackWalk"))
            rotation = 270;

        //Vector2 playerPos = inputVec - transform.position;
        //float rotation = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);


        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotation = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;    //dir �� ���ϱ� * ������ ���� �ٲٱ�
        //transform.rotation = Quaternion.Euler(0, 0, rotation); //������ ������Ʈ ȸ�������� ����

        GameObject bullet = GameManager.Instance.pool.Get(prefabId);    //���� ������Ʈ ��Ȱ�� �ϱ� 
        bullet.transform.position = pos.position;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Bullet>().Init(damage);
    }


}
