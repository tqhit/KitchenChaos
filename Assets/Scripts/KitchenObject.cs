using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;

    public KitchenObjectSO KitchenObjectSO => _kitchenObjectSO;

    public IKitchenObjectParent KitchenObjectParent
    {
        get => _kitchenObjectParent;
        set
        {
            if (_kitchenObjectParent != null)
                _kitchenObjectParent.ClearKitchenObject();

            _kitchenObjectParent = value;

            if (value.HasKitchenObject())
                Debug.LogError("IKitchenObjectParent already has a kitchen object");

            value.SetKitchenObject(this);

            transform.parent = _kitchenObjectParent.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }
    }
}
