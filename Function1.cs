using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;
using System.Drawing;

namespace GrapeCityDocsAzFunc
{
    public static class GeneratePdf
    {
        /// <summary>
        /// This function generates a PDF with the text
        /// "Hi there 'name'!".
        /// To invoke: http://localhost:9999/api/GeneratePdf?name=MyName
        /// (replace URL with the one this function runs on, and 'MyName' with the name to use in PDF).
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("GeneratePdf")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function,
                      "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            // Get parameter
            string name = req.Query["name"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            name = name ?? "GrapeCity";
            if (string.IsNullOrEmpty(name))
                return new BadRequestObjectResult("Please pass a name on the query"
                        + " string or in the request body");
            else
            {
                // Create Pdf Document
                var pdf = new GcPdfDocument();
                pdf.NewPage().Graphics.DrawString($"Hi there {name}!", new TextFormat(), new PointF(72, 72));
                // Save PDF
                var ms = new MemoryStream();
                pdf.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                // Return file
                FileStreamResult result = new FileStreamResult(ms, "application/pdf");
                pdf.NewPage().
                return result;
            }
        }
    }
}