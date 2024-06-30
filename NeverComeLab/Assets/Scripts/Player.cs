using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float hp = 3;
    private bool isDie = false;
    float speed = 4f;
    float h = 0;
    float v = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");


            rigid.velocity = new Vector2(h, v).normalized * speed;
        }
    }

    public void takeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            isDie = true;
        }
    }
}
