using UnityEngine;

public enum BlockType
{
    Default,
    Portal,
    Trampoline,
    Ghost,
    Imaginary,
    PreImaginary,
    Frozen
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
    
    
    private Portal _portal1;
    private Portal _portal2;
    private Ghost _ghost;
    private Trampoline _trampoline;
    private Imaginary _imaginary;
    private PreImaginary _preImaginary;
    private Frozen _frozen;
    
    public void OnBlockFilled(Block block,BeamType beamType)
    {
        Vector3 position = block.transform.position;
        
        switch (beamType)
        {
            case BeamType.Void:
                block.Destroy();
                CreatePortal(position);
                break;
            
            case BeamType.Creativity:
                block.Destroy();
                CreateDefault(position);
                break;
            
            case BeamType.Nature:
                block.Destroy();
                CreateTrampoline(position);
                break;
            
            case BeamType.Soul:
                block.Destroy();
                CreateGhost(position);
                break;
            
            case BeamType.Agony:
                block.Destroy();
                CreateImaginary(position);
                break;
            
            case BeamType.Frost:
                block.Destroy();
                CreateFrozen(position);
                break;
        }
    }

    private void CreateDefault(Vector3 position)
    {
        InstantiateBlock(BlockType.Default, position);
    }

    private void CreateFrozen(Vector3 position)
    {
        if(_frozen != null) ResetBlock(_frozen);
        _frozen = InstantiateBlock(BlockType.Frozen,position).GetComponent<Frozen>();
    }

    private void CreateTrampoline(Vector3 position)
    {
        if(_trampoline != null) ResetBlock(_trampoline);
        _trampoline = InstantiateBlock(BlockType.Trampoline,position).GetComponent<Trampoline>();
    }

    private void CreateGhost(Vector3 position)
    {
        if(_ghost != null) ResetBlock(_ghost);
        _ghost = InstantiateBlock(BlockType.Ghost, position).GetComponent<Ghost>();
    }

    private void CreateImaginary(Vector3 position)
    {
        if(_imaginary != null) _imaginary.Destroy();
        _imaginary = InstantiateBlock(BlockType.Imaginary, position).GetComponent<Imaginary>();
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
                ResetBlock(_portal1);
                _portal1 = _portal2;
                _portal2 = portal;
                
                _portal1.SetPair(_portal2);
                _portal2.SetPair(_portal1);
            }
        }
        
    }
    
    public void ResetBlock(Block block)
    {
        Vector3 position = block.transform.position;
        block.Destroy();
        InstantiateBlock(BlockType.Default,position);
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
    }
    
    
}
