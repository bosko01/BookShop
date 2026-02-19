using BookShop.API.ErrorHandling;
using BookShop.Application.Author.Services;
using BookShop.Application.Book.Contracts;
using BookShop.Application.Book.Services;
using BookShop.Application.Book.Validation;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Infrastructure.Persistence;
using BookShop.Infrastructure.Persistence.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Ensure Swagger generator is registered so app.UseSwagger() / UseSwaggerUI() are available.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<BookShopDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.MapControllers();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.Run();

