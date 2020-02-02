using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace YaraServer.Utils.CertificateHandler
{
    public class CertHandler : ICertHandler
    {
        public X509Certificate2 GenerateSelfSignedCertificate(string subjectName, string issuerName, AsymmetricKeyParameter issuerPrivKey,
            DateTime notBefore, DateTime notAfter)
        {
            const int keyStrength = 2048;

            // Generating Random Numbers
            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();
            SecureRandom random = new SecureRandom(randomGenerator);

            // The Certificate Generator
            X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();

            // Serial Number
            BigInteger serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            certificateGenerator.SetSerialNumber(serialNumber);

            // Signature Algorithm
            const string signatureAlgorithm = "SHA256WithRSA";
            certificateGenerator.SetSignatureAlgorithm(signatureAlgorithm);

            // Issuer and Subject Name
            X509Name subjectDN = new X509Name(subjectName);
            X509Name issuerDN = new X509Name(issuerName);
            certificateGenerator.SetIssuerDN(issuerDN);
            certificateGenerator.SetSubjectDN(subjectDN);

            // Valid For
            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);

            // Subject Public Key
            AsymmetricCipherKeyPair subjectKeyPair;
            var keyGenerationParameters = new KeyGenerationParameters(random, keyStrength);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            subjectKeyPair = keyPairGenerator.GenerateKeyPair();

            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            // Generating the Certificate
            Org.BouncyCastle.X509.X509Certificate certificate = certificateGenerator.Generate(issuerPrivKey, random);

            // Merge into X509Certificate2
            X509Certificate2 x509 = new X509Certificate2(certificate.GetEncoded());

            //Asn1Sequence seq = (Asn1Sequence)info.ParsePrivateKey();
            //if (seq.Count != 9)
            //{
            //    throw new PemException("malformed sequence in RSA private key");
            //}

            //RsaPrivateKeyStructure rsa = new RsaPrivateKeyStructure(seq);
            //RsaPrivateCrtKeyParameters rsaparams = new RsaPrivateCrtKeyParameters(
            //    rsa.Modulus, rsa.PublicExponent, rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1, rsa.Exponent2, rsa.Coefficient);

            //x509.PrivateKey = ToDotNetKey(rsaparams); //x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);
            return x509;
        }

        public void RevokeCertificate(X509Certificate2 x509Certificate2, X509Certificate2 authority, string pathToCrls)
        {
            var certCA = DotNetUtilities.FromX509Certificate(authority);

            X509V2CrlGenerator crlGen = new X509V2CrlGenerator();
            crlGen.SetIssuerDN(certCA.IssuerDN);
            crlGen.SetThisUpdate(DateTime.Now);
            crlGen.SetNextUpdate(DateTime.Now.AddYears(1));
            crlGen.SetSignatureAlgorithm("SHA1withRSA");

            crlGen.AddCrlEntry(new BigInteger(x509Certificate2.SerialNumber, 16), DateTime.Now, CrlReason.PrivilegeWithdrawn);

            crlGen.AddExtension(X509Extensions.AuthorityKeyIdentifier,
                               false,
                               new AuthorityKeyIdentifierStructure(certCA));

            crlGen.AddExtension(X509Extensions.CrlNumber,
                               false,
                               new CrlNumber(BigInteger.One));

            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);

            var Akp = DotNetUtilities.GetKeyPair(authority.PrivateKey).Private;


            X509Crl crlTemp = crlGen.Generate(Akp, random);
            byte[] crl = crlTemp.GetEncoded();
            using (var filesStream = new FileStream(Path.Combine(pathToCrls, x509Certificate2.Subject + ".crl"), FileMode.Create, FileAccess.Write))
            {
                filesStream.Write(crl, 0, crl.Length);
            }
        }

        public ValidityConstants VerifyCertificate(X509Certificate2 client, X509Certificate2 authority, string crlPath)
        {
            X509Chain chain = new X509Chain();

            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

            chain.ChainPolicy.ExtraStore.Add(authority);

            // Do the preliminary validation.
            if (!chain.Build(client))
            {
                Dictionary<string, string> statuses = new Dictionary<string, string>();
                foreach (var item in chain.ChainStatus)
                {
                    if (item.Status == X509ChainStatusFlags.NotTimeValid)
                    {
                        return ValidityConstants.NOT_VALID_YET_OR_EXPIRED;
                    }
                    if (item.Status == X509ChainStatusFlags.NotSignatureValid)
                    {
                        return ValidityConstants.NOT_SIGNED_CA;
                    }
                }
            }

            // Do CRL Validation
            bool revoked = false;
            foreach (string file in Directory.EnumerateFiles(crlPath))
            {
                try
                {
                    byte[] buf = File.ReadAllBytes(file);
                    X509CrlParser xx = new X509CrlParser();
                    X509Crl ss = xx.ReadCrl(buf);
                    var nextupdate = ss.NextUpdate;

                    var isRevoked = ss.GetRevokedCertificate(new BigInteger(client.SerialNumber, 16));
                    if (isRevoked != null)
                    {
                        revoked = true;
                    }
                    Console.WriteLine("{0} {1}", nextupdate, isRevoked);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (revoked)
            {
                return ValidityConstants.IS_REVOKED;
            }

            // This piece makes sure it actually matches your known root
            var valid = chain.ChainElements
                .Cast<X509ChainElement>()
                .Any(x => x.Certificate.Thumbprint == authority.Thumbprint);

            if (!valid)
                return ValidityConstants.NOT_THUMBPRINT_CA;

            return ValidityConstants.OK;
        }

        public byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];

            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
