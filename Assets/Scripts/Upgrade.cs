using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    Upgrades ug;

    void Start()
    {
        ug = GameObject.FindObjectOfType<Upgrades>();
    }

    public void UpgradeFire()
    {
        ug.fireCoolDownLv++;
        Clicked();
    }

    public void UpgradeDash()
    {
        ug.dashCoolDownLv++;
        Clicked();
    }

    public void UpgradeReload()
    {
        ug.reloadTimeLv++;
        Clicked();
    }

    public void UpgradeAmmo()
    {
        ug.maxAmmoCountLv++;
        Clicked();
    }

    public void UpgradeHealth()
    {
        GameObject.FindObjectOfType<PlayerMovement>().HealthPack();
        Clicked();
    }

    public void UpgradePierce()
    {
        ug.pierceChanceLv++;
        Clicked();
    }

    public void UpgradeExplosion()
    {
        ug.explosionChanceLv++;
        Clicked();
    }

    public void UpgradeMovement()
    {
        ug.movementSpeedLv++;
        Clicked();
    }

    private void Clicked()
    {
        GameObject.FindObjectOfType<UpgradeUIManager>().UnsummonUpgrades();
    }
}
