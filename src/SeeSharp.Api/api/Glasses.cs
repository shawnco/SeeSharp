using Microsoft.EntityFrameworkCore;

// TODO: find a use case for returning records instead of entities

namespace SeeSharp.Api
{
    public static class GlassesEndpoints
    {
        public static IEndpointRouteBuilder MapGlassesEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/glasses");

            group.MapGet("/", async (SeeSharpContext db) =>
                await db.Glasses.ToListAsync()
            );

            group.MapGet("/{id}", async (int id, SeeSharpContext db) =>
            {
                var glass = await db.Glasses.FindAsync(id);
                return glass is not null ? Results.Ok(glass) : Results.NotFound();
            });

            group.MapPost("/", async (Glasses glasses, SeeSharpContext db) =>
            {
                db.Glasses.Add(glasses);
                await db.SaveChangesAsync();
                return Results.Created($"/glasses/{glasses.Id}", glasses);
            });

            group.MapPut("/{id}", async (int id, Glasses glasses, SeeSharpContext db) =>
            {
                var existingGlasses = await db.Glasses.FindAsync(id);
                if (existingGlasses is null) return Results.NotFound();

                existingGlasses.Model = glasses.Model;
                await db.SaveChangesAsync();
                return Results.Ok(existingGlasses);
            });
            
            group.MapDelete("/{id}", async (int id, SeeSharpContext db) =>
            {
                var glasses = await db.Glasses.FindAsync(id);
                if (glasses is null) return Results.NotFound();

                db.Glasses.Remove(glasses);
                await db.SaveChangesAsync();
                return Results.Ok(true);
            });

            return app;
        }
    }
}