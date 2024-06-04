using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().KitchenObjectParent = player;

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
