using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _cutKitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().KitchenObjectParent = this;
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().KitchenObjectParent = player;
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(_cutKitchenObjectSO, this);
        }
        else
        {

        }
    }
}
