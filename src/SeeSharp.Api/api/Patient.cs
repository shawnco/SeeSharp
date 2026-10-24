using Microsoft.EntityFrameworkCore;

// This was the first endpoints file I made, so you'll see a couple comments from me
// about things I'm learning along the way

// TODO: my personal convention is to return data from endpoints in an object shaped like
// { result: ... } for consistency's sake.

// Arguably the namespace should be SeeSharp.Api.Api, with the folder itself being Api instead
// of api.
namespace SeeSharp.Api
{
    public static class PatientEndpoints
    {
        public static IEndpointRouteBuilder MapPatientEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/patients");

            // async and await! I recognize those from Typescript.
            group.MapGet("/", async (SeeSharpContext db) =>
                await db.Patients.AsNoTracking().ToListAsync()
            );

            group.MapGet("/{id}", async (int id, SeeSharpContext db) =>
            {
                var patient = await db.Patients.FindAsync(id);
                return patient is not null ? Results.Ok(patient) : Results.NotFound();
            });

            group.MapPost("/", async (Patient patient, SeeSharpContext db) =>
            {
                db.Patients.Add(patient);
                await db.SaveChangesAsync();
                // The first param returns the value for the Location header. I've never
                // seen this pattern before in Typescript/Python.
                return Results.Created($"/patients/{patient.Id}", patient);
            });

            group.MapPut("/{id}", async (int id, Patient patient, SeeSharpContext db) =>
            {
                var existingPatient = await db.Patients.FindAsync(id);
                if (existingPatient is null) return Results.NotFound();

                existingPatient.Name = patient.Name;
                await db.SaveChangesAsync();
                return Results.Ok(existingPatient);
            });

            group.MapDelete("/{id}", async (int id, SeeSharpContext db) =>
            {
                var patient = await db.Patients.FindAsync(id);
                if (patient is null) return Results.NotFound();

                db.Patients.Remove(patient);
                await db.SaveChangesAsync();
                return Results.Ok(true);
            });

            return app;
        }
    }
}
