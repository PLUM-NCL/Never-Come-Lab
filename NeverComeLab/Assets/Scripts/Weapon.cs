using UnityEngine;

//몬스터->플레이어 공격 관련, 피격 효과 필요, 몬스터 공격시 여러번 적용되는 버그 수정 필요 

public class Weapon : MonoBehaviour
{
    public Transform pos;
    Player player;
    Vector3 inputVec;


    //무기 id, 프리펩 ID, 데미지, 속도
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
        //임시용! 왼쪽마우스시 죽이는 무기, 오른쪽마우스시 기절 무기 
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
        switch (id) //무기 아이디별로 무기로직 실행 
        {
            case 0: //공격 총알인 경우 
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

        //기본 셋팅
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;    //weapon 부모를 player로 지정 
        transform.localPosition = Vector3.zero;    //지역위치로 함으로써 플레이어 안으로 지정

        //속성 셋팅
        id = data.itemId;
        damage = data.baseDamage;
        isSelected = data.isSelected;

        //스크립트블 오브젝트의 독립성을 위해 인덱스가 아닌 프리펩으로 설정하기 
        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.Instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        //id에 따라 다르게..
        switch (id)
        {
            case 0: //공격 총알인 경우 
                speed = 1f;   //연사속도 
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
            print("엥 return됌");
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
        //float rotation = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;    //dir 각 구하기 * 라디안을 도로 바꾸기
        //transform.rotation = Quaternion.Euler(0, 0, rotation); //실제겜 오브젝트 회전값으로 설정

        GameObject bullet = GameManager.Instance.pool.Get(prefabId);    //기존 오브젝트 재활용 하기 

        //bullet.transform.position = pos.position;
        bullet.transform.position = pos.position + currentPosition;
        bullet.transform.rotation = transform.rotation;
        print("pos test" + bullet.transform.rotation);

        bullet.GetComponent<Bullet>().Init(damage);
    }


}
