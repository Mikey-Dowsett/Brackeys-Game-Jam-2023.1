using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] Transform player;
    float timer = 0;
    float spawnDelay = 1.5f;
    public int enemyLevel = 1;

    void Start()
    {
        StartCoroutine("Spawner");
        StartCoroutine("Difficulty");
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-15, 15),
                Random.Range(-15, 15), 0);
            if (Vector2.Distance(spawnPos, player.position) < 10)
                continue;
            var newEnemy = Instantiate(enemies[Random.Range(0, enemyLevel)],
                spawnPos, Quaternion.identity);
            newEnemy.transform.SetParent(transform);

            yield return new WaitForSeconds(spawnDelay);
            spawnDelay -= 0.005f;
            spawnDelay = Mathf.Clamp(spawnDelay, 0.5f, 5f);
            //print(spawnDelay);
        }
    }

    private IEnumerator Difficulty()
    {
        while (true)
        {
            timer += 1;
            switch (timer)
            {
                case 60:
                    enemyLevel++;
                    print("Level 2");
                    break;
                case 120:
                    enemyLevel++;
                    print("Level 3");
                    break;
                case 180:
                    enemyLevel++;
                    print("Level 4");
                    break;
                case 240:
                    enemyLevel++;
                    print("Level 5");
                    break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
