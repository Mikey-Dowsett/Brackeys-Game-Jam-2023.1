using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Camera mainCam;
    Vector3 mousePos;

    [SerializeField] float walkSpeed = 5;
    [SerializeField] float dashSpeed = 20;
    public Stats stats;
    [SerializeField] Transform gunRotPoint;
    [SerializeField] Transform shootPoint;

    [SerializeField] TMP_Text ammoCountText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text reloadCountText;
    [SerializeField] Image dashIcon;
    [SerializeField] SpriteRenderer characterIcon;
    [SerializeField] SpriteRenderer gunIcon;
    [SerializeField] Image healthFillBar;

    [SerializeField] ParticleSystem gunParticle;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip gunJam;
    [SerializeField] AudioClip victorySound;
    [SerializeField] AudioClip defeatSound;
    [SerializeField] AudioClip hitSound;

    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] Image canShootImage;

    Upgrades upgrades;
    bool coolDown = false;
    bool canDash = true;
    bool reloading = false;
    bool gameOver = false;
    float speed;
    float seconds = 0;
    float minutes = 5;

    public float reloadCount = 1;
    public int ammoCount;
    public int health;
    public int maxAmmoCount;
    public float dashCoolDown;
    public float reloadTime;
    public float fireCoolDown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<Camera>();
        upgrades = GetComponent<Upgrades>();

        characterIcon.sprite = stats.characterIcon;
        gunIcon.sprite = stats.gunIcon;
        ammoCount = stats.maxAmmoCount;
        health = stats.health;
        maxAmmoCount = stats.maxAmmoCount;
        dashCoolDown = stats.dashCoolDownTime;
        reloadTime = stats.reloadTime;
        fireCoolDown = stats.fireCoolDownTime;
        walkSpeed = stats.walkSpeed;

        speed = walkSpeed;

        StartCoroutine("Timer");
    }

    void Update()
    {
        //Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -15, 15),
            Mathf.Clamp(transform.position.y, -15, 15), transform.position.z);

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(DashTimer(horizontal, vertical));
        }
        else if (canDash)
        {
            speed = walkSpeed;
        }

        //Rotate the gunRotPoint
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - gunRotPoint.position;
        Vector3 rotation = gunRotPoint.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        gunRotPoint.rotation = Quaternion.Euler(0, 0, rot);

        //Attacking
        if (Input.GetMouseButton(0) && !coolDown && ammoCount > 0 && !reloading
            && !gameOver && !GameObject.FindObjectOfType<Upgrades>().levelingUp)
        {
            StopCoroutine("ReloadTimer");
            Instantiate(stats.bulletType, shootPoint.position, Quaternion.identity);
            --ammoCount;
            gunParticle.Play();
            PlaySound(stats.gunSound);
            GameObject.FindObjectOfType<CameraMovement>().StartCoroutine(
                GameObject.FindObjectOfType<CameraMovement>().Shake(0.2f, 0.1f));
            StartCoroutine("CoolDownTimer");
        }
        else if (Input.GetMouseButtonDown(0) && ammoCount <= 0 && !gameOver)
        {
            PlaySound(gunJam);
        }

        if (Input.GetKeyDown(KeyCode.R) && reloadCount > 0)
        {
            reloadCount--;
            StartCoroutine("ReloadTimer");
        }

        //UI Management
        ammoCountText.text = ($"{ammoCount}");

        dashIcon.color = canDash ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 125);

        healthFillBar.fillAmount = Mathf.Lerp(healthFillBar.fillAmount,
            (float)health / stats.health, 5 * Time.unscaledDeltaTime);

        reloadCountText.text = reloadCount.ToString();

        //Upgrades
        reloadTime = stats.reloadTime - upgrades.reloadTimeLv / 5;
        maxAmmoCount = stats.maxAmmoCount + upgrades.maxAmmoCountLv * 2;
        dashCoolDown = stats.dashCoolDownTime - upgrades.dashCoolDownLv / 10;
        fireCoolDown = Mathf.Clamp(stats.fireCoolDownTime - upgrades.fireCoolDownLv / 10, 0.075f, 10);
        walkSpeed = stats.walkSpeed + upgrades.movementSpeedLv / 2f;
        /*print($"Reload: {reloadTime} Ammo: {maxAmmoCount}" +
            $" \nDash: {dashCoolDown} Fire: {fireCoolDown} Speed: {walkSpeed}");*/

        if ((loseScreen.activeSelf || winScreen.activeSelf) && Input.GetMouseButtonDown(0))
        {
            gameOver = true;
            SceneManager.LoadScene(0);
        }
    }

    //Timer for cooldown between attacks.
    private IEnumerator CoolDownTimer()
    {
        canShootImage.enabled = false;
        coolDown = true;
        yield return new WaitForSeconds(fireCoolDown);
        canShootImage.enabled = true;
        coolDown = false;
        StopCoroutine("CoolDownTimer");
    }

    //Timer to reload gun.
    private IEnumerator ReloadTimer()
    {
        reloading = true;
        int ammoNeeded = maxAmmoCount - ammoCount;
        for (int i = 0; i < ammoNeeded; i++)
        {
            ammoCount += 1;
            PlaySound(stats.gunReloadSound);
            yield return new WaitForSeconds(reloadTime / maxAmmoCount);
        }
        reloading = false;
    }

    //Timer for the player to dash
    private IEnumerator DashTimer(float h, float v)
    {
        canDash = false;
        speed = dashSpeed;
        yield return new WaitForSeconds(0.2f);
        speed = walkSpeed;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    //Controls the timer display.
    private IEnumerator Timer()
    {
        string formateTime = "";
        while (minutes > -1)
        {
            formateTime = seconds >= 10 ? "" : "0";
            timerText.text = ($"{minutes}:{formateTime}{seconds}");
            --seconds;
            if (seconds == -1)
            {
                --minutes;
                seconds = 59;
            }
            yield return new WaitForSeconds(1f);
        }
        FindObjectOfType<EnemySpawner>().enabled = false;
        Collider2D[] hitColliders =
                Physics2D.OverlapCircleAll(transform.position, 5, enemyLayerMask);
        foreach (var goo in hitColliders)
        {
            if (goo.GetComponent<Enemy>())
            {
                GameObject.Destroy(goo.gameObject);
            }
        }
        audioSource.clip = victorySound;
        audioSource.Play();
        winScreen.SetActive(true);
    }

    //Checks for collision with enemies.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemy"))
        {
            health--;
            hitParticle.Play();
            PlaySound(hitSound);
            if (health <= 0)
            {
                FindObjectOfType<EnemySpawner>().enabled = false;
                Collider2D[] hitColliders =
                        Physics2D.OverlapCircleAll(transform.position, 5, enemyLayerMask);
                foreach (var goo in hitColliders)
                {
                    if (goo.GetComponent<Enemy>())
                    {
                        GameObject.Destroy(goo.gameObject);
                    }
                }
                audioSource.clip = defeatSound;
                audioSource.Play();
                loseScreen.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ammo"))
        {
            reloadCount++;
            GameObject.Destroy(col.gameObject);
        }
    }

    //Gives player health back.
    public void HealthPack()
    {
        health += stats.health / 2;
        health = Mathf.Clamp(health, 0, stats.health);
    }

    //Plays any given sounds.
    private void PlaySound(AudioClip clip)
    {
        audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        audioSource.clip = clip;
        audioSource.Play();
    }
}
