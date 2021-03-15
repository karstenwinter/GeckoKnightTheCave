using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped>
    {
        public PlayerController player;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (player.audioSource && player.jumpAudio)
                player.audioSource.PlayOneShot(player.jumpAudio);
                
            var foundObjects = GameObject.FindObjectsOfType<EnemyController>();
            Debug.Log("found jump: " + foundObjects.Length);
            var away = true;
            foreach(var enemy in foundObjects)
            {
                var dist = (enemy.gameObject.transform.position - player.gameObject.transform.position).magnitude;
                if (dist < 4)
                {
                    Debug.Log(enemy + "was to close to player, dist: " + dist);
                    away = false;
                    break;
                }
            }
            if (away)
            {
                model.spawnPoint.transform.position = player.gameObject.transform.position;

                Debug.Log("spawnPoint set to " + player.gameObject.transform.position);
            }
        }
    }
}