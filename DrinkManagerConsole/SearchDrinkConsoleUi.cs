﻿using BLL;
using BLL.Enums;
using System;
using System.Collections.Generic;

namespace DrinkManagerConsole
{
    public static class SearchDrinkConsoleUi
    {
        public static void StartSearch(List<Drink> drinksList, SearchCriterion searchCriterion)
        {
            bool continueSearch = true;
            do
            {
                Console.Clear();
                Console.WriteLine($"\nSearch drink by {searchCriterion}\n".ToUpper());
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");

                List<Drink> drinksFound = null;

                switch (searchCriterion)
                {
                    case SearchCriterion.Name:
                        Console.Write($"\nEnter a drink {searchCriterion.ToString().ToLower()} to find: ");
                        drinksFound = SearchDrink.SearchByName(Console.ReadLine(), drinksList);
                        break;
                    case SearchCriterion.Ingredients:
                        Console.WriteLine("\nInstructions: \nYou can provide ONE or MORE ingredients - separated with a space. \nYou can search drinks containing ALL or ANY of provided ingredients.");
                        Console.Write("\nWould you like to display drinks containing: \n1. ALL provided ingredients \n2. ANY of provided ingredients\n(1/2) ");
                        SearchDrinkOption searchOption;
                        switch (Console.ReadKey().KeyChar)
                        {
                            case '1':
                                searchOption = SearchDrinkOption.All;
                                break;
                            case '2':
                                searchOption = SearchDrinkOption.Any;
                                break;
                            default:
                                Console.WriteLine("\nI don't know what you mean - try again :)");
                                searchOption = SearchDrinkOption.Any; //default initialization of local variable - default choice in case user fails to choose 
                                break;
                        }
                        Console.Write($"\n\nEnter a drink {searchCriterion.ToString().ToLower()} to find: ");
                        drinksFound = SearchDrink.SearchByIngredients(new SortedSet<string>(Console.ReadLine()?.Split(' ') ?? throw new InvalidOperationException()), drinksList, searchOption);
                        break;
                }

                if (drinksFound?.Count == 0 || drinksFound == null)
                {
                    Console.WriteLine("\nNo matching drinks in our database.");
                }
                else
                {
                    foreach (var drink in drinksFound)
                    {
                        Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("Name:".PadRight(20) + drink.Name.PadRight(30) + drink.AlcoholicInfo);
                        Console.WriteLine("Category:".PadRight(20) + drink.Category);
                        Console.WriteLine("Glass type:".PadRight(20) + drink.GlassType);
                        Console.WriteLine("\nIngredients: ");
                        foreach (var ingredient in drink.Ingredients)
                        {
                            if (ingredient.Name == null)
                            {
                                continue;
                            }
                            Console.WriteLine(ingredient.Name.PadRight(20) + ingredient.Amount);
                        }
                        Console.WriteLine($"\nInstructions:\n{drink.Instructions}");
                    }
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
                }

                Console.Write($"\nContinue search by {searchCriterion.ToString().ToLower()} (y/n)? ");
                if (Console.ReadKey().KeyChar == 'n')
                {
                    continueSearch = false;
                }
            } while (continueSearch);
        }
        /// <summary>
        /// Shows search criteria menu, gets user input and cause GetDrinksByAlcoholContent to run
        /// </summary>
        /// <param name="drinks"></param>
        public static void HandleSearchDrinksByContentInConsole(List<Drink> drinks)
        {
            Console.Clear();
            Console.WriteLine("Choose one of the searching criteria: ");
            Console.WriteLine("1. Alcoholic drinks");
            Console.WriteLine("2. Non alcoholic drinks");
            Console.WriteLine("3. Optional alcohol drinks");
            Console.WriteLine("4. Alcoholic and optional alcohol drinks");
            Console.WriteLine("5. Non alcoholic and optional alcohol drinks");
            Console.WriteLine("Press any other key to go back to previous menu");

            var searchChoice = Console.ReadKey();
            if (searchChoice.Key == ConsoleKey.D1 || searchChoice.Key == ConsoleKey.D2 || searchChoice.Key == ConsoleKey.D3 || searchChoice.Key == ConsoleKey.D4 || searchChoice.Key == ConsoleKey.D5)
            {
                GetDrinksByAlcoholContent(searchChoice, drinks);
            }
        }
        /// <summary>
        /// Depending on user input from HandleSearchDrinksByContentinConsole, causes SearchByAlcoholContent to run with specified search criteria
        /// </summary>
        /// <param name="key"></param>
        /// <param name="drinks"></param>
        /// <returns></returns>
        public static List<Drink> GetDrinksByAlcoholContent(ConsoleKeyInfo key, List<Drink> drinks)
        {
            var contemporaryList = new List<Drink>();
            string alcoholicInfo;
            Console.Clear();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    {
                        alcoholicInfo = "Alcoholic";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        alcoholicInfo = "Non alcoholic";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        alcoholicInfo = "Optional alcohol";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        break;
                    }
                case ConsoleKey.D4:
                    {
                        alcoholicInfo = "Alcoholic";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        alcoholicInfo = "Optional alcohol";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        break;
                    }
                case ConsoleKey.D5:
                    {
                        alcoholicInfo = "Non alcoholic";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        alcoholicInfo = "Optional alcohol";
                        contemporaryList = SearchDrink.SearchByAlcoholContent(alcoholicInfo, drinks, contemporaryList);
                        break;
                    }
            }
            return contemporaryList;
        }
    }
}
