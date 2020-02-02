using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace YaraServer.Utils.CertificateHandler
{
    public interface ICertHandler
    {
        X509Certificate2 GenerateSelfSignedCertificate(string subjectName, string issuerName, AsymmetricKeyParameter issuerPrivKey,
            DateTime notBefore, DateTime notAfter);
        ValidityConstants VerifyCertificate(X509Certificate2 client, X509Certificate2 authority, string crlPath);
        void RevokeCertificate(X509Certificate2 x509Certificate2, X509Certificate2 authority, string pathToCrls);
        byte[] StringToByteArray(string hex);
    }
}
