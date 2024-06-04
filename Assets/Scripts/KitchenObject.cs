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

    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.KitchenObjectParent = kitchenObjectParent;

        return kitchenObject;
    }
}
