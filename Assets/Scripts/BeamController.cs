using System;
using System.Collections.Generic;
using UnityEngine;

public enum BeamType
{
    Fire = 0,
    Void = 1,
    Frost = 2, 
    Lighting = 3, 
    Nature = 4,
    Agony = 5,
    Soul = 6,
    Creativity = 7,
}

[Serializable]
public struct BeamPrefab
{
    public BeamType beamType;
    public GameObject prefab;
}

public class BeamController : MonoBehaviour
{
    [SerializeField] private List<BeamPrefab> beamPrefabs = new List<BeamPrefab>();
    private readonly Dictionary<BeamType, GameObject> _beamDictionary = new Dictionary<BeamType, GameObject>();

    private int _beamTypeCount;
    private BeamType _selectedBeamType;
    private GameObject _activeBeam;
    private PlayerControl _playerControl;
    private Camera _camera;
    
    
    
    void Start()
    {
        _camera = Camera.main;
        _playerControl = GetComponentInParent<PlayerControl>();

        foreach (BeamPrefab beamPrefab in beamPrefabs)
        {
            _beamDictionary.Add(beamPrefab.beamType,beamPrefab.prefab);
        }

        _beamTypeCount = Enum.GetValues(typeof(BeamType)).Length;
    }

    private GameObject GetBeamPrefab()
    {
        if (_beamDictionary.TryGetValue(_selectedBeamType, out GameObject prefab))
        {
            return prefab;
        }

        return null;
    }
    
    void Update()
    {

        if (_activeBeam != null)
        {
            _activeBeam.transform.position = transform.position;
            Vector3 mousePosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-_camera.transform.position.z));
            Vector3 dir = (mousePosition - transform.position);
            float dist = dir.magnitude;
            dir.Normalize();
            _activeBeam.transform.forward = dir;
            
            if (_selectedBeamType == BeamType.Agony)
            {
                Hovl_Laser laser = _activeBeam.GetComponent<Hovl_Laser>();
                laser.SetMaxLength(dist);
            }
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

        if (_playerControl.changeBeamInput > 0)
        {
            SelectNextBeam();
        }
        else if (_playerControl.changeBeamInput < 0)
        {
            SelectPreviousBeam();
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
        _activeBeam = Instantiate(GetBeamPrefab(), transform.position, Quaternion.identity);
        _activeBeam.GetComponent<Hovl_Laser>().SetBeamType(_selectedBeamType);
    }

    void SelectNextBeam()
    {
        int currentBeamIndex = (int)_selectedBeamType;
        if (currentBeamIndex < _beamTypeCount-1) currentBeamIndex++;
        else currentBeamIndex = 0;
        _selectedBeamType = (BeamType)currentBeamIndex;
        
        if(_activeBeam != null)InstantiateBeam();
    }

    void SelectPreviousBeam()
    {
        int currentBeamIndex = (int)_selectedBeamType;
        if (currentBeamIndex > 0) currentBeamIndex--;
        else currentBeamIndex = _beamTypeCount-1;
        _selectedBeamType = (BeamType)currentBeamIndex;
        
        if(_activeBeam != null)InstantiateBeam();
    }
    
    
}
