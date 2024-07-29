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
    public bool isSelected;
    public ItemData Itemdata;

    float timer;

    private void Awake()
    {
        //Init();
        player = GameManager.Instance.player;
        pos = GetComponent<Transform>();
    }
    
    private void Update()
    {
        isSelected = Itemdata.isSelected;

        if (isSelected == false)
            return;
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

    public void Init(ItemData data)
    {
        Itemdata = data;

        //�⺻ ����
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;    //weapon �θ� player�� ���� 
        transform.localPosition = Vector3.zero;    //������ġ�� �����ν� �÷��̾� ������ ����

        //�Ӽ� ����
        id = data.itemId;
        damage = data.baseDamage;
        isSelected = data.isSelected;

        //��ũ��Ʈ�� ������Ʈ�� �������� ���� �ε����� �ƴ� ���������� �����ϱ� 
        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

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
        Vector3 currentPosition = Vector3.zero;
        if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("RightWalk") && player.spriter.flipX == false)
        {
            rotation = 0;
            currentPosition = new Vector3(1f, 0, 0);
            //currentPosition = new Vector3(-0.5f, -0.1f, 0);
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("RightWalk") && player.spriter.flipX == true)
        {
            rotation = 180;
            currentPosition = new Vector3(-1f, 0, 0);
            //currentPosition = new Vector3(-0.5f, -0.1f, 0);
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("ForwardWalk"))
        {
            rotation = 90;
            currentPosition = new Vector3(0f, 1f, 0);
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("BackWalk"))
        {
            rotation = 270;
            currentPosition = new Vector3(0f, -1f, 0);
        }

        //Vector2 playerPos = inputVec - transform.position;
        //float rotation = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);


        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotation = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;    //dir �� ���ϱ� * ������ ���� �ٲٱ�
        //transform.rotation = Quaternion.Euler(0, 0, rotation); //������ ������Ʈ ȸ�������� ����

        GameObject bullet = GameManager.Instance.pool.Get(prefabId);    //���� ������Ʈ ��Ȱ�� �ϱ� 

        //bullet.transform.position = pos.position;
        bullet.transform.position = pos.position + currentPosition;
        bullet.transform.rotation = transform.rotation;
        print("pos test" + bullet.transform.rotation);

        bullet.GetComponent<Bullet>().Init(damage);
    }


}
