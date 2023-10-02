using System.Collections.Generic;

public class Default : Block
{
    protected override BlockConfig GetBlockConfig()
    {
        return new BlockConfig(0, 2, 0.1f, 1);
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
        blockController.CreateBlockByBeamType(lastBeamingBeamType,transform.position);
        Destroy();
    }

    
    protected override void OnRefilled()
    {
        //nothing to do
    }

    public override void Destroy()
    {
        Destroy(gameObject);
    }

    protected override List<BeamType> GetFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Nature,
            BeamType.Fire,
            BeamType.Frost,
            BeamType.Lighting,
            BeamType.Void,
            BeamType.Soul,
        };
    }

    protected override List<BeamType> GetReFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Creativity
        };
    }
    
}
