using Microsoft.AspNetCore.Mvc;
using APBD_cw5.Models;

namespace APBD_cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    public static List<Reservation> _reservations = new()
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
            Status = "planned"
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
            Status = "confirmed"
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
            Status = "planned"
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
            Status = "cancelled"
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
            Status = "confirmed"
        }
    };

    [HttpGet]
    public IActionResult Get(
        [FromQuery] DateTime? date, string? status, int? roomId)
    {
        var reservations = _reservations;
        
        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value).ToList();
        if (!string.IsNullOrEmpty(status))
            reservations = reservations.Where(r => r.Status == status).ToList();
        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value).ToList();
        
        if (reservations.Any())
            return Ok(reservations);
        
        return NotFound("Brak rezerwacji o podanych filtrach.");
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id: {id} nie istnieje");
        }
        
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Reservation reservation)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var room = RoomsController._rooms
            .FirstOrDefault(r => r.Id == reservation.RoomId);
        
        if(room is null || !room.IsActive)
            return BadRequest("Pokój nie istnieje lub jest nieaktywny");

        if (reservation.EndTime <= reservation.StartTime)
            return BadRequest("Spotkanie nie może mieć godziny skończenia przed godziną rozpoczęcia");
        
        var conflict = _reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime
        );

        if (conflict)
            return Conflict("Czas spotkania nakłada się już na inne spotkanie");
        
        reservation.Id = _reservations.Max(r => r.Id) + 1;
        _reservations.Add(reservation);
        
        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Reservation updatedReservation)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (updatedReservation.EndTime <= updatedReservation.StartTime)
            return BadRequest("Spotkanie nie może mieć godziny skończenia przed godziną rozpoczęcia");

        var reservation = _reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null)
            return NotFound();
        
        var room = RoomsController._rooms
            .FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        
        if(room is null || !room.IsActive)
            return BadRequest("Pokój nie istnieje lub jest nieaktywny");
        
        var conflict = _reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updatedReservation.RoomId &&
            r.Date == updatedReservation.Date &&
            updatedReservation.StartTime < r.EndTime &&
            updatedReservation.EndTime > r.StartTime
        );

        if (conflict)
            return Conflict("Czas spotkania nakłada się już na inne spotkanie");
        
        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null)
            return NotFound();
        
        _reservations.Remove(reservation);
        return NoContent();
    }
    
}