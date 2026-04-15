using Microsoft.AspNetCore.Mvc;
using APBD_cw5.Models;

namespace APBD_cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private static List<Reservation> _reservations = new()
    {
        new Reservation
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Jan Kowalski",
            Topic = "Spotkanie zespołu",
            Date = new DateTime(2025, 5, 10),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(11, 0, 0),
            Status = ReservationStatus.Planned
        },
        new Reservation
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Anna Nowak",
            Topic = "Prezentacja",
            Date = new DateTime(2025, 5, 11),
            StartTime = new TimeSpan(12, 0, 0),
            EndTime = new TimeSpan(13, 30, 0),
            Status = ReservationStatus.Confirmed
        },
        new Reservation
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Piotr Wiśniewski",
            Topic = "Szkolenie",
            Date = new DateTime(2025, 5, 12),
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(12, 0, 0),
            Status = ReservationStatus.Planned
        },
        new Reservation
        {
            Id = 4,
            RoomId = 1,
            OrganizerName = "Katarzyna Zielińska",
            Topic = "Warsztaty",
            Date = new DateTime(2025, 5, 13),
            StartTime = new TimeSpan(14, 0, 0),
            EndTime = new TimeSpan(16, 0, 0),
            Status = ReservationStatus.Cancelled
        },
        new Reservation
        {
            Id = 5,
            RoomId = 4,
            OrganizerName = "Tomasz Lewandowski",
            Topic = "Rekrutacja",
            Date = new DateTime(2025, 5, 14),
            StartTime = new TimeSpan(11, 0, 0),
            EndTime = new TimeSpan(12, 0, 0),
            Status = ReservationStatus.Confirmed
        }
    };
}