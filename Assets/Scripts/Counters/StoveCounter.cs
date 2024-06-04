using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOArray;

    private State _state;
    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private float _burningTimer;
    private BurningRecipeSO _burningRecipeSO;

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Frying:
                _fryingTimer += Time.deltaTime;
                if (_fryingTimer >= _fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                    _state = State.Fried;
                    _burningTimer = 0f;
                    _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                }
                break;
            case State.Fried:
                _burningTimer += Time.deltaTime;
                if (_burningTimer >= _burningRecipeSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);

                    _state = State.Burned;
                }
                break;
            case State.Burned:
                break;
        }

    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
                {
                    player.GetKitchenObject().KitchenObjectParent = this;

                    _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);

                    _state = State.Frying;
                    _fryingTimer = 0f;
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().KitchenObjectParent = player;

                _state = State.Idle;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
