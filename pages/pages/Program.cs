using DBConnect.Data;
using DBConnect.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var conn_str = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new StudentRepository(conn_str));
builder.Services.AddSingleton(new GroupRepository(conn_str));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/students/all", (StudentRepository student_repo) =>
{
    return Results.Ok(student_repo.GetAll());
});

app.MapGet("/students/{id:int}", (int id, StudentRepository student_repo) =>
{
    var student = student_repo.GetById(id);
    return student is null
        ? Results.NotFound(new { message = $"Студент с ID={id} не найден" })
        : Results.Ok(student);
});

app.MapPost("/students/add", (Student student, StudentRepository student_repo) =>
{
    if (string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName))
    {
        return Results.BadRequest(new { message = "Имя и фамилия обязательны" });
    }

    int newId = student_repo.CreateStudent(student);
    student.StudentId = newId;
    return Results.Ok(student);
});

app.MapGet("/groups/all", (GroupRepository group_repo) =>
{
    return Results.Ok(group_repo.GetAll());
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();