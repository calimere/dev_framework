using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dev_framework.Security
{
    public class PasswordManager : Singleton<PasswordManager>
    {
        public FormModel ValidatePassword(PasswordCondition[] passwordConditions, string value)
        {
            var retour = new FormModel();
            var message = string.Empty;

            foreach (var item in passwordConditions)
            {
                switch (item)
                {
                    case PasswordCondition.Blank:
                        message = IsBlankPassword(value);
                        break;
                    case PasswordCondition.NoMajuscule:
                        message = IsPasswordMaj(value);
                        break;
                    case PasswordCondition.NoNumeric:
                        message = IsPasswordNumeric(value);
                        break;
                    case PasswordCondition.NoSpecial:
                        message = IsPasswordSpecial(value);
                        break;
                    case PasswordCondition.TooSmall:
                        message = IsPasswordLong(value);
                        break;
                }
                if (!string.IsNullOrEmpty(message)) retour.Errors.Add(message);
            }

            if (retour.Errors.Any())
            {
                retour.ResultStatut = ResultStatut.error;
                retour.FormModelStatus = retour.ResultStatut.ToString();
            }
            return retour;
        }
        public string IsBlankPassword(string value)
        {
            return value.Length > 0
                ? string.Empty
                : "Le mot de passe ne peut pas être vide.";
        }
        public string IsPasswordLong(string value, int minLength = 6)
        {
            return value.Length >= minLength
                ? string.Empty
                : string.Format("Le mot de passe doit faire au moins {0}", minLength);
        }
        public string IsPasswordNumeric(string value)
        {
            return Regex.IsMatch(value, @"\d")
                ? string.Empty
                : "Le mot de passe ne contient pas de chiffre.";
        }
        public string IsPasswordMaj(string value)
        {
            return Regex.IsMatch(value, @"[A-Z]")
                ? string.Empty
                : "Le mot de passe ne contient pas de majuscule.";
        }
        public string IsPasswordSpecial(string value, string regex = ".[<,>,!,@,#,$,%,^,&,*,?,_, ~,-,£,(,)]")
        {
            return Regex.IsMatch(value, @"[<,>,!,@,#,$,%,^,&,*,?,_, ~,-,£,(,)]")
                ? string.Empty
                : "Le mot de passe ne contient pas de caractère spéciaux.";
        }
        public PasswordCondition[] GetPasswordConditions()
        {
            return Enum<PasswordCondition>.ToList().ToArray();
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public string RandomPassword(int size = 10)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));

            if (RandomNumber(0, 3) % 2 == 0)
                builder.Append("#");
            else
                builder.Append("!");

            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string GetSpecialCharacter()
        {
            var _random = new Random();
            int num = _random.Next(5); // Zero to 25
            switch (num)
            {
                case 0:
                    return "!";
                case 1:
                    return "@";
                case 2:
                    return "#";
                case 3:
                    return "$";
                default:
                    return "%";
            }
        }
        private string GetMinLetter()
        {
            var _random = new Random();
            int num = _random.Next(0, 26); // Zero to 25
            char let = (char)('a' + num);
            return let.ToString();
        }
        private string GetMajLetter()
        {
            var _random = new Random();
            int num = _random.Next(0, 26); // Zero to 25
            char let = (char)('A' + num);
            return let.ToString();
        }
        private string GetRandomNumber()
        {
            var _random = new Random();
            int num = _random.Next(0, 10); // Zero to 25
            char let = (char)('0' + num);
            return let.ToString();
        }
        public string GenerateRandomPassword(int length, bool withSharp = true)
        {
            var rand = new Random();
            var str = new StringBuilder();
            str.Append(GetMinLetter());
            str.Append(GetRandomNumber());

            for (var i = 0; i < length - 3; i++)
            {
                var n = withSharp ? rand.Next(8) : rand.Next(6);
                switch (n)
                {
                    case 0:
                    case 1:
                    default:
                        str.Append(GetMinLetter());
                        break;
                    case 2:
                    case 3:
                        str.Append(GetMajLetter());
                        break;
                    case 4:
                    case 5:
                        str.Append(GetRandomNumber());
                        break;
                    case 6:
                        str.Append("#");
                        break;
                    case 7:
                        str.Append("!");
                        break;
                }
            }
            str.Append(GetMajLetter());
            return str.ToString();
        }
    }

    public enum PasswordCondition
    {
        Blank,
        TooSmall,
        NoNumeric,
        NoMajuscule,
        NoSpecial
    }
}
