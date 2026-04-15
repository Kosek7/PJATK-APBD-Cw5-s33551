using Microsoft.AspNetCore.Mvc;
using APBD_cw5.Models;

namespace APBD_cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private static List<Room> _rooms = new()
    {
        new Room { Id = 1, Name = "A101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "A102", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "B201", BuildingCode = "B", Floor = 2, Capacity = 30, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "C301", BuildingCode = "C", Floor = 3, Capacity = 10, HasProjector = false, IsActive = false },
        new Room { Id = 5, Name = "D401", BuildingCode = "D", Floor = 4, Capacity = 50, HasProjector = true, IsActive = true }
    };
    
    [HttpGet]
    public IActionResult Get(
        [FromQuery] int? minCapacity, bool? hasProjector, bool? activeOnly)
    {
        var rooms = _rooms;
        
        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value).ToList();
        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value).ToList();
        if (activeOnly == true)
            rooms = rooms.Where(r => r.IsActive).ToList();
        
        if (rooms.Any())
            return Ok(rooms);
        
        return NotFound("Brak pokojów o podanych filtrach.");
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound($"Pokój o id: {id} nie istnieje");
        }
        
        return Ok(room);
    }

    [HttpGet("/building/{buildingCode}")]
    public IActionResult GetByBuildingCode(string buildingCode)
    {
        var rooms = _rooms.Where(r => r.BuildingCode == buildingCode).ToList();
        
        if (!rooms.Any())
        {
            return NotFound($"Żaden pokój w budynku o kodzie: {buildingCode} nie istnieje");
        }
        
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Room room)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        room.Id = _rooms.Max(r => r.Id) + 1;
        _rooms.Add(room);
        
        return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Room updatedRoom)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
        {
            return NotFound($"Pokój o id: {id} nie istnieje");
        }
        
        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
        {
            return NotFound($"Pokój o id: {id} nie istnieje");
        }
        
        _rooms.Remove(room);
        return NoContent();
    }
}