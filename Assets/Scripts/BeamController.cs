using System;
using System.Collections.Generic;
using UnityEngine;

public enum BeamType
{
    Fire,
    Void,
    Frost, 
    Lighting, 
    Nature,
    Agony,
    Soul,
    Creativity,
}

[Serializable]
public struct BeamPrefab
{
    public BeamType beamType;
    public GameObject prefab;
}

[Serializable]
public struct BeamKeyMap
{
    public BeamType beamType;
    public KeyCode keyCode;
}

public class BeamController : MonoBehaviour
{
    [SerializeField] private List<BeamPrefab> beamPrefabs = new List<BeamPrefab>();
    private readonly Dictionary<BeamType, GameObject> _beamDictionary = new Dictionary<BeamType, GameObject>();
    
    private BeamType _selectedBeamType;
    private GameObject _activeBeam;
    private PlayerControl _playerControl;
    private Camera _camera;

    [SerializeField] private List<BeamKeyMap> beamKeyMaps;


    void Start()
    {
        _camera = Camera.main;
        _playerControl = GetComponentInParent<PlayerControl>();

        foreach (BeamPrefab beamPrefab in beamPrefabs)
        {
            _beamDictionary.Add(beamPrefab.beamType,beamPrefab.prefab);
        }
    }

    private void SelectBeamType(BeamType beamType)
    {
        _selectedBeamType = beamType;

        if(_activeBeam != null)InstantiateBeam();
    }
    
    public GameObject GetBeamPrefab(BeamType beamType)
    {
        if (_beamDictionary.TryGetValue(beamType, out GameObject prefab))
        {
            return prefab;
        }

        return null;
    }
    
    void Update()
    {

        if (_activeBeam != null)
        {
            Vector3 pos = transform.position;
            pos.z = 0;
            
            _activeBeam.transform.position = pos;
            Vector3 mousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-_camera.transform.position.z));
            Vector3 dir = (mousePosition - pos);
            float dist = dir.magnitude;
            dir.Normalize();
            _activeBeam.transform.forward = dir;
            

            Hovl_Laser laser = _activeBeam.GetComponent<Hovl_Laser>();
            laser.SetMaxLength(dist);
        }
        
        if (_playerControl.beamInput > 0.1f)
        {
            if (_activeBeam == null)
            {
                InstantiateBeam();
            }
        }
        else
        {
            DestroyActiveBeam();
        }
        
        
        
        //Checking inputs
        foreach (BeamKeyMap beamKeyMap in beamKeyMaps)
        {
            if (Input.GetKeyDown(beamKeyMap.keyCode))
            {
                SelectBeamType(beamKeyMap.beamType);
            }
        }
        
    }

    private void DestroyActiveBeam()
    {
        if (_activeBeam != null)
        {
            Destroy(_activeBeam);
            _activeBeam = null;
        }
    }

    private void InstantiateBeam()
    {
        DestroyActiveBeam();
        _activeBeam = Instantiate(GetBeamPrefab(_selectedBeamType), transform.position, Quaternion.identity);
        _activeBeam.GetComponent<Hovl_Laser>().SetBeamType(_selectedBeamType);
    }
}
