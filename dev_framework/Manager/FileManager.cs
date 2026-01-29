using dev_framework.Message.Model;
using DinkToPdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Manager
{
    public class FileManager
    {
        private readonly SerilogManager _serilogManager;

        public FileManager(SerilogManager serilogManager)
        {
            _serilogManager = serilogManager;
        }

        public async Task<IOMessage> GeneratePDFFromPython(string pythonServer, string url, string filePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = new { url = url, filePath = filePath };
                    var content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(pythonServer, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Lire le contenu PDF (stream)
                        byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();

                        // Enregistrer dans un fichier
                        await System.IO.File.WriteAllBytesAsync("output.pdf", pdfBytes);

                        Console.WriteLine("PDF téléchargé et sauvegardé en output.pdf");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Erreur : {response.StatusCode}");
                        Console.WriteLine(error);
                        return new IOMessage(EIOMessage.Error) { ReturnValue = new { FilePath = filePath }, Exception = new Exception($"Erreur : {response.StatusCode} - {error}") };
                    }
                }

                return new IOMessage(EIOMessage.Success) { ReturnValue = new { FilePath = filePath } };
            }
            catch (HttpRequestException ex)
            {
                _serilogManager.Error("Erreur de requête HTTP lors de la génération du PDF", ex);
                return new IOMessage(EIOMessage.Error) { Exception = ex };
            }
            catch (Exception ex)
            {
                _serilogManager.Error("Erreur générale lors de la génération du PDF", ex);
                return new IOMessage(EIOMessage.Error) { Exception = ex };
            }
        }
    }
}