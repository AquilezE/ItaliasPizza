using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ItaliasPizzaCliente.Utils
{
    public class RolAVisibilidadConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;


            string userRole = value.ToString();
            string[] allowedRoles = parameter.ToString().Split(',');

            Console.WriteLine(userRole);
            foreach (var role in allowedRoles)
            {
                Console.WriteLine("-"+role);
            }

            return allowedRoles.Contains(userRole, StringComparer.OrdinalIgnoreCase)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
