using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Controllers
{
    public class AudioVideoController : Controller
    {
        private readonly IAudioSer _service;
        private readonly clsMainMenuList _menuService;

        public AudioVideoController(clsMainMenuList menuService, IAudioSer service)
        {
            _service = service;
            _menuService = menuService;
        }

        [HttpGet]
        [Route("backoffice/investor/addaudio")]
        public async Task<IActionResult> AddAudio(int id = 0)
        {
            ViewBag.Menus = _menuService.GetMenu();
            ViewBag.ButtonName = id > 0 ? "Update" : "Save";
            ViewBag.AudioList = await _service.GetAudioVideoList();

            AudioVideoModel model = new AudioVideoModel();

            if (id > 0)
            {
                var dt = await _service.GetAudioVideoList();
                var row = dt.AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["AudioId"]) == id);
                if (row != null)
                {
                    model.AudioId = id;
                    model.AudioTitle = row["AudioTitle"].ToString();
                    model.AudioPath = row["AudioPath"].ToString();
                    model.displayOrder = row["DisplayOrder"] as int?;
                    model.Status = Convert.ToBoolean(row["Status"]);
                }
            }

            return View("~/Views/backoffice/investor/addeditaudio.cshtml", model);
        }

        [HttpPost]
        [Route("backoffice/investor/addvideoaudio")]
        public async Task<IActionResult> AddVideoAudio(AudioVideoModel model, IFormFile file_Uploader)
        {
            try
            {
                ModelState.Remove(nameof(model.AudioPath));
                ModelState.Remove(nameof(model.trdate));
                ModelState.Remove(nameof(model.Mode));

                if (!ModelState.IsValid)
                {
                    ViewBag.AudioList = await _service.GetAudioVideoList();
                    return View("~/Views/backoffice/investor/addeditaudio.cshtml", model);
                }

                if (file_Uploader != null && file_Uploader.Length > 0)
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/AudioVideo");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string fileName =Path.GetExtension(file_Uploader.FileName);
                    string path = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    await file_Uploader.CopyToAsync(stream);
                    model.AudioPath = fileName;
                }

                model.Mode = model.AudioId > 0 ? 2 : 1;
                await _service.AddAudioVideo(model);
                TempData["SuccessMessage"] = model.AudioId > 0 ? "Audio updated successfully." : "Audio added successfully.";
                return RedirectToAction("AddAudio");
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong.";
                return RedirectToAction("AddAudio");
            }
        }

        [HttpGet]
        [Route("backoffice/investor/deleteaudio/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAudioVideo(id);
            TempData["SuccessMessage"] = "Audio deleted successfully.";
            return RedirectToAction("AddAudio");
        }

        [HttpGet]
        [Route("backoffice/investor/togglestatus/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _service.ChangeStatus(id);
            return RedirectToAction("AddAudio");
        }
    }
}
