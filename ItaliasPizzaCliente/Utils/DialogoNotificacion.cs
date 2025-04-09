using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;


namespace ItaliasPizzaCliente.Utils
{
    public class DialogoNotificacion
    {
        private Notifier _notifier;

        public DialogoNotificacion()
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    Application.Current.MainWindow,

                    Corner.TopRight,
                    10,
                    10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    TimeSpan.FromSeconds(3),
                    MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public void ShowInfoNotification(string message)
        {
            _notifier.ShowInformation(message);
        }

        public void ShowSuccessNotification(string message)
        {
            _notifier.ShowSuccess(message);
        }

        public void ShowWarningNotification(string message)
        {
            _notifier.ShowWarning(message);
        }

        public void ShowErrorNotification(string message)
        {
            _notifier.ShowError(message);
        }

        public void Dispose()
        {
            _notifier.Dispose();
        }
    }
}
