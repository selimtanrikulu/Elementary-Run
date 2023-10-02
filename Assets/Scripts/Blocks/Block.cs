using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class BlockConfig
{
    public readonly float StartFill;
    public readonly float FillTime;
    public readonly float DelayBeforeRefill;
    public readonly float RefillTime;
    
    public BlockConfig(float startFill, float fillTime, float delayBeforeRefill, float refillTime)
    {
        StartFill = startFill;
        FillTime = fillTime;
        DelayBeforeRefill = delayBeforeRefill;
        RefillTime = refillTime;
    }
}

public abstract class Block : MonoBehaviour
{
    private float _delayBeforeRefill;
    private float _refillTime;
    private float _fillTime;
    private float _delayBeforeRefillCounter;
    private float _fillAmount;
    private FillBar _fillBar;
    
    protected BlockController blockController;
    protected BeamType lastBeamingBeamType;
    
    private void Awake()
    {
        blockController = FindObjectOfType<BlockController>();
        _fillBar = GetComponentInChildren<FillBar>(includeInactive:true);
        _delayBeforeRefillCounter = _delayBeforeRefill;

        BlockConfig blockConfig = GetBlockConfig();

        _delayBeforeRefill = blockConfig.DelayBeforeRefill;
        _refillTime = blockConfig.RefillTime;
        _fillTime = blockConfig.FillTime;
        _fillAmount = blockConfig.StartFill;
        _fillBar.gameObject.SetActive(_fillAmount>0.001f);
        _fillBar.UpdateFill(_fillAmount);

        AwakeTail();
    }
    private void Update()
    {
        if (_delayBeforeRefillCounter < 0)
        {
            Refill(Time.deltaTime*(1/_refillTime));
        }
        else
        {
            _delayBeforeRefillCounter -= Time.deltaTime;
        }

        UpdateTail();
    }
    private void OnDestroy()
    {
        _fillBar.fillGameObject.transform.DOKill();
    }
    public void Beaming(BeamType beamType)
    {
        if (GetFillerBeamTypes().Contains(beamType))
        {
            lastBeamingBeamType = beamType;
            Fill(Time.deltaTime * (1/_fillTime));
            return;
        } 
        if (GetReFillerBeamTypes().Contains(beamType))
        {
            lastBeamingBeamType = beamType;
            float refillerSpeed = 1;
            Refill(Time.deltaTime*(1/refillerSpeed));
        }
    }
    private void Fill(float amount)
    {
        _fillAmount = Mathf.Min(1, _fillAmount + amount);
        _fillBar.UpdateFill(_fillAmount);
        _fillBar.gameObject.SetActive(true);
        _delayBeforeRefillCounter = _delayBeforeRefill;

        if (_fillAmount >= 0.9999f)
        {
            OnFilled();
        }
    }
    private void Refill(float amount)
    {
        _fillAmount = Mathf.Max(0, _fillAmount - amount);
        _fillBar.UpdateFill(_fillAmount);

        if (_fillAmount <= 0.0001f)
        {
            _fillBar.gameObject.SetActive(false);
            OnRefilled();
        }
    }
    protected void ResetFillAmount()
    {
        _fillAmount = GetBlockConfig().StartFill;
        _fillBar.gameObject.SetActive(_fillAmount>0.001f);
        _fillBar.UpdateFill(_fillAmount);
    }
    protected abstract BlockConfig GetBlockConfig();
    protected abstract void AwakeTail();
    protected abstract void UpdateTail();
    protected abstract void OnFilled();
    protected abstract void OnRefilled();
    public abstract void Destroy();
    protected abstract List<BeamType> GetFillerBeamTypes();
    protected abstract List<BeamType> GetReFillerBeamTypes();
}
