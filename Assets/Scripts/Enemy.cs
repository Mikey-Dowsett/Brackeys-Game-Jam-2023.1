using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Transform player;
    [SerializeField] float xp;
    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] normalDeathSound;
    [SerializeField] AudioClip explosionDeathSound;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject ammoResupply;

    private bool dead = false;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
        player.position, speed * Time.deltaTime);

        if (health <= 0 && !dead)
        {
            dead = true;
            GameObject.FindObjectOfType<Upgrades>().experience += xp;

            //Check for chance to explode
            if (Random.Range(1, 100) <
                GameObject.FindObjectOfType<Upgrades>().explosionChanceLv * 5)
            {
                Collider2D[] hitColliders =
                    Physics2D.OverlapCircleAll(transform.position, 2, enemyLayerMask);
                foreach (var goo in hitColliders)
                {
                    if (goo.GetComponent<Enemy>())
                    {
                        goo.GetComponent<Enemy>().Damage();
                    }
                }
                Instantiate(explosionParticle, transform.position, Quaternion.identity);
                PlaySound(explosionDeathSound);
            }
            else
            {
                Instantiate(deathParticle, transform.position, Quaternion.identity);
                PlaySound(normalDeathSound[Random.Range(0, normalDeathSound.Length - 1)]);
            }
            StartCoroutine("Death");
        }

        sr.flipX = player.position.x > transform.position.x ? true : false;

        if (Time.timeScale == 0)
        {
            audioSource.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            Damage();
        }
    }

    public void Damage()
    {
        --health;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource)
        {
            audioSource.clip = clip;
            audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
            audioSource.Play();
        }
    }

    private IEnumerator Death()
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (Random.Range(0, 10) == 1 || (pm.ammoCount == 0 && pm.reloadCount == 0))
            Instantiate(ammoResupply, transform.position, Quaternion.identity);
        speed = 0;
        GetComponent<PolygonCollider2D>().enabled = false;
        sr.color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
    }
}
