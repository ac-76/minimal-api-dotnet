using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinimalApiDotNet.Data;
using MinimalApiDotNet.Models;

namespace MinimalApiDotNet.Controllers
{
    [Route("mc")]
    public class CarsController : Controller
    {
        private readonly CarDb _context;

        public CarsController(CarDb context)
        {
            _context = context;
        }

        
        [HttpGet("cars")]
        public async Task<IList<Car>> GetAllCars()
        {
            return await _context.Cars.ToListAsync();
        }

        [HttpGet("car/")]
        public async Task<Car> GetCarById([FromQuery]string id)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        }


        [HttpPost("car/create")]
        public async Task<IActionResult> Create([Bind("Id,Model,Make,Year,Amount,IsDeleted")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetAllCars));
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("car/update")]
        public async Task<IActionResult> Update([Bind("Id,Model,Make,Year,Amount,IsDeleted")] Car car)
        {
            if (car.Id == null)
            {
                return NotFound();
            }
            var carInDb = await _context.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);

            if (carInDb == null) return NotFound();

            carInDb.Amount = car.Amount;
            carInDb.Make = car.Make;
            carInDb.Model = car.Model;

            _context.Update(carInDb);

            await _context.SaveChangesAsync();

            return Ok(carInDb);
        }

        [HttpDelete("car/delete/{id}")]
        public async Task<IActionResult> DeleteCar(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var carInDb = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

            if (carInDb == null) return NotFound();

            _context.Cars.Remove(carInDb);

            await _context.SaveChangesAsync();
            return Ok("Successfully Deleted");
        }
    }
}
