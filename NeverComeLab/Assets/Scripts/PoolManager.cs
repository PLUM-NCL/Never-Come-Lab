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

        // ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ ��) ���ӿ�����Ʈ ����.
        // �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �� ã������(��� ������̶��)
        // ���Ӱ� ������ select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
