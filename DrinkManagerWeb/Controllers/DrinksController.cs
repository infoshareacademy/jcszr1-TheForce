﻿using BLL;
using DrinkManagerWeb.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DrinkManagerWeb.Controllers
{
    public class DrinksController : Controller
    {
        private readonly DrinkAppContext _db;

        public DrinksController(DrinkAppContext db)
        {
            _db = db;
        }

        public IActionResult Index(string sortOrder, int? pageNumber)
        {
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                pageNumber = 1;

                var drinks = _db.Drinks.AsQueryable();

                switch (sortOrder)
                {
                    case "name_desc":
                        drinks = drinks.OrderByDescending(s => s.Name);
                        break;
                    default:    
                        drinks = drinks.OrderBy(s => s.Name);
                        break;
                }

                int pageSize = 10;
                return View(PaginatedList<Drink>.CreatePaginatedList(drinks, pageNumber ?? 1, pageSize));
        }
    }
}