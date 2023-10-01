using DG.Tweening;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    [SerializeField] public GameObject fillGameObject;
    [SerializeField] private float fillUpdateDuration;
    
    public void UpdateFill(float fillAmount)
    {
        fillGameObject.transform.DOKill();
        fillGameObject.transform.DOScaleX(fillAmount, fillUpdateDuration);
    }
}
