using System.Collections.Generic;
using UnityEngine;

public class Portal : Block
{
    private Portal _pair;
    
    private const float TeleportDelay = 1f;
    private float _teleportDelayCounter;
    

    protected override BlockConfig GetBlockConfig()
    {
        return new BlockConfig(1, 2, 0.1f, 3f);
    }
    protected override void AwakeTail()
    {
        //nothing to do
    }
    protected override void UpdateTail()
    {
        if (_teleportDelayCounter > 0)
        {
            _teleportDelayCounter -= Time.deltaTime;
        }
    }

    protected override void OnFilled()
    {
        //nothing to do
    }
    protected override void OnRefilled()
    {
        Destroy();
    }

    public override void Destroy()
    {
        if (_pair != null)
        {
            _pair.ResetPair();
            _pair.Destroy();
        }
        
        blockController.CreateBlockByBeamType(BeamType.Creativity,transform.position);
        Destroy(gameObject);
    }

    protected override List<BeamType> GetFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Void
        };
    }

    protected override List<BeamType> GetReFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Creativity
        };
    }


    public void SetPair(Portal portal)
    {
        GetComponent<Collider>().isTrigger = true;
        _pair = portal;
        
        //Reset fill amount
        ResetFillAmount();
    }
    
    public void ResetPair()
    {
        GetComponent<Collider>().isTrigger = false;
        _pair = null;
    }
    
    private void TeleportPlayer(PlayerControl playerControl)
    {
        _teleportDelayCounter = TeleportDelay;
        playerControl.TeleportToPosition(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_teleportDelayCounter > 0)return;
        
        if (other.TryGetComponent(out PlayerControl playerControl))
        {
            if (_pair == null)
            {
                Debug.LogError("No pair exists but trigger entered");
                return;
            }

            _teleportDelayCounter = TeleportDelay;
            _pair.TeleportPlayer(playerControl);
        }
    }
}
