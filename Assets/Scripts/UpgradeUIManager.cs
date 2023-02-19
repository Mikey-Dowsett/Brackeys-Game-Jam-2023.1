using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> upgradeChoices;
    [SerializeField] Transform[] upgradePlaces;
    [SerializeField] AudioSource audioSource;

    public void SummonUpgrades()
    {
        List<GameObject> tempChoices = new List<GameObject>(upgradeChoices);
        for (int i = 0; i < 3; i++)
        {
            int choice = Random.Range(0, tempChoices.Count);
            var temp = Instantiate(tempChoices[choice],
                upgradePlaces[i].position, Quaternion.identity).transform;
            tempChoices.RemoveAt(choice);
            temp.transform.SetParent(upgradePlaces[i]);
        }
    }

    public void UnsummonUpgrades()
    {
        audioSource.Play();
        Time.timeScale = 1;
        foreach (Upgrade ug in GameObject.FindObjectsOfType<Upgrade>())
        {
            GameObject.Destroy(ug.gameObject);
        }
    }
}
