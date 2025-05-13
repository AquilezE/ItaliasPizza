using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItaliasPizzaCliente.Utils
{
    public class Validador
    {

        public string ValidarEmail(string email)
        {
            if (email == null || string.IsNullOrWhiteSpace(email))
            {
                return "El campo email no puede estar vacío";
            }

            if (email.Length > 255)
            {
                return "El campo email no puede tener más de 255 caracteres";
            }


            if (email.Contains(' '))
            {
                return "El campo email no puede contener espacios";
            }

            if (!EsCorreoValido(email))
            {
                return "El campo email no tiene un formato válido";
            }

            return string.Empty;
        }

        public string ValidarNombreUsuario(string username)
        {
            if (username == null)
            {
             return "El campo usuario no puede estar vacío";
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                return "El campo usuario no puede estar vacío";
            }

            if (username.Length > 50)
            {
                return "El campo usuario no puede tener más de 50 caracteres";
            }

            if (username.Contains(' '))
            {
                return "El campo usuario no puede contener espacios";
            }

            if (!EsNombreUsuarioValido(username))
            {
                return "El campo usuario no tiene un formato válido";
            }

            return string.Empty;
        }
        public string ValidarContrasenia(string password)
        {
            if (password == null || string.IsNullOrWhiteSpace(password))
            {
                return "El campo contraseña no puede estar vacío";
            }

            if (password.Length < 12)
            {
                return "La contraseña debe tener al menos 12 caracteres";
            }

            if (password.Length > 255)
            {
                return "La contraseña no puede tener más de 255 caracteres";
            }

            if (password.Contains(' '))
            {
                return "La contraseña no puede contener espacios";
            }

            if (!EsContraseniaValida(password))
            {
                return "La contraseña no tiene un formato válido";
            }

            return string.Empty;
        }


        public bool EsContraseniaValida(string password)
        {
            if (password.Contains(' '))
            {
                return false;
            }

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecialChar && password.Length >= 8;
        }

        public bool EsNombreUsuarioValido(string username)
        {
            string pattern = @"^[a-zA-Z0-9_]+$";
            return Regex.IsMatch(username, pattern);
        }

        private bool EsCorreoValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                string domain = addr.Host;

                return domain.IndexOf("..") == -1 && domain.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '.');
            }
            catch
            {
                return false;
            }
        }

        public string ValidarNombreInsumo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El campo nombre del insumo no puede estar vacío";
            }

            if (nombre.Length > 100)
            {
                return "El nombre del insumo no puede tener más de 100 caracteres";
            }

            foreach (char c in nombre)
            {
                if (!char.IsLetterOrDigit(c) && c != ' ' && c != '-' && c != '\'' && c != 'á' && c != 'é' && c != 'í' && c != 'ó' && c != 'ú' && c != 'Á' && c != 'É' && c != 'Í' && c != 'Ó' && c != 'Ú')
                {
                    return "El nombre del insumo solo puede contener letras, números, espacios, guiones o apóstrofes";
                }
            }

            return string.Empty;
        }

        public string ValidarDireccion(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "Los campos no pueden estar vacío";

            texto = texto.Trim();

            if (texto.Length > 100)
                return "Confirme que los campos no tengan más de 100 caracteres";

            string patron = @"^[A-ZÁÉÍÓÚÑa-záéíóúñ0-9.,\s\-°#()]+$";

            if (!Regex.IsMatch(texto, patron))
                return "El campo contiene caracteres inválidos, solo puede contener . , -  °  # ()";

            return string.Empty;
        }

        public string ValidarNombrePersona(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "El nombre no puede estar vacío";

            nombre = nombre.Trim();

            if (nombre.Length > 50)
                return "El nombre no puede tener más de 50 caracteres";

            string patron = @"^[A-ZÁÉÍÓÚÑa-záéíóúñ]+(?:\s[A-ZÁÉÍÓÚÑa-záéíóúñ]+)*$";

            if (!Regex.IsMatch(nombre, patron))
                return "El nombre solo puede contener letras y espacios";

            return string.Empty;
        }

    }
}
