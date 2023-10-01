using System.Collections.Generic;

public class Default : Block
{

    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>() { BeamType.Creativity ,BeamType.Agony};
    }
    
}
