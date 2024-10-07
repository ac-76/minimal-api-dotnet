using Microsoft.EntityFrameworkCore;
using MinimalApiDotNet.Data;
using MinimalApiDotNet.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarDb>(opt => opt.UseInMemoryDatabase("CarList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//Add services for a controller base api version
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
    });
}

#region MinimalAPI
//Minimal Api Examples of CRUD
app.MapGet("/", () => "Hello World!");

app.MapGet("/mininal/cars", async (CarDb db) =>
{
    return await db.Cars.ToListAsync();
});

app.MapGet("/minimal/car/{id}", async (string id, CarDb db) =>
{
    return await db.Cars.FirstOrDefaultAsync(x => x.Id == id);
});

app.MapPost("/minimal/car/create", async (Car newCar, CarDb db) =>
{
    db.Cars.Add(newCar);
    await db.SaveChangesAsync();
    return Results.Created($"/minimal/car/create/{newCar.Id}",newCar);
});

app.MapPut("/minimal/car/update", async (Car car, CarDb db) =>
{
    var carInDb = await db.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);

    if (carInDb == null) return Results.NotFound();

    carInDb.Amount = car.Amount;
    carInDb.Make = car.Make;
    carInDb.Model = car.Model;

    db.Update(carInDb);

    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/minimal/car/delete/soft", async (string id, CarDb db) =>
{
    var carInDb = await db.Cars.FirstOrDefaultAsync(x => x.Id == id);

    if (carInDb == null) return Results.NotFound();

    carInDb.IsDeleted = true;

    db.Update(carInDb);

    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("/minimal/car/delete/hard", async (string id, CarDb db) =>
{
    var carInDb = await db.Cars.FirstOrDefaultAsync(x => x.Id == id);

    if (carInDb == null) return Results.NotFound();

    db.Cars.Remove(carInDb);

    await db.SaveChangesAsync();

    return Results.Ok();
});

#endregion

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
