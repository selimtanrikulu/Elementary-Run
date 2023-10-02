using System.Collections.Generic;
using UnityEngine;

public class Frozen : Block
{
    private PlayerControl _pullingPlayer;
    private Vector3 _pullBackPosition;

    protected override BlockConfig GetBlockConfig()
    {
        return new BlockConfig(1, 2, 0.1f, 7f);
    }
    protected override void AwakeTail()
    {
        //nothing to do
    }
    protected override void UpdateTail()
    {
        //nothing to do
    }

    protected override void OnFilled()
    {
        //nothing to do
    }
    
    protected override void OnRefilled()
    {
        if (_pullingPlayer != null)
        {
            _pullingPlayer.TeleportToPosition(_pullBackPosition);
        }
        
        Destroy();
    }

    public override void Destroy()
    {
        blockController.CreateBlockByBeamType(BeamType.Creativity,transform.position);
        Destroy(gameObject);
    }

    protected override List<BeamType> GetFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Frost
        };
    }

    protected override List<BeamType> GetReFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Creativity
        };
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl playerControl))
        {
            _pullingPlayer = playerControl;
            _pullBackPosition = _pullingPlayer.transform.position;
        }        
    }
}
