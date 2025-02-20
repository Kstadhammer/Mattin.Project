using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Client;
using Mattin.Project.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mattin.Project.Web.Controllers;

public class ClientController(IClientService clientService) : Controller
{
    // GET: Client
    public async Task<IActionResult> Index()
    {
        var result = await clientService.GetAllAsync();
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return View(result.Value);
    }

    // GET: Client/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await clientService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        // Get client's projects
        var projectsResult = await clientService.GetClientProjectsAsync(id);
        if (projectsResult.IsSuccess)
        {
            ViewBag.Projects = projectsResult.Value;
        }

        return View(result.Value);
    }

    // GET: Client/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Client/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateClientDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await clientService.CreateAsync(dto);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.Error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Client/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await clientService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        var dto = new UpdateClientDto
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Email = result.Value.Email,
            PhoneNumber = result.Value.PhoneNumber,
            Address = result.Value.Address,
        };

        return View(dto);
    }

    // POST: Client/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateClientDto dto)
    {
        if (id != dto.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(dto);

        var result = await clientService.UpdateAsync(dto);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.Error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Client/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await clientService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        // Check if client has any projects
        var projectsResult = await clientService.GetClientProjectsAsync(id);
        if (projectsResult.IsSuccess && projectsResult.Value.Any())
        {
            ViewBag.HasProjects = true;
            ViewBag.ProjectCount = projectsResult.Value.Count();
        }

        return View(result.Value);
    }

    // POST: Client/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await clientService.DeleteAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return RedirectToAction(nameof(Index));
    }

    // GET: Client/Search
    public async Task<IActionResult> Search(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RedirectToAction(nameof(Index));

        var result = await clientService.GetClientsByNameAsync(name);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        ViewBag.SearchTerm = name;
        return View("Index", result.Value);
    }
}
