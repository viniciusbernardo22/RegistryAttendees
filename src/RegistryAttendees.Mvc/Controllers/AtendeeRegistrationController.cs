using Microsoft.AspNetCore.Mvc;
using RegistryAttendees.Mvc.Entities;
using RegistryAttendees.Mvc.Interfaces;


namespace RegistryAttendees.Mvc.Controllers;

public class AtendeeRegistrationController : Controller
{
    private readonly ITableStorageService<Attendee> _service;
    private readonly ILogger<AtendeeRegistrationController> _logger;
    
    public AtendeeRegistrationController(ILogger<AtendeeRegistrationController> logger, ITableStorageService<Attendee> service)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<ActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        
        _logger.LogWarning("Executed _service.GetAllAsync()");
        
        return View(data);
    }

    public async Task<ActionResult> Details(string industry, string id)
    {
        var data = await _service.GetByIdAsync(industry, id);
        _logger.LogWarning("Executed _service.GetByIdAsync(industry, id)");
        
        return View(data);
    }

    public async Task<ActionResult> Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Attendee attendee)
    {
        try
        {
            attendee.PartitionKey = attendee.Industry;
            attendee.RowKey = Guid.NewGuid().ToString();

            await _service.UpsertAsync(attendee);

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    
    public async Task<ActionResult> EditAsync(string industry, string id)
    {
        var data = await _service.GetByIdAsync(industry, id);

        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, Attendee attendee)
    {
        try
        {
            attendee.PartitionKey = attendee.Industry;
            
            await _service.UpsertAsync(attendee);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(string industry, string id)
    {
        try
        {
            await _service.DeleteAsync(industry, id);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }
}