using Microsoft.EntityFrameworkCore;

namespace SeeSharp.Api
{
    public static class AppointmentEndpoints
    {
        public static IEndpointRouteBuilder MapAppointmentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/appointments");

            group.MapGet("/", async (SeeSharpContext db) =>
                await db.Appointments.ToListAsync()
            );

            group.MapGet("/{id}", async (int id, SeeSharpContext db) =>
            {
                var appointment = await db.Appointments.FindAsync(id);
                return appointment is not null ? Results.Ok(appointment) : Results.NotFound();
            });

            group.MapPost("/", async (Appointment appointment, SeeSharpContext db) =>
            {
                db.Appointments.Add(appointment);
                await db.SaveChangesAsync();
                return Results.Created($"/appointments/{appointment.Id}", appointment);
            });

            group.MapPut("/{id}", async (int id, Appointment appointment, SeeSharpContext db) =>
            {
                var existingAppointment = await db.Appointments.FindAsync(id);
                if (existingAppointment is null) return Results.NotFound();

                existingAppointment.Date = appointment.Date;
                existingAppointment.PatientId = appointment.PatientId;
                await db.SaveChangesAsync();
                return Results.Ok(existingAppointment);
            });

            group.MapDelete("/{id}", async (int id, SeeSharpContext db) =>
            {
                var appointment = await db.Appointments.FindAsync(id);
                if (appointment is null) return Results.NotFound();

                db.Appointments.Remove(appointment);
                await db.SaveChangesAsync();
                return Results.Ok(true);
            });

            return app;
        }
    }
}