using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public KitchenObject GetKitchenObject();
    public void SetKitchenObject(KitchenObject kitchenObject);
    public Transform GetKitchenObjectFollowTransform();
    public void ClearKitchenObject();
    public bool HasKitchenObject();
}
