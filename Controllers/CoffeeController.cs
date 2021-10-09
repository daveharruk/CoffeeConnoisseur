﻿using CoffeeConnoisseur.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeConnoisseur.Controllers
{
    [Route("api/[controller]")]
    public class CoffeeController : ControllerBase
    {
        private readonly string _connectionString;
        
        public CoffeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CoffeeDatabase");
        }

        // GET: api/coffee
        [HttpGet]
        public ActionResult OnGet()
        {
            var coffees = new List<Coffee>();

            using (var db = new SqlConnection(_connectionString))
            {
                coffees = db.Query<Coffee>("select * from Coffee").ToList();
            }
            return Ok(coffees);
        }

        // GET: api/coffee/1
        [HttpGet("{id}")]
        public ActionResult OnGet(int id)
        {
            var ratings = new List<Rating>();
            try
            {
                using var db = new SqlConnection(_connectionString);
                // Add number of ratings to ratingId column...
                ratings = db.Query<Rating>(@"select Rating.*, Coffee.CoffeeName from Rating
                                             join Coffee on Rating.CoffeeId = Coffee.CoffeeId
                                             where Rating.CoffeeId = @CoffeeId
                                             union all
                                             select count(*), min(Coffee.CoffeeId), 'Average', avg(Rating.RatingValue), min(Coffee.CoffeeName) from Rating
                                             join Coffee on Rating.CoffeeId = Coffee.CoffeeId
                                             where Rating.CoffeeId = @CoffeeId",
                                           new Dictionary<string, object>() { { "CoffeeId", id } }).ToList();
            }
            catch(Exception)
            {
                return NotFound();
            }
            return Ok(ratings);
        }

        // POST: api/coffee
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Coffee coffee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var db = new SqlConnection(_connectionString);
                    await db.ExecuteAsync("insert into Coffee(CoffeeName) Values(@CoffeeName);", coffee);
                }
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status201Created);
        }

        // DELETE: api/coffee/3
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using var db = new SqlConnection(_connectionString);
                await db.ExecuteAsync("delete from Coffee where CoffeeId = @CoffeeId;", new Dictionary<string, object>() { { "CoffeeId", id } });
            }
            catch(Exception)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}