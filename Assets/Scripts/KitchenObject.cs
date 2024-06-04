using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private ClearCounter _clearCounter;

    public KitchenObjectSO KitchenObjectSO => _kitchenObjectSO;

    public ClearCounter ClearCounter
    {
        get => _clearCounter;
        set
        {
            if (_clearCounter != null)
                _clearCounter.ClearKitchenObject();

            _clearCounter = value;

            if (value.HasKitchenObject())
                Debug.LogError("Clear Counter already has a kitchen object");

            value.KitchenObject = this;

            transform.parent = _clearCounter.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }
    }
}
