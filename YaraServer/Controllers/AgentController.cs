using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YaraServer.Data;
using YaraServer.Models;
using YaraServer.Utils.CertificateHandler;

namespace YaraServer.Controllers
{
    public class AgentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ICertHandler _certHandler;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AgentController(ApplicationDbContext db, ICertHandler certHandler, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _certHandler = certHandler;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] TerminalDetailsModel terminalDetailsModel)
        {
            // Get client certificate
            string clientCertFromHeader = Request.Headers["X-ARR-ClientCert"];
            if (clientCertFromHeader == null)
            {
                return BadRequest();
            }
            byte[] bytes = _certHandler.StringToByteArray(clientCertFromHeader);
            X509Certificate2 clientCertificate = new X509Certificate2(bytes);

            // Get CA
            string webRootPath = _hostingEnvironment.WebRootPath;
            var CACert = Path.Combine(webRootPath, "certificates", "YaraCA.pfx");
            X509Certificate2 CA = new X509Certificate2(CACert, "12345678",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            // Validate Certificate
            string crlsPath = Path.Combine(webRootPath, "certificates", "crls");
            var validity = _certHandler.VerifyCertificate(clientCertificate, CA, crlsPath);
            if (validity != Utils.ValidityConstants.OK)
            {
                return Unauthorized();
            }

            // Save Details to DB
            if (ModelState.IsValid)
            {
                // Check if client already enrolled
                var client = await _db.Terminals.Include(s => s.Certificate).SingleOrDefaultAsync(m => m.Certificate.Subject == clientCertificate.Subject);
                if (client == null)
                {
                    // Enroll client
                    var certificate = await _db.Certificates.SingleOrDefaultAsync(m => m.Subject == clientCertificate.Subject);
                    if (certificate == null)
                    {
                        return StatusCode(500);
                    }
                    TerminalDetailsModel clientToDb = new TerminalDetailsModel()
                    {
                        SystemName = terminalDetailsModel.SystemName,
                        OsName = terminalDetailsModel.OsName,
                        Version = terminalDetailsModel.Version,
                        MAC = terminalDetailsModel.MAC,
                        Processor = terminalDetailsModel.Processor,
                        Motherboard = terminalDetailsModel.Motherboard,
                        RAM = terminalDetailsModel.RAM,
                        CertificateId = certificate.Id
                    };
                    _db.Terminals.Add(clientToDb);
                    await _db.SaveChangesAsync();
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Report([FromBody] TerminalDetailsModel terminalDetailsModel)
        {
            // Get client certificate
            string clientCertFromHeader = Request.Headers["X-ARR-ClientCert"];
            if (clientCertFromHeader == null)
            {
                return BadRequest();
            }
            byte[] bytes = _certHandler.StringToByteArray(clientCertFromHeader);
            X509Certificate2 clientCertificate = new X509Certificate2(bytes);

            // Get CA
            string webRootPath = _hostingEnvironment.WebRootPath;
            var CACert = Path.Combine(webRootPath, "certificates", "YaraCA.pfx");
            X509Certificate2 CA = new X509Certificate2(CACert, "12345678",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            // Validate Certificate
            string crlsPath = Path.Combine(webRootPath, "certificates", "crls");
            var validity = _certHandler.VerifyCertificate(clientCertificate, CA, crlsPath);
            if (validity != Utils.ValidityConstants.OK)
            {
                return Unauthorized();
            }

            // Save Report to DB
            if (ModelState.IsValid)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}