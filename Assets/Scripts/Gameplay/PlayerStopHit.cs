using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerStopHit : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public PlayerController player;

        public override void Execute()
        {
            // player.controlEnabled = false;
            //if (player.audioSource && player.respawnAudio)
            // player.audioSource.PlayOneShot(player.respawnAudio);
            
            player.animator.SetBool("hit", false);
        }
    }
}