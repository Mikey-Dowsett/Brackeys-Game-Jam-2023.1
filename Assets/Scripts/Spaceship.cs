using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] ParticleSystem rocketParicle;
    [SerializeField] GameObject GameUI;
    [SerializeField] AudioSource audioSource;

    public void Landed()
    {
        player.SetParent(null);
        player.GetComponent<PlayerMovement>().enabled = true;
        enemySpawner.SetActive(true);
        GameUI.SetActive(true);
        rocketParicle.Play();
        StartSound();
    }

    public void StopParticle()
    {
        rocketParicle.Stop();
    }

    public void StartSound()
    {
        audioSource.Play();
    }

    public void EndSound()
    {
        audioSource.Stop();
    }
}
