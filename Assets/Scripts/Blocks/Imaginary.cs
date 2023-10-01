using System.Collections.Generic;

public class Imaginary : Block
{

    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Agony,
            BeamType.Nature,
            BeamType.Fire,
            BeamType.Frost,
            BeamType.Lighting,
            BeamType.Void,
            BeamType.Soul,
        };
    }


    protected override void OnFilled()
    {
        Destroy();
    }
}
