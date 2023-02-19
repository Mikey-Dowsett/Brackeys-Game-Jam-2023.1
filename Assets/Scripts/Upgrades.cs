using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public string cName;
    public float experience;
    public float fireCoolDownLv;
    public float dashCoolDownLv;
    public float reloadTimeLv;
    public int maxAmmoCountLv;
    public int pierceChanceLv;
    public int explosionChanceLv;
    public float movementSpeedLv;

    [SerializeField] TMP_Text title;
    [SerializeField] Image xpAmount;
    [SerializeField] float xpNeeded = 10;



    float level = 1;
    string dashes = "--";
    public bool levelingUp;

    void Start()
    {
        cName = GameObject.FindObjectOfType<PlayerMovement>().stats.characterName;
        title.text = ($"{cName} - Lv {dashes}{level}");
    }

    void Update()
    {
        xpAmount.fillAmount = Mathf.Lerp(xpAmount.fillAmount,
            experience / xpNeeded, 5 * Time.deltaTime);
        if (xpAmount.fillAmount >= 0.98f && !levelingUp)
        {
            levelingUp = true;
            GameObject.FindObjectOfType<CameraMovement>().StopCoroutine(
                GameObject.FindObjectOfType<CameraMovement>().Shake(0.2f, 0.1f));

            xpNeeded += 2;
            experience = 0;
            ++level;

            switch (level)
            {
                case 10: dashes = "-"; break;
                case 100: dashes = ""; break;
            }
            title.text = ($"{cName} - Lv {dashes}{level}");
            GameObject.FindObjectOfType<UpgradeUIManager>().SummonUpgrades();
            xpAmount.fillAmount = Mathf.Lerp(xpAmount.fillAmount,
                experience / xpNeeded, 5 * Time.deltaTime);
            Time.timeScale = 0;
            StartCoroutine("Pause");
        }
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.02f);
        levelingUp = false;
        StopCoroutine("Pause");
    }
}
