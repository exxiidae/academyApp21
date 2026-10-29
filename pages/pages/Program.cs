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

app.MapGet("/students/all", (CollegeDBContext db) => Results.Ok(db.Students.ToList()));

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

app.MapGet("/teachers/all", (CollegeDBContext db) => Results.Ok(db.Teachers.ToList()));

app.MapPost("/teachers/add", (Teacher teacher, CollegeDBContext db) =>
{
    if (string.IsNullOrWhiteSpace(teacher.FirstName) || string.IsNullOrWhiteSpace(teacher.LastName))
    {
        return Results.BadRequest(new { message = "Имя и фамилия обязательны" });
    }
    db.Teachers.Add(teacher);
    db.SaveChanges();
    return Results.Ok(teacher);
});

app.MapGet("/groups/all", (CollegeDBContext db) => Results.Ok(db.Groups.ToList()));

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();