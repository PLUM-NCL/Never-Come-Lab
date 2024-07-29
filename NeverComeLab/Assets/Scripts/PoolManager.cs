using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<GameObject>[] pools;

    void Awake()
    {
        pools = new List <GameObject> [prefabs.Length];

        for(int index = 0; index < prefabs.Length; index++)
        {
            pools[index] = new List <GameObject> ();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 있는 (비활성화 된) 게임오브젝트 접근.
        // 발견하면 select 변수에 할당
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못 찾았으면(모두 사용중이라면)
        // 새롭게 생성후 select 변수에 할당
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
