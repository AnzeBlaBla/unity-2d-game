using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class EnemySoundManager : MonoBehaviour
{
    static List<EnemyController> enemies = new List<EnemyController>();
    static Dictionary<EnemyType, AudioSource> currentlyPlayingSounds = new Dictionary<EnemyType, AudioSource>();
    static Dictionary<EnemyType, bool> enemyTypeToSoundPlaying = new Dictionary<EnemyType, bool>();

    public static void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy.GetComponent<EnemyController>());
        enemy.GetComponent<KillableEntity>().OnDeath += (KillableEntity k) => RemoveEnemy(enemy);

        UpdateSounds();
    }

    static void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy.GetComponent<EnemyController>());
        UpdateSounds();
    }
    static void UpdateSounds()
    {
        // Loop all enemy types and see if we need to play a sound
        foreach (EnemyType enemyType in System.Enum.GetValues(typeof(EnemyType)))
        {
            // If we have no enemies of this type, stop playing the sound
            int thisTypeCount = enemies.FindAll((EnemyController e) => e.enemyType == enemyType).Count;
            Debug.Log("Enemy type " + enemyType + " has " + thisTypeCount + " enemies");
            if (thisTypeCount == 0)
            {
                if (currentlyPlayingSounds.ContainsKey(enemyType))
                {
                    currentlyPlayingSounds[enemyType].Stop();
                    currentlyPlayingSounds.Remove(enemyType);
                }
                enemyTypeToSoundPlaying[enemyType] = false;
            }
            else
            {
                // If we have enemies of this type, see if we need to play the sound
                if (!enemyTypeToSoundPlaying.ContainsKey(enemyType))
                {
                    enemyTypeToSoundPlaying[enemyType] = false;
                }
                if (!enemyTypeToSoundPlaying[enemyType])
                {
                    // Play the sound
                    AudioSource sound = AudioManager.Instance.Play(GetSoundName(enemyType));
                    if(sound != null)
                    {
                        currentlyPlayingSounds[enemyType] = sound;
                        enemyTypeToSoundPlaying[enemyType] = true;
                    }
                }
            }
        }

    }

    static string GetSoundName(EnemyType enemyType)
    {
        return enemyType.ToString() + "Base";
    }

}