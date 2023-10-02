using System.Collections.Generic;
using UnityEngine;

public class Mirror : Block
{
    private BeamController _beamController;
    private GameObject _mirroredBeam;
    private BeamType _mirroredBeamType;

    private const float MirrorExitTime = 0.1f;
    private float _mirrorExitTimeCounter;
    
    protected override BlockConfig GetBlockConfig()
    {
         return new BlockConfig(1, 2, 0.1f, 5f);
    }
    protected override void AwakeTail()
    {
        _beamController = FindObjectOfType<BeamController>();
    }
    protected override void UpdateTail()
    {
        if(_mirroredBeam == null)return;
        
        if (_mirrorExitTimeCounter < 0)
        {
            Destroy(_mirroredBeam.gameObject);
        }
        else
        {
            _mirrorExitTimeCounter -= Time.deltaTime;
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
        if(_mirroredBeam != null)Destroy(_mirroredBeam.gameObject);
        
        blockController.CreateBlockByBeamType(BeamType.Creativity,transform.position);
        Destroy(gameObject);
    }

    protected override List<BeamType> GetFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Lighting
        };
    }

    protected override List<BeamType> GetReFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Creativity
        };
    }

    public void OnLaserStay(BeamType beamType,Vector3 hitPosition, Vector3 reflectVector)
    {
        hitPosition.z = 0;
        reflectVector.z = 0;
        
        if (_mirroredBeam != null)
        {
            if (beamType != _mirroredBeamType)
            {
                Destroy(_mirroredBeam.gameObject);
                _mirroredBeam = Instantiate(_beamController.GetBeamPrefab(beamType),hitPosition,Quaternion.identity);
            }
        }
        else
        {
            _mirroredBeam = Instantiate(_beamController.GetBeamPrefab(beamType),hitPosition,Quaternion.identity);
        }

        _mirroredBeamType = beamType;
        
        //Update position and rotation

        float offSet = 0.01f;
        Vector3 outVector = (hitPosition - transform.position).normalized;

        _mirroredBeam.transform.position = hitPosition + offSet * outVector;
        _mirroredBeam.transform.forward = reflectVector;
        
        Hovl_Laser laser = _mirroredBeam.GetComponent<Hovl_Laser>();
        laser.SetBeamType(beamType);
        laser.SetMaxLength(10);

        _mirrorExitTimeCounter = MirrorExitTime;
    }
}
