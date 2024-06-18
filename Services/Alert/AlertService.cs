using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace MultipleAreas_BlazorTemplate.Services.Alert
{
    public class AlertService
    {
        private readonly NotificationService _notificationService;

        public AlertService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void ShowSimpleAlert(string message, string summary = "Info")
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Summary = summary,
                Detail = message,
                Duration = 4000
            });
        }

        public void ShowWarningAlert(string message, string summary = "Warning")
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Summary = summary,
                Detail = message,
                Duration = 4000
            });
        }

        public void ShowErrorAlert(string message, string summary = "Error")
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = summary,
                Detail = message,
                Duration = 4000
            });
        }

        public void ShowSuccessAlert(string message, string summary = "Success")
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = summary,
                Detail = message,
                Duration = 4000
            });
        }

        public void ShowCustomPositionAlert(string message, string summary = "Success", string style = "position: absolute; left: -1000px;")
        {
            _notificationService.Notify(new NotificationMessage
            {
                Style = style,
                Severity = NotificationSeverity.Success,
                Summary = summary,
                Detail = message,
                Duration = 40000
            });
        }

        public void ShowAlertWithClickHandler(string message, Action<NotificationMessage> clickHandler, string summary = "Info Click")
        {
            _notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Summary = summary,
                Detail = message,
                Duration = 4000,
                Click = clickHandler,
                CloseOnClick = true
            });
        }
    }
}
