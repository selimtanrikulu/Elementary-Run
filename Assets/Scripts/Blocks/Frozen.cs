using System.Collections.Generic;
using UnityEngine;

public class Frozen : Block
{
    [SerializeField] private float pullBackDelay;
    private float _pullBackDelayCounter;

    private PlayerControl _pullingPlayer;

    private Vector3 _pullBackPosition;
    
    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>() { BeamType.Frost };
    }

    protected override void Update()
    {
        base.Update();

        if(_pullingPlayer == null)return;
        
        if (_pullBackDelayCounter < 0)
        {
            _pullingPlayer.TeleportToPosition(_pullBackPosition);
            _pullingPlayer = null;
            
            blockController.ResetBlock(this);
        }
        else
        {
            _pullBackDelayCounter -= Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl playerControl))
        {
            _pullingPlayer = playerControl;
            _pullBackPosition = _pullingPlayer.transform.position;
            _pullBackDelayCounter = pullBackDelay;
        }        
    }
}
