using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            GameManager.Instance.NextLevel();
        }
    }
}
