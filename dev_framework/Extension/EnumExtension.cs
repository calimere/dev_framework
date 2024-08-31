using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class EnumExtension
    {
        /// <summary>
        /// Permet si la valeur de l'enum possède un attribut "Description", 
        /// de retourner le contenu du champs description de l'attribut.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum o)
        {
            var attrsDescription = o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).Select(p => (DescriptionAttribute)p);
            if (attrsDescription.Any())
                return attrsDescription.First().Description;

            return string.Empty;
        }

        /// <summary>
        /// Permet de récupérer la valeur de l'attribut Name
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetName(this Enum o)
        {
            var attrsDescription = o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(NameAttribute), false).Select(p => (NameAttribute)p);
            if (attrsDescription.Any())
                return attrsDescription.First().Name;

            return string.Empty;
        }

        /// <summary>
        /// Permet de récupérer la valeur de l'attribut DefaultValue
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Enum o)
        {
            var attrsDescription = o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(DefaultValueAttribute), false).Select(p => (DefaultValueAttribute)p);
            if (attrsDescription.Any())
                return attrsDescription.First().Value;

            return null;
        }

        public static string GetCategorie(this Enum o)
        {
            var attrsDescription = o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(CategoryAttribute), false).Select(p => (CategoryAttribute)p);
            if (attrsDescription.Any())
                return attrsDescription.First().Category;

            return string.Empty;
        }

        public static string GetDataType(this Enum o)
        {
            var attrsDescription = o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(DataTypeAttribute), false).Select(p => (DataTypeAttribute)p);
            if (attrsDescription.Any())
            {
                if (attrsDescription.First().DataType == DataType.Custom)
                    return attrsDescription.First().CustomDataType;
                else
                    return attrsDescription.First().DataType.ToString();
            }
            return string.Empty;
        }

        public static List<T> ToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T doit être du type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }

        public static T? ParseValue<T>(string name) where T : struct, IConvertible
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T doit être du type System.Enum");

            T retour;
            if (Enum.TryParse<T>(name, out retour))
                return retour;
            else
                return null;
        }

        public static string ToDescription(this Enum e) { return GetEnumDescription((Enum)(object)((Enum)e)); }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;

            return value.ToString();
        }

        public static List<SelectListItem> ToSelectListItems<T>(string value)
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(m => new SelectListItem
            {
                Text = m.ToDescription(),
                Value = m.ToString(),
                Selected = m.ToString() == value
            }).ToList();
        }
        public static List<SelectListItem> ToSelectListItemsByDefaultValue<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(m => new SelectListItem
            {
                Text = m.ToDescription(),
                Value = m.GetDefaultValue().ToString()
            }).ToList();
        }
        public static IEnumerable<SelectListItem> ToIEnumerableSelectListItems<T>(string value)
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(m => new SelectListItem
            {
                Text = m.ToDescription(),
                Value = m.ToString(),
                Selected = m.ToString() == value
            });
        }
        public static IEnumerable<SelectListItem> ToIEnumerableSelectListItems<T>(string value, string[] excluded)
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(m => new SelectListItem
            {
                Text = m.ToDescription(),
                Value = m.ToString(),
                Selected = m.ToString() == value,
                Disabled = excluded.Contains(m.ToString())
            });
        }

        public static IEnumerable<SelectListItem> ToIEnumerableSelectListItemsByDefaultValue<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(m => new SelectListItem
            {
                Text = m.ToDescription(),
                Value = m.GetDefaultValue().ToString()
            });
        }

        public static T GetByValue<T>(string value) where T : Enum
        {
            return (T)Enum.GetValues(typeof(T)).Cast<Enum>().FirstOrDefault(m => m.ToString() == value);
        }
        public static T Get<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }

    public static class Enum<T> where T : struct, IConvertible
    {
        public static List<T> ToList()
        {
            return EnumExtension.ToList<T>();
        }

        public static T? ParseValue(string value)
        {
            return EnumExtension.ParseValue<T>(value);
        }
    }

    public class NameAttribute : Attribute
    {
        private string _name;
        public NameAttribute(string name)
        {
            _name = name;
        }

        public virtual string Name
        {
            get { return _name; }
        }

        public override string ToString()
        {
            return this._name;
        }

    }
}