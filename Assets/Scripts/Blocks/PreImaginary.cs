using System.Collections.Generic;

public class PreImaginary : Block
{
    protected override void Start()
    {
        base.Start();

        DelayBeforeRefill = 0.01f;
        RefillTime = 0.01f;
    }

    protected override void Update()
    {
        base.Update();


        if (fillAmount <= 0)
        {
            Destroy();
        }
    }

    protected override List<BeamType> GetIgnoringBeamTypes()
    {
        return new List<BeamType>()
        {
            BeamType.Nature,
            BeamType.Creativity,
            BeamType.Fire,
            BeamType.Frost,
            BeamType.Lighting,
            BeamType.Void,
            BeamType.Soul,
        };
    }
}
