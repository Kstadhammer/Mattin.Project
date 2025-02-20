using Mattin.Project.Core.Interfaces;
using Mattin.Project.Core.Models.DTOs.Project;
using Mattin.Project.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mattin.Project.Web.Controllers;

public class ProjectController(
    IProjectService projectService,
    IClientService clientService,
    IProjectManagerService projectManagerService,
    IServiceService serviceService
) : Controller
{
    // GET: Project
    public async Task<IActionResult> Index()
    {
        var result = await projectService.GetAllAsync();
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return View(result.Value);
    }

    // GET: Project/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await projectService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        return View(result.Value);
    }

    // GET: Project/Create
    public async Task<IActionResult> Create()
    {
        // Get data for dropdowns
        var clientsResult = await clientService.GetAllAsync();
        var managersResult = await projectManagerService.GetAllAsync();
        var servicesResult = await serviceService.GetActiveServicesAsync();

        if (clientsResult.IsFailure || managersResult.IsFailure || servicesResult.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = "Failed to load required data" });

        ViewBag.Clients = clientsResult.Value;
        ViewBag.ProjectManagers = managersResult.Value;
        ViewBag.Services = servicesResult.Value;
        ViewBag.Statuses = new[]
        {
            Core.Models.Entities.ProjectStatus.NotStarted,
            Core.Models.Entities.ProjectStatus.InProgress,
            Core.Models.Entities.ProjectStatus.Completed,
        };

        return View();
    }

    // POST: Project/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await projectService.CreateAsync(dto);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.Error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Project/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var projectResult = await projectService.GetByIdAsync(id);
        if (projectResult.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = projectResult.Error });

        if (projectResult.Value == null)
            return NotFound();

        // Get data for dropdowns
        var clientsResult = await clientService.GetAllAsync();
        var managersResult = await projectManagerService.GetAllAsync();
        var servicesResult = await serviceService.GetActiveServicesAsync();

        if (clientsResult.IsFailure || managersResult.IsFailure || servicesResult.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = "Failed to load required data" });

        ViewBag.Clients = clientsResult.Value;
        ViewBag.ProjectManagers = managersResult.Value;
        ViewBag.Services = servicesResult.Value;
        ViewBag.Statuses = new[]
        {
            Core.Models.Entities.ProjectStatus.NotStarted,
            Core.Models.Entities.ProjectStatus.InProgress,
            Core.Models.Entities.ProjectStatus.Completed,
        };

        // Create UpdateProjectDto from ProjectDetailsDto
        var dto = new UpdateProjectDto
        {
            Id = projectResult.Value.Id,
            Title = projectResult.Value.Title,
            Description = projectResult.Value.Description,
            Status = projectResult.Value.Status,
            StartDate = projectResult.Value.StartDate,
            EndDate = projectResult.Value.EndDate,
            ProjectManagerId = projectResult.Value.ProjectManagerId,
            HourlyRate = projectResult.Value.HourlyRate,
            TotalPrice = projectResult.Value.TotalPrice,
            ClientId = projectResult.Value.ClientId,
        };

        return View(dto);
    }

    // POST: Project/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateProjectDto dto)
    {
        if (id != dto.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(dto);

        var result = await projectService.UpdateAsync(dto);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.Error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Project/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await projectService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        return View(result.Value);
    }

    // POST: Project/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await projectService.DeleteAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return RedirectToAction(nameof(Index));
    }
}
