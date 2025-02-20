using Mattin.Project.Core.Interfaces;
using Mattin.Project.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mattin.Project.Web.Controllers;

public class ProjectManagerController(IProjectManagerService projectManagerService) : Controller
{
    // GET: ProjectManager
    public async Task<IActionResult> Index()
    {
        var result = await projectManagerService.GetAllAsync();
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        return View(result.Value);
    }

    // GET: ProjectManager/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await projectManagerService.GetByIdAsync(id);
        if (result.IsFailure)
            return View("Error", new ErrorViewModel { RequestId = result.Error });

        if (result.Value == null)
            return NotFound();

        return View(result.Value);
    }
}
