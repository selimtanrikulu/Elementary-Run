using System.Collections.Generic;

public class PreImaginary : Block
{
    protected override BlockConfig GetBlockConfig()
    {
        return new BlockConfig(0, 1, 0.1f, 0.1f);
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
        blockController.CreateBlockByBeamType(BeamType.Agony,transform.position);
        Destroy();
    }

    protected override void OnRefilled()
    {
        Destroy();
    }

    public override void Destroy()
    {
        Destroy(gameObject);
    }

    protected override List<BeamType> GetFillerBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Agony
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
