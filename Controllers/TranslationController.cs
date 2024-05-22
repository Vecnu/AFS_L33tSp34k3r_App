using AFS_L33tSp34k3r_App.Interfaces;
using AFS_L33tSp34k3r_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace AFS_L33tSp34k3r_App.Controllers
{
    /// <summary>
    /// Handles requests related to text translation.
    /// </summary>
    public class TranslationController : Controller
    {
        private readonly ITranslatorService _translatorService;

        /// <summary>
        /// Initializes a new instance of the TranslationController class.
        /// </summary>
        /// <param name="translatorService">The translator service used for text translation.</param>
        public TranslationController(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }

        /// <summary>
        /// View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Translates the string taken on input to leet text using the translator service.
        /// </summary>
        /// <param name="request">The translation request containing the text to be translated.</param>
        /// <returns>The translated text as a JSON response.</returns>
        [HttpPost]
        public async Task<IActionResult> Translate([FromBody] TranslationRequester request)
        {
            var result = await _translatorService.TranslateAsync(request.TextToTranslate);
            return Json(new { result });
        }

    }
}
