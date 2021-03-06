using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerLanded"></typeparam>
    public class PlayerLanded : Simulation.Event<PlayerLanded>
    {
        public PlayerController player;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var foundObjects = GameObject.FindObjectsOfType<Enemy>();
            //Debug.Log("found landed: " + foundObjects.Length);
            var away = true;
            foreach(var enemy in foundObjects)
            {
                var dist = (enemy.gameObject.transform.position - player.gameObject.transform.position).magnitude;
                if (dist < model.safeSpawnPointSetDistance)
                {
                    //Debug.Log(enemy + "was to close to player, dist: " + dist);
                    away = false;
                    break;
                }
            }
            if (away)
            {
                model.spawnPoint.transform.position = player.gameObject.transform.position;

                //Debug.Log("spawnPoint set to " + player.gameObject.transform.position);
            }
            /*IEnumerable<Vector3> pos = foundObjects.Select(x => x.gameObject.transform.position);
            Debug.Log("found landed: " + 
                 String.Join(", ", pos)
            );
            pos.All(x => );*/
        }
    }
}