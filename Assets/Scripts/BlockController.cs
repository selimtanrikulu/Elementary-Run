using UnityEngine;

public enum BlockType
{
    Default,
    Portal,
    Trampoline,
    Ghost,
    Imaginary,
    PreImaginary,
    Frozen,
    Mirror,
    Magma
}


public class BlockController : MonoBehaviour
{

    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private GameObject defaultPrefab;
    [SerializeField] private GameObject trampolinePrefab;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject imaginaryPrefab;
    [SerializeField] private GameObject preImaginaryPrefab;
    [SerializeField] private GameObject frozenPrefab;
    [SerializeField] private GameObject mirrorPrefab;
    [SerializeField] private GameObject magmaPrefab;
    
    
    private Portal _portal1;
    private Portal _portal2;
    private Ghost _ghost;
    private Trampoline _trampoline;
    private Imaginary _imaginary;
    private PreImaginary _preImaginary;
    private Frozen _frozen;
    private Mirror _mirror;
    private Magma _magma;
    
    public void CreateBlockByBeamType(BeamType beamType, Vector3 position)
    {

        switch (beamType)
        {
            case BeamType.Void:
                CreatePortal(position);
                break;
            
            case BeamType.Creativity:
                CreateDefault(position);
                break;
            
            case BeamType.Nature:
                CreateTrampoline(position);
                break;
            
            case BeamType.Soul:
                CreateGhost(position);
                break;
            
            case BeamType.Agony:
                CreateImaginary(position);
                break;
            
            case BeamType.Frost:
                CreateFrozen(position);
                break;
            
            case BeamType.Lighting:
                CreateMirror(position);
                break;
            
            case BeamType.Fire:
                CreateMagma(position);
                break;
        }
    }

    private void CreateDefault(Vector3 position)
    {
        InstantiateBlock(BlockType.Default, position);
    }

    private void CreateFrozen(Vector3 position)
    {
        if(_frozen != null) _frozen.Destroy();
        _frozen = InstantiateBlock(BlockType.Frozen,position).GetComponent<Frozen>();
    }

    private void CreateTrampoline(Vector3 position)
    {
        if(_trampoline != null) _trampoline.Destroy();
        _trampoline = InstantiateBlock(BlockType.Trampoline,position).GetComponent<Trampoline>();
    }

    private void CreateGhost(Vector3 position)
    {
        if(_ghost != null) _ghost.Destroy();
        _ghost = InstantiateBlock(BlockType.Ghost, position).GetComponent<Ghost>();
    }
    
    private void CreateMagma(Vector3 position)
    {
        if(_magma != null) _magma.Destroy();
        _magma = InstantiateBlock(BlockType.Magma, position).GetComponent<Magma>();
    }

    private void CreateImaginary(Vector3 position)
    {
        if(_imaginary != null) _imaginary.Destroy();
        _imaginary = InstantiateBlock(BlockType.Imaginary, position).GetComponent<Imaginary>();
    }
    
    private void CreateMirror(Vector3 position)
    {
        if(_mirror != null) _mirror.Destroy();
        _mirror = InstantiateBlock(BlockType.Mirror, position).GetComponent<Mirror>();
    }
    
    private void CreatePortal(Vector3 position)
    {
        Block instantiatedBlock = InstantiateBlock(BlockType.Portal, position);
        Portal portal = instantiatedBlock.GetComponent<Portal>();


        if (_portal1 == null)
        {
            if (_portal2 == null)
            {
                //Both not in use
                _portal1 = portal;

            }
            else
            {
                //portal2 in use, portal1 is not in use
                _portal1 = portal;

                _portal1.SetPair(_portal2);
                _portal2.SetPair(_portal1);
            }
        }
        else
        {
            if (_portal2 == null)
            {
                //portal1 in use, portal 2 is not in use
                _portal2 = portal;
                _portal1.SetPair(_portal2);
                _portal2.SetPair(_portal1);
            }
            else
            {
                //Both used, change portal1
                _portal1.ResetPair();
                _portal1.Destroy();

                _portal1 = _portal2;
                _portal2 = portal;
                
                _portal1.SetPair(_portal2);
                _portal2.SetPair(_portal1);
                
                
            }
        }
        
    }

    

    private Block InstantiateBlock(BlockType blockType,Vector3 position)
    {
        Block result;

        switch (blockType)
        {
            case BlockType.Portal: 
                result = Instantiate(portalPrefab, position,Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Default:
                result = Instantiate(defaultPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Trampoline:
                result = Instantiate(trampolinePrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Ghost:
                result = Instantiate(ghostPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Imaginary:
                result = Instantiate(imaginaryPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.PreImaginary:
                result = Instantiate(preImaginaryPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Frozen:
                result = Instantiate(frozenPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Mirror:
                result = Instantiate(mirrorPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            case BlockType.Magma:
                result = Instantiate(magmaPrefab, position, Quaternion.identity).GetComponent<Block>();
                break;
            
            default:
                return null;
        }

        return result;
    }

    public void AgonyHitAt(Vector3 position)
    {
        Vector3 normalizedPosition = new Vector3(Mathf.RoundToInt(position.x),Mathf.RoundToInt(position.y), position.z);
        if (_preImaginary != null)
        {
            Vector3 pos = _preImaginary.transform.position;
            if ((pos - normalizedPosition).magnitude < 0.1f) return;
            
            _preImaginary.Destroy();
        }
        
        _preImaginary = InstantiateBlock(BlockType.PreImaginary, normalizedPosition).GetComponent<PreImaginary>();
        
        if(_imaginary)_imaginary.Destroy();
    }
    
    
}
