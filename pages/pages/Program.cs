using DBConnect.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var conn_str = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new StudentRepository(conn_str));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.MapGet("/students/all", (StudentRepository student_repo) =>
{
    var students = student_repo.GetAll();
    return Results.Ok(students);
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
