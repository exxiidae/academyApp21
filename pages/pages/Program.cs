using DBConnect.Data;

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

// ====== ЬРаиагвл API ======
app.MapGet("/students/all", (StudentRepository student_repo) =>
{
    var students = student_repo.GetAll();
    return Results.Ok(students);
});

app.MapGet("/groups/all", (GroupRepository group_repo) =>
{
    var groups = group_repo.GetAll();
    return Results.Ok(groups);
});


app.UseHttpsRedirection();
app.UseDefaultFiles();     
app.UseStaticFiles();      

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();