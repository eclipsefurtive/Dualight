namespace _Script.Player
{
    public class PlayerLightState : PlayerState
    {
        
        public PlayerLightState(global::Player player) : base(player)
        {
        }
        
        public override void OnEnterState()
        {
            _playerRenderer.material = _player.LightMaterial;
        }

    }
}