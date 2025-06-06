﻿using dev_framework.Message.Model;
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
        private SynchronizedConverter _converter;
        private readonly SerilogManager _serilogManager;

        public FileManager(SynchronizedConverter converter, SerilogManager serilogManager)
        {
            _converter = converter;
            _serilogManager = serilogManager;
        }

        public IOMessage GeneratePDF(string url, string filePath, string footer)
        {
            var responseData = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                using (var response = request.GetResponse())
                {
                    using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                    {
                        try
                        {
                            responseData = responseReader.ReadToEnd();
                            var doc = new HtmlToPdfDocument()
                            {
                                GlobalSettings = {
                                    ColorMode = DinkToPdf.ColorMode.Color,
                                    Orientation = DinkToPdf.Orientation.Portrait,
                                    PaperSize = DinkToPdf.PaperKind.A4,
                                    Margins = new MarginSettings() { Top = 10 },
                                    Out = filePath,
                                    Collate = false,
                                    PageOffset = 0,
                                },
                                Objects = { new ObjectSettings() {
                                HtmlContent = responseData,
                                WebSettings = { DefaultEncoding = "utf-8", LoadImages=true },
                                FooterSettings = new FooterSettings
                                {
                                    Center="[page]/[topage]",
                                    FontSize = 8,
                                    Right = footer
                                }
                            } }
                            };

                            _converter.Convert(doc);
                            return new IOMessage(EIOMessage.Success) { ReturnValue = new { FilePath = filePath } };
                        }
                        catch (Exception ex)
                        {
                            _serilogManager.Error("Erreur durant la génération du pdf", ex);
                            return new IOMessage(EIOMessage.Error) { Exception = ex };
                        }
                    }
                }
            }
            catch (Exception ex) { return new IOMessage(EIOMessage.Error) { Exception = ex }; }
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