using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DeepLearnig;
using DeepLearning.Models;

namespace DeepLearning.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(IFormFile file)
    {
        if (file != null)
        {
            var Imagepath = await UploadedFile(file);

            var input = new MLModel.ModelInput
            {
                ImageSource = Imagepath
            };
            MLModel.ModelOutput result = MLModel.Predict(input);
            ViewBag.data = "Selected type is " + result.Prediction;
            System.IO.File.Delete(Imagepath);
        }
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<string?> UploadedFile(IFormFile image)
    {
        try
        {
            if (image != null)
            {
                string uploadsFolder = "wwwroot/";
                string uniqueFileName = image.FileName.ToLower();
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return Path.Combine("wwwroot/", uniqueFileName);
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
