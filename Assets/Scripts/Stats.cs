using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject
{
    public string characterName;
    public float fireCoolDownTime;
    public float dashCoolDownTime;
    public float reloadTime;
    public int health;
    public int maxAmmoCount;
    public float walkSpeed;

    public Sprite characterIcon;
    public Sprite gunIcon;
    public AudioClip gunSound;
    public AudioClip gunReloadSound;
    public GameObject bulletType;
}
