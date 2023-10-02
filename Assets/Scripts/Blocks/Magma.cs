using System.Collections.Generic;

public class Magma : Block
{
    protected override BlockConfig GetBlockConfig()
    {
        return new BlockConfig(1, 2, 0.1f, 5f);
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
            BeamType.Fire
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
