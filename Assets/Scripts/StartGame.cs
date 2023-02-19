using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] Animator rocketAnim;
    [SerializeField] Animator menuAnim;
    [SerializeField] GameObject menuUI;
    [SerializeField] ParticleSystem rocketParticles;
    [SerializeField] Stats[] allPlayerStats;
    [SerializeField] Image playerIcon;
    [SerializeField] TMP_Text playerStats;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] GameObject[] tutorialPoints;
    [SerializeField] AudioSource howToAudio;

    Stats stat;
    int tutorialNum;

    public void Awake()
    {
        stat = allPlayerStats[Random.Range(0, allPlayerStats.Length)];
        playerIcon.sprite = stat.characterIcon;
        playerStats.text = ($"{stat.characterName}\nFire: {stat.fireCoolDownTime}" +
            $"\nDash: {stat.dashCoolDownTime}\nReload: {stat.reloadTime}\nAmmo: {stat.maxAmmoCount}" +
            $"\nSpeed: {stat.walkSpeed}");

        playerMovement.stats = stat;
    }

    public void Update()
    {
        if (tutorialParent.activeSelf && Input.GetMouseButtonDown(0))
        {
            tutorialPoints[tutorialNum].SetActive(false);
            ++tutorialNum;
            howToAudio.Play();
            if (tutorialNum >= tutorialPoints.Length)
                tutorialParent.SetActive(false);
            else
                tutorialPoints[tutorialNum].SetActive(true);
        }
    }

    public void StartGameButton()
    {
        menuAnim.SetTrigger("Begin");
    }

    public void RocketSequence()
    {
        menuUI.SetActive(false);
        rocketAnim.SetTrigger("Begin");
        rocketParticles.Play();
    }

    public void StartTutorial()
    {
        tutorialParent.SetActive(true);
        tutorialPoints[0].SetActive(true);
        tutorialNum = 0;
    }
}
