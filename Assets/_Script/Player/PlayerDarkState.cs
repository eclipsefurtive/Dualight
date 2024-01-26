namespace _Script.Player
{
    public class PlayerDarkState : PlayerState
    {
        public PlayerDarkState(global::Player player) : base(player)
        {
        }
        
        public override void OnEnterState()
        {
            _playerRenderer.material = _player.DarkMaterial;
        }
    }
}