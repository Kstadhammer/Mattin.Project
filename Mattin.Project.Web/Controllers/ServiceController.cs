using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Service;
using Mattin.Project.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mattin.Project.Web.Controllers;

public class ServiceController(IServiceService serviceService) : Controller
{
    // GET: Service
    public async Task<IActionResult> Index()
    {
        var result = await serviceService.GetAllAsync();
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return View(result.Value);
    }

    // GET: Service/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await serviceService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        return View(result.Value);
    }

    // GET: Service/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Service/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await serviceService.CreateAsync(dto);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.Error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Service/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await serviceService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        var dto = new UpdateServiceDto
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Description = result.Value.Description,
            BasePrice = result.Value.BasePrice,
            HourlyRate = result.Value.HourlyRate,
            Category = result.Value.Category,
            IsActive = result.Value.IsActive,
        };

        return View(dto);
    }

    // POST: Service/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateServiceDto dto)
    {
        if (id != dto.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(dto);

        var result = await serviceService.UpdateAsync(dto);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.Error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Service/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await serviceService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        return View(result.Value);
    }

    // POST: Service/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await serviceService.DeleteAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return RedirectToAction(nameof(Index));
    }

    // GET: Service/Category/{category}
    public async Task<IActionResult> Category(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return RedirectToAction(nameof(Index));

        var result = await serviceService.GetServicesByCategoryAsync(category);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        ViewBag.CurrentCategory = category;
        return View("Index", result.Value);
    }

    // GET: Service/Active
    public async Task<IActionResult> Active()
    {
        var result = await serviceService.GetActiveServicesAsync();
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        ViewBag.ShowingActive = true;
        return View("Index", result.Value);
    }
}
