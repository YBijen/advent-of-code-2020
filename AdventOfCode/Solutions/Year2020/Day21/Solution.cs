using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Day21 : ASolution
    {
        private readonly Dictionary<string, List<List<string>>> _allergenIngredientsCollection = new Dictionary<string, List<List<string>>>();
        private readonly List<string> _allIngredients = new List<string>();

        public Day21() : base(21, 2020, "")
        {
            //SetDebugInput();
            ParseInput();
        }

        protected override string SolvePartOne()
        {
            var ingredientsWithAllergen = _allergenIngredientsCollection
                .SelectMany(allergenKvp => 
                    allergenKvp.Value.Aggregate((v1, v2) => v1.Intersect(v2)
                    .ToList()))
                .Distinct();

            var countIngredientWithoutAllergen = 0;
            foreach(var ingredient in _allIngredients)
            {
                if(!ingredientsWithAllergen.Contains(ingredient))
                {
                    countIngredientWithoutAllergen++;
                }
            }

            return countIngredientWithoutAllergen.ToString();
        }

        protected override string SolvePartTwo()
        {
            // Remove all ingredients which are not allergen
            var allergenIngredientCollection = _allergenIngredientsCollection
                .ToDictionary(allergenKvp => allergenKvp.Key, allergenKvp => allergenKvp.Value.Aggregate((v1, v2) => v1.Intersect(v2).ToList()));

            var dangerousAllergenIngredients = new List<(string Allergen, string Ingredient)>();

            while(dangerousAllergenIngredients.Count < allergenIngredientCollection.Count)
            {
                var nextAllergenIngredient = allergenIngredientCollection.First(aic => aic.Value.Count == 1);
                dangerousAllergenIngredients.Add((nextAllergenIngredient.Key, nextAllergenIngredient.Value[0]));

                foreach(var aic in allergenIngredientCollection)
                {
                    if(aic.Key != nextAllergenIngredient.Key)
                    {
                        aic.Value.RemoveAll(ingredient => ingredient == nextAllergenIngredient.Value[0]);
                    }
                }

                allergenIngredientCollection[nextAllergenIngredient.Key].Clear();
            }

            return string.Join(",", dangerousAllergenIngredients.OrderBy(dai => dai.Allergen).Select(dai => dai.Ingredient));
        }

        private void ParseInput()
        {
            _allIngredients.Clear();
            foreach (var food in base.Input.SplitByNewline())
            {
                var ingredients = food.Split('(')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                _allIngredients.AddRange(ingredients);

                var allergens = food.Split('(')[1].Replace("contains ", "").Replace(")", "").Split(", ");

                foreach(var allergen in allergens)
                {
                    if (_allergenIngredientsCollection.ContainsKey(allergen))
                    {
                        _allergenIngredientsCollection[allergen].Add(ingredients);
                    }
                    else
                    {
                        _allergenIngredientsCollection.Add(allergen, new List<List<string>> { ingredients });
                    }
                }
            }
        }

        private void SetDebugInput()
        {
            base.DebugInput = "" +
                "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)\n" +
                "trh fvjkl sbzzf mxmxvkd (contains dairy)\n" +
                "sqjhc fvjkl (contains soy)\n" +
                "sqjhc mxmxvkd sbzzf (contains fish)";
        }
    }
}
