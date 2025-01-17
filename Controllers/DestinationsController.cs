using Microsoft.AspNetCore.Mvc;
using TravelClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Diagnostics;

namespace TravelClient.Controllers;

public class DestinationsController : Controller
{
  // public async Task<IActionResult> Index(int page = 1)
  // {
  //     var httpClient = new HttpClient();
  //     var response = await httpClient.GetAsync("https://localhost:5001/api/Destinations");
  //     var destinations = await response.Content.ReadAsAsync<List<Destination>>();

  //     var pageSize = 10; // Set the number of items to display per page
  //     var count = destinations.Count;
  //     var data = destinations.Skip((page - 1) * pageSize).Take(pageSize).ToList();

  //     var viewModel = new DestinationViewModel
  //     {
  //         Destinations = data,
  //         PageNumber = page,
  //         PageSize = pageSize,
  //         TotalItems = count,
  //         PageCount = (int)Math.Ceiling((double)count / pageSize)
  //     };

  //     return View(viewModel);
  // }

  public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
  {
    List<Destination> destList = new List<Destination> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:5001/api/Destinations?page={pageNumber}"))
      {
        var destContent = await response.Content.ReadAsStringAsync();
        JArray destArray = JArray.Parse(destContent);
        destList = destArray.ToObject<List<Destination>>();
      }
    }
    // ViewBag.FinalDestination = destList.Count();
    ViewBag.PageNumber = pageNumber;
    ViewBag.PageSize = pageSize;

    return View(destList);
  }

  public IActionResult Details(int id)
  {
    Destination destination = Destination.GetDetails(id);
    return View(destination);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Destination destination)
  {
    Destination.Post(destination);
    return RedirectToAction("Index");
  }

  public ActionResult Edit(int id)
  {
    Destination destination = Destination.GetDetails(id);
    return View(destination);
  }

  [HttpPost]
  public ActionResult Edit(Destination destination)
  {
    Destination.Put(destination);
    return RedirectToAction("Details", new { id = destination.DestinationId });
  }

  public ActionResult Delete(int id)
  {
    Destination destination = Destination.GetDetails(id);
    return View(destination);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Destination.Delete(id);
    return RedirectToAction("Index");
  }
}