using System.Collections.Generic;

public class Ghost : Block
{


    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>() { BeamType.Soul ,BeamType.Agony};
    }
}
