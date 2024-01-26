using UnityEngine;

namespace _Script.Player
{
    public class PlayerLightState : PlayerState
    {
        private MouseBeam _beam;
        
        public PlayerLightState(global::Player player) : base(player)
        {
            _beam = new MouseBeam()
            {
                ValidPos = false,
                Pos = Vector3.zero,
                Length = 0f
            };
        }
        
        public override void OnEnterState()
        {
            _playerRenderer.material = _player.LightMaterial;
            _player.Inputs.OnClick += TryBeam;
        }

        public override void OnUpdateState()
        {
            _beam.Pos = _player.Inputs.MousePositionWorld;
            if (_beam.Pos == Vector3.zero) _beam.ValidPos = false;
            else _beam.ValidPos = true;
        }

        public override void OnExitState()
        {
            _player.Inputs.OnClick -= TryBeam;
        }

        private void TryBeam()
        {
            if (!_beam.ValidPos) return;
            Debug.Log("Beam!");
        }
    }

    public struct MouseBeam
    {
        public bool ValidPos;
        public Vector3 Pos;
        public float Length;
    }
}