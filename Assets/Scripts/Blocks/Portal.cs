using System.Collections.Generic;
using UnityEngine;

public class Portal : Block
{
    private Portal _pair;
    
    private const float TeleportDelay = 1f;
    private float _teleportDelayCounter;
    
    
    protected override void Update()
    {
        base.Update();

        if (_teleportDelayCounter > 0)
        {
            _teleportDelayCounter -= Time.deltaTime;
        }
    }


    public override void Destroy()
    {
        if (_pair != null)
        {
            _pair.ResetPair();
        }
        
        base.Destroy();
    }

    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>() { BeamType.Void ,BeamType.Agony};
    }


    public void SetPair(Portal portal)
    {
        if (_pair != null)
        {
            Debug.LogError("Portal already has a pair");
            return;
        }
        
        GetComponent<Collider>().isTrigger = true;
        _pair = portal;
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
