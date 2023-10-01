using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    protected float DelayBeforeRefill = 1f;
    protected float RefillTime = 4f;
    private const float FillTime = 0.5f;
    private float _delayBeforeRefillCounter;
    private FillBar _fillBar;
    private BeamType _lastBeamingBeamType;
    protected float fillAmount;
    protected BlockController blockController;

    protected virtual void Start()
    {
        blockController = FindObjectOfType<BlockController>();
        _fillBar = GetComponentInChildren<FillBar>(includeInactive:true);

        _delayBeforeRefillCounter = DelayBeforeRefill;
    }

    protected virtual void Update()
    {
        if (_delayBeforeRefillCounter < 0)
        {
            UpdateFillAmount(-Time.deltaTime*(1/RefillTime));
        }
        else
        {
            _delayBeforeRefillCounter -= Time.deltaTime;
        }
    }
    

    //Call is on Update
    public void Beaming(BeamType beamType)
    {
        if(GetIgnoringBeamTypes().Contains(beamType))return;
        
        if (_lastBeamingBeamType != beamType)
        {
            fillAmount = 0;
        }
        
        _lastBeamingBeamType = beamType;
        _delayBeforeRefillCounter = DelayBeforeRefill;
        
        UpdateFillAmount(Time.deltaTime * (1/FillTime));
    }

    protected abstract List<BeamType> GetIgnoringBeamTypes();

    public virtual void Destroy()
    {
        _fillBar.fillGameObject.transform.DOKill();
        Destroy(gameObject);
    }


    protected void UpdateFillAmount(float amount)
    {
        float tempAmount = fillAmount += amount;
        if (tempAmount < 0) tempAmount = 0;
        else if (tempAmount > 1) tempAmount = 1;


        fillAmount = tempAmount;
        _fillBar.UpdateFill(fillAmount);


        if (fillAmount >= 0.999f)
        {
            OnFilled();
            fillAmount = 0;
        }


        _fillBar.gameObject.SetActive(fillAmount > 0);
        
    }



    protected virtual void OnFilled()
    {
        blockController.OnBlockFilled(this,_lastBeamingBeamType);
    }

}
