using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Block
{
    [SerializeField] private float jumpSpeed;


    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>() { BeamType.Nature ,BeamType.Agony};
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl playerControl))
        {
            playerControl.SetVerticalSpeed(jumpSpeed);
        }
    }
}
