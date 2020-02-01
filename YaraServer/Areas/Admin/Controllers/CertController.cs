using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using YaraServer.Data;
using YaraServer.Models;
using YaraServer.Models.ViewModels;
using YaraServer.Utils;
using YaraServer.Utils.CertificateHandler;

namespace YaraServer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CertController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICertHandler _certHandler;

        public CertController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment, ICertHandler certHandler)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _certHandler = certHandler;
        }

        public async Task<IActionResult> Index()
        {
            var certificates = await _db.Certificates.ToListAsync();
            var certificatesViewModel = new CertificatesViewModel()
            {
                Certificates = certificates
            };
            return View(certificatesViewModel);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View(new CreateCertificateViewModel());
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCertificateViewModel createCertificateViewModel)
        {
            if (ModelState.IsValid)
            {
                createCertificateViewModel.ErrorMessage = null;
                try
                {
                    if (createCertificateViewModel.Certificate.ExpiryDate <= createCertificateViewModel.Certificate.ValidDate)
                    {
                        throw new Exception("Expiry Date cannot be equal or smaller than Valid Date");
                    }

                    string webRootPath = _hostingEnvironment.WebRootPath;

                    // Get CA
                    var CACert = Path.Combine(webRootPath, "certificates", "YaraCA.pfx");
                    X509Certificate2 CA = new X509Certificate2(CACert, createCertificateViewModel.CAPassword,
                        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                    AsymmetricKeyParameter CAPrivKey = DotNetUtilities.GetKeyPair(CA.PrivateKey).Private;

                    // Generate client certificate
                    string GUID = Guid.NewGuid().ToString();
                    string pathToClientCerts = Path.Combine(webRootPath, "certificates", "clients");
                    X509Certificate2 clientCertificate = _certHandler.GenerateSelfSignedCertificate("CN=" + GUID, "CN=YaraCA", CAPrivKey,
                        createCertificateViewModel.Certificate.ValidDate, createCertificateViewModel.Certificate.ExpiryDate);
                    byte[] clientCertBytes = clientCertificate.Export(X509ContentType.Cert);
                    using (var filesStream = new FileStream(Path.Combine(pathToClientCerts, GUID + ".cer"), FileMode.Create, FileAccess.Write))
                    {
                        filesStream.Write(clientCertBytes, 0, clientCertBytes.Length);
                    }

                    CertificateDetailsModel certificateDetailsModelToDB = new CertificateDetailsModel()
                    {
                        Subject = clientCertificate.Subject,
                        Issuer = clientCertificate.Issuer,
                        Version = clientCertificate.Version.ToString(),
                        ValidDate = clientCertificate.NotBefore,
                        ExpiryDate = clientCertificate.NotAfter,
                        Thumbprint = clientCertificate.Thumbprint,
                        SerialNumber = clientCertificate.SerialNumber,
                        FriendlyName = clientCertificate.FriendlyName,
                        PublicKeyFormat = clientCertificate.PublicKey.EncodedKeyValue.Format(true),
                        RawDataLength = clientCertificate.RawData.Length.ToString()
                    };
                    _db.Certificates.Add(certificateDetailsModelToDB);
                    await _db.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    createCertificateViewModel.ErrorMessage = ex.Message;
                    return View(createCertificateViewModel);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET - DETAILS
        public async Task<IActionResult> Details(int Id)
        {
            CertificateDetailsModel certificateDetailsModel = await _db.Certificates.FirstOrDefaultAsync(m => m.Id == Id);

            return PartialView("_IndividualCertificateDetails", certificateDetailsModel);
        }
    }
}