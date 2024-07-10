using Microsoft.AspNetCore.Mvc;
using RegistryAttendees.Mvc.Entities;
using RegistryAttendees.Mvc.Interfaces;


namespace RegistryAttendees.Mvc.Controllers;

public class AtendeeRegistrationController : Controller
{
    private readonly ITableStorageService<Attendee> _tableStorageService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<AtendeeRegistrationController> _logger;
    
    public AtendeeRegistrationController(ILogger<AtendeeRegistrationController> logger, ITableStorageService<Attendee> service, IBlobStorageService blobStorageService)
    {
        _tableStorageService = service;
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<ActionResult> Index()
    {
        
        List<Attendee> atendees = await _tableStorageService.GetAllAsync();

        foreach (Attendee attendee in atendees)
        {
            attendee.ImageName = await _blobStorageService.GetBlobUrl(attendee.ImageName);
        }
        _logger.LogWarning("Executed _service.GetAllAsync()");
        
        return View(atendees);
    }

    public async Task<ActionResult> Details(string industry, string id)
    {
        
        var data = await _tableStorageService.GetByIdAsync(industry, id);
        _logger.LogWarning("Executed _service.GetByIdAsync(industry, id)");
        
        return View(data);
    }

    public async Task<ActionResult> Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Attendee attendee, IFormFile formFile)
    {
        try
        {
            string id = Guid.NewGuid().ToString();
            attendee.PartitionKey = attendee.Industry;
            attendee.RowKey = id;

            if (formFile.Length > 0)
            {
                attendee.ImageName = await _blobStorageService.UploadBlob(formFile, id);
            }
            else
            {
                attendee.ImageName = "default.jpg";
            }
            

            await _tableStorageService.UpsertAsync(attendee);

            _logger.LogWarning("Executed _service.UpsertAsync(attendee)");
            
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
        
    }
    
    public async Task<ActionResult> Edit(string industry, string id)
    {
        var data = await _tableStorageService.GetByIdAsync(industry, id);

        _logger.LogWarning("Executed _service.GetByIdAsync(industry, id)");
        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, Attendee attendee, IFormFile formFile)
    {
        try
        {
            if (formFile?.Length > 0)
            {
                attendee.ImageName = formFile.Name;
                await _blobStorageService.UploadBlob(formFile, attendee.RowKey);
            }
            attendee.PartitionKey = attendee.Industry;
            await _tableStorageService.UpsertAsync(attendee);

          
            _logger.LogWarning("Executed _service.UpsertAsync(attendee)");
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
            var attendee = await _tableStorageService.GetByIdAsync(industry, id);
            
            await _tableStorageService.DeleteAsync(industry, id);

            await _blobStorageService.RemoveBlob(attendee.ImageName);
            
            _logger.LogWarning("Executed _service.DeleteAsync(industry, id)");
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            return View();
        }
    }
}