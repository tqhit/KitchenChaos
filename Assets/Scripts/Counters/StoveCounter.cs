using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
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

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progerssNormalized = _fryingTimer / _fryingRecipeSO.fryingTimerMax });

                if (_fryingTimer >= _fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                    _state = State.Fried;
                    _burningTimer = 0f;
                    _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);

                    OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _state });
                }
                break;
            case State.Fried:
                _burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progerssNormalized = _burningTimer / _burningRecipeSO.burningTimerMax });

                if (_burningTimer >= _burningRecipeSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);

                    _state = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _state });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progerssNormalized = 0f });
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

                    OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _state });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progerssNormalized = _fryingTimer / _fryingRecipeSO.fryingTimerMax });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().KitchenObjectParent = player;

                _state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _state });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progerssNormalized = 0f });
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO))
                    {
                        GetKitchenObject().DestroySelf();

                        _state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _state });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progerssNormalized = 0f });
                    }
                }
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

    public bool IsFried()
    {
        return _state == State.Fried;
    }
}
