using UnityEngine;

public class Hovl_Laser : MonoBehaviour
{
    private BeamType _beamType;

    public int damageOverTime = 30;

    public GameObject HitEffect;
    public float HitOffset = 0;
    public bool useLaserRotation = false;

    public float MaxLength;
    private LineRenderer Laser;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1,1,1,1);

    //One activation per shoot
    private bool LaserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;


    private BlockController _blockController;
    
    void Start ()
    {
        //Get LineRender and ParticleSystem components from current prefab;  
        _blockController = FindObjectOfType<BlockController>();
        Laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));                    
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        //To set LineRender position
        if (Laser != null && UpdateSaver == false)
        {
            Laser.SetPosition(0, transform.position);

            bool hit1Occurred = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out RaycastHit hit1,
                MaxLength,
                LayerMask.GetMask("Block"));

            bool hit2Occurred = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out RaycastHit hit2,
                MaxLength,
                LayerMask.GetMask("Mirror"));

            if (hit1Occurred && (!hit2Occurred || (hit1.distance < hit2.distance)))
            {
                //End laser position if collides with object
                Laser.SetPosition(1, hit1.point);

                HitEffect.transform.position = hit1.point + hit1.normal * HitOffset;
                if (useLaserRotation)
                    HitEffect.transform.rotation = transform.rotation;
                else
                    HitEffect.transform.LookAt(hit1.point + hit1.normal);

                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit1.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit1.point));

                if (hit1.collider.TryGetComponent(out Block block))
                {
                    block.Beaming(_beamType);
                }

                if (hit1.collider.TryGetComponent(out Mirror mirror))
                {
                    Vector3 incomingVector = hit1.point - hit1.collider.transform.position;
                    Vector3 reflectVector = Vector3.Reflect(incomingVector, hit1.normal);
                    
                    mirror.OnLaserStay(_beamType,hit1.point,reflectVector);
                }
                
            }
            else if (hit2Occurred)
            {
                //End laser position if collides with object
                Laser.SetPosition(1, hit2.point);
                
                foreach (var AllPs in Effects)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
                
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit2.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit2.point));

                if (hit2.collider.TryGetComponent(out Block block))
                {
                    block.Beaming(_beamType);
                }

                if (hit2.collider.TryGetComponent(out Mirror mirror))
                {
                    Vector3 incomingVector = transform.forward;
                    Vector3 reflectVector = Vector3.Reflect(incomingVector, hit2.normal);
                    
                    mirror.OnLaserStay(_beamType,hit2.point,reflectVector);
                }
                
            }
            else
            {
                //End laser position if doesn't collide with object
                var EndPos = transform.position + transform.forward * MaxLength;
                Laser.SetPosition(1, EndPos);
                /*HitEffect.transform.position = EndPos;
                foreach (var AllPs in Hit)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));*/
                
                HitEffect.transform.position = EndPos;
                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit1.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit1.point));


                if (_beamType == BeamType.Agony)
                {
                    _blockController.AgonyHitAt(EndPos);
                }
            }
            //Insurance against the appearance of a laser in the center of coordinates!
            if (Laser.enabled == false && LaserSaver == false)
            {
                LaserSaver = true;
                Laser.enabled = true;
            }
        }  
    }
    
    public void SetBeamType(BeamType beamType)
    {
        _beamType = beamType;
    }

    public void SetMaxLength(float length)
    {
        MaxLength = length;
    }

    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
    }
}
