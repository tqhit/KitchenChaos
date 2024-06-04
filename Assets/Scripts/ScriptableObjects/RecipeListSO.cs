using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RecipeListSO", menuName = "ScriptableObjects/RecipeListSO", order = 6)]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
