using DialogTest.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DialogTest.Controllers
{
    [Route("api/[controller]")]
    public class DialogController : Controller
    {
        private readonly List<RGDialogsClients> _dialogClients = new RGDialogsClients().Init();
        private readonly ILogger<DialogController>? _logger;
        public DialogController(ILogger<DialogController>? log)
        {
            _logger = log;
        }


        [HttpPost]
        [Route("get/dialog")]
        public IActionResult GetDialoGUID([FromBody] List<Guid> users)
        {
            List<List<Guid>> dialogs = new List<List<Guid>>();

            foreach (var user in users)
            {
                dialogs.Add(_dialogClients.FindAll(el => el.IDClient == user)
                    .Select(el => el.IDRGDialog)
                    .ToList());
            }

            List<Guid>? Results = dialogs.FirstOrDefault();

            foreach (var dialog in dialogs)
            {
                Results = Results.Intersect(dialog).ToList();
            }

            if (Results == null || Results.Count == 0)
            {
                return Ok(Guid.Empty);
            }

            return Ok(Results); //возвращаются все диалоги, но можно вернуть любой удобный
        }
    }
}
