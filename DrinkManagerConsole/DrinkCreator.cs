﻿using BLL;
using System;
using System.Collections.Generic;

namespace DrinkManagerConsole
{
    internal class DrinkCreator
    {
        public void AddNewDrink(List<Drink> drinks)
        {
            var newDrink = CreateDrink();
            drinks.Add(newDrink);
            Console.WriteLine($"\nNew drink {newDrink.Name} added.\n");
        }

        private Drink CreateDrink()
        {
            return new Drink()
            {
                Name = Utility.GetGenericData("Drink name: "),
                AlcoholicInfo = Utility.GetAlcoholicInfoFromConsole(),
                GlassType = Utility.GetGenericData("Glass type: "),
                Category = Utility.GetGenericData("Category name: "),
                Ingredients = Utility.GetIngredients(),
                Instructions = Utility.GetGenericData("Instructions: ")
            };
        }
    }
}