using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnable;
    [SerializeField] GameObject moto;
    public int quantitySpawn = 40;
    List<GameObject> listOfobstacle = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spawnObstacle();
    }

    public void spawnObstacle()
    {
        if (listOfobstacle.Count != 0)
        {
            int goDestroy = listOfobstacle.Count;
            for (int i = 0; i < goDestroy; i++)
            {
                GameObject clone = listOfobstacle[0];
                listOfobstacle.RemoveAt(0);
                Destroy(clone);
            }
        }

        for (int i = 0; i < quantitySpawn; i++)
        {
            GameObject clone = Instantiate(spawnable, new Vector3(Random.Range(-338, 338), 0, Random.Range(-215, 215)), Quaternion.identity);
            listOfobstacle.Add(clone);

            if (Vector3.Distance(moto.transform.position, clone.transform.position) < 28)
            {
                while (Vector3.Distance(moto.transform.position, clone.transform.position) < 28)
                {
                    clone.transform.position = new Vector3(Random.Range(-338, 338), 0, Random.Range(-215, 215));
                }

            }
        }
    }
}
