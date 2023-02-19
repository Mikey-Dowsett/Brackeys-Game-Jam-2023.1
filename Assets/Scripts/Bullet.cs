using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed = 10;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool rocket;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] ParticleSystem explosionParticle;

    void Start()
    {
        //Get components
        rb = GetComponent<Rigidbody2D>();
        Camera mainCam = GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<Camera>();
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //Rotate the bullet
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        StartCoroutine("LifeCounter");
    }

    private IEnumerator LifeCounter()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (Random.Range(1, 100) > GameObject.FindObjectOfType<Upgrades>().pierceChanceLv * 5)
            {
                StartCoroutine("Death");
            }
            if (rocket)
            {
                Collider2D[] hitColliders =
                        Physics2D.OverlapCircleAll(transform.position, 4, enemyLayerMask);
                foreach (var goo in hitColliders)
                {
                    if (goo.GetComponent<Enemy>())
                    {
                        goo.GetComponent<Enemy>().Damage();
                    }
                }
                Instantiate(explosionParticle, transform.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator Death()
    {
        StopCoroutine("LifeCounter");
        audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        audioSource.Play();
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
    }
}
