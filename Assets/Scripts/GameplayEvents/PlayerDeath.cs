using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if(!player.controlEnabled)
                return;

            player.controlEnabled = false;
            //Debug.Log("Player Death" + player.health.IsAlive);
            //if (player.health.IsAlive)
            //{
            //player.health.Die();
            //if(model.virtualCamera.m_Follow != null) {
            model.virtualCamera.m_Follow = null;
            model.virtualCamera.m_LookAt = null;
            // player.collider.enabled = false;

            //if (player.audioSource && player.ouchAudio)
            //    player.audioSource.PlayOneShot(player.ouchAudio);
            player.animator.SetTrigger("hurt");
            player.animator.SetBool("dead", true);
            Simulation.Schedule<PlayerSpawn>(2f);
            //}
            //}
        }
    }
}