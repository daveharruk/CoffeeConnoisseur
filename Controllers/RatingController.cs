using CoffeeConnoisseur.Models;
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
    public class RatingController : ControllerBase
    {
        private readonly string _connectionString;

        public RatingController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CoffeeDatabase");
        }

        // GET: api/rating
        [HttpGet]
        public ActionResult OnGet()
        {
            var ratings = new List<Rating>();

            using (var db = new SqlConnection(_connectionString))
            {
                ratings = db.Query<Rating>("select Rating.*, Coffee.CoffeeName from Rating join Coffee on Coffee.CoffeeId = Rating.CoffeeId").ToList();
            }
            return Ok(ratings);
        }

        // PUT: api/rating
        [HttpPut]
        public ActionResult OnPut([FromBody] Rating rating)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                int rows = db.Execute("update Rating set Comment = @Comment, RatingValue = @RatingValue where RatingId = @RatingId", rating);
                if (rows != 1)
                {
                    return NotFound();
                }
            }
            return Ok();
        }

        // POST: api/rating
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Rating rating)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using var db = new SqlConnection(_connectionString);
                    await db.ExecuteAsync("insert into Rating(CoffeeId, Comment, RatingValue) Values(@CoffeeId, @Comment, @RatingValue);", rating);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
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
            catch (Exception)
            {
                return NotFound(StatusCodes.Status404NotFound);
            }
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
