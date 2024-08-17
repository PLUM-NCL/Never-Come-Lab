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
        
        switch (id) //���� ���̵𺰷� ������� ���� 
        {
            case 0: //���� �Ѿ��� ��� 
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Bullet);
                    Fire();
                }
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.BindBullet);
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
                speed = 0.5f;   //����ӵ� 
                //Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
                //bullet.parent = transform;
                //bullet.GetComponent<Bullet>().Init(damage, Vector3.zero);
                break;
            case 1:
                speed = 0.5f;
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
            currentPosition = new Vector3(0.5f, 0, 0);
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("RightWalk") && player.spriter.flipX == true)
        {
            rotation = 180;
            currentPosition = new Vector3(-0.5f, 0, 0);
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("ForwardWalk"))
        {
            rotation = 90;
            currentPosition = new Vector3(0f, 0.5f, 0);
        }
        else if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("BackWalk"))
        {
            rotation = 270;
            currentPosition = new Vector3(0f, -0.5f, 0);
        }

        //Vector2 playerPos = inputVec - transform.position;
        //float rotation = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);


        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotation = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;    //dir �� ���ϱ� * ������ ���� �ٲٱ�
        //transform.rotation = Quaternion.Euler(0, 0, rotation); //������ ������Ʈ ȸ�������� ����

        
        //�ٷξտ� ���ִ��� Ȯ��, ���ִٸ� �߻� ���ϰԲ� return 
        float rayDistance = 0.05f;
        Vector2 bulletDirection = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
        Debug.DrawLine(transform.position + currentPosition, bulletDirection * rayDistance, Color.red, 1f); // 1�ʰ� ����

        RaycastHit2D hit = Physics2D.Raycast(transform.position + currentPosition, bulletDirection, rayDistance, ~LayerMask.GetMask("Bullet"));

        if (hit.collider != null)
        {
            print("Hit object: " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Wall"))
            {
                print("return��");
                return;
            }
        }

        //���� ���ٸ� �Ѿ� ���� ���� 
        GameObject bullet = GameManager.Instance.pool.Get(prefabId);    //���� ������Ʈ ��Ȱ�� �ϱ� 

        //bullet.transform.position = pos.position;
        bullet.transform.position = transform.position + currentPosition;
        bullet.transform.rotation = transform.rotation;

        bullet.GetComponent<Bullet>().Init(damage);
    }


}
