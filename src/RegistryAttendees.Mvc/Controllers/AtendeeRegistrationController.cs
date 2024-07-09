using Microsoft.AspNetCore.Mvc;
using RegistryAttendees.Mvc.Entities;
using RegistryAttendees.Mvc.Interfaces;


namespace RegistryAttendees.Mvc.Controllers;

public class AtendeeRegistrationController : Controller
{
    private readonly ITableStorageService<Attendee> _service;
    
    public AtendeeRegistrationController(ILogger<AtendeeRegistrationController> logger, ITableStorageService<Attendee> service)
    {
        _service = service;
    }

    public async Task<ActionResult> Index()
    {
        var data = await _service.GetAllAsync();

        return View(data);
    }

    public async Task<ActionResult> DetailsAsync(string industry, string id)
    {
        var data = await _service.GetByIdAsync(industry, id);

        return View(data);
    }
    
}