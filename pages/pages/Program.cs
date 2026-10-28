using DBConnect.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var conn_str = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(conn_str));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/students/all", (CollegeDBContext db) =>
{
    return Results.Ok(db.Students.ToList());
});

app.MapGet("/students/{id:int}", (int id, CollegeDBContext db) =>
{
    var student = db.Students.Find(id);
    return student is null
        ? Results.NotFound(new { message = $"Студент с ID={id} не найден" })
        : Results.Ok(student);
});

app.MapPost("/students/add", (Student student, CollegeDBContext db) =>
{
    if (string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
    {
        return Results.BadRequest(new { message = "Имя и фамилия обязательны" });
    }

    db.Students.Add(student);
    db.SaveChanges();
    return Results.Ok(student);
});

app.MapPut("/students/{id:int}", (int id, Student student, CollegeDBContext db) =>
{
    var existing = db.Students.Find(id);
    if (existing is null) return Results.NotFound(new { message = "Студент не найден" });

    existing.FirstName = student.FirstName;
    existing.LastName = student.LastName;
    existing.Age = student.Age;
    existing.GroupId = student.GroupId;

    db.SaveChanges();
    return Results.Ok(existing);
});

app.MapDelete("/students/{id:int}", (int id, CollegeDBContext db) =>
{
    var student = db.Students.Find(id);
    if (student is null) return Results.NotFound(new { message = "Студент не найден" });

    db.Students.Remove(student);
    db.SaveChanges();
    return Results.Ok(new { message = "Студент удален" });
});

app.MapGet("/groups/all", (CollegeDBContext db) =>
{
    return Results.Ok(db.Groups.ToList());
});

app.MapPost("/groups/add", (Group group, CollegeDBContext db) =>
{
    if (string.IsNullOrWhiteSpace(group.GroupName))
    {
        return Results.BadRequest(new { message = "Название группы обязательно" });
    }
    db.Groups.Add(group);
    db.SaveChanges();
    return Results.Ok(group);
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();