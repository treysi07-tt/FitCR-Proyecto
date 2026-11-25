using System;
using System.Security.Cryptography;

namespace FitCR.Services
{
    public class Contrasenas
    {
        private const int Iteraciones = 600000;
        private const int TamanoSal = 16;
        private const int TamanoHash = 32;

        // Convierte una contraseña en un hash
        public string GenerarHash(string contrasena)
        {
            byte[] sal = RandomNumberGenerator.GetBytes(TamanoSal);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                contrasena,
                sal,
                Iteraciones,
                HashAlgorithmName.SHA256,
                TamanoHash
            );

            string salTexto = Convert.ToBase64String(sal);
            string hashTexto = Convert.ToBase64String(hash);
            return salTexto + ":" + hashTexto;
        }
        public bool VerificarContrasena(
            string contrasena,
            string contrasenaGuardada)
        {

            string[] partes = contrasenaGuardada.Split(':');

            if (partes.Length != 2)
            {
                return false;
            }

            try
            {
                byte[] sal = Convert.FromBase64String(partes[0]);
                byte[] hashGuardado = Convert.FromBase64String(partes[1]);

                if (sal.Length != TamanoSal ||
                    hashGuardado.Length != TamanoHash)
                {
                    return false;
                }

                byte[] hashIngresado = Rfc2898DeriveBytes.Pbkdf2(
                    contrasena,
                    sal,
                    Iteraciones,
                    HashAlgorithmName.SHA256,
                    TamanoHash
                );

                return CryptographicOperations.FixedTimeEquals(
                    hashIngresado,
                    hashGuardado
                );
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

