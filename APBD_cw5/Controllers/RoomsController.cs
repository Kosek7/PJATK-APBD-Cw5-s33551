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
}