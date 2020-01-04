using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ServicesControl
{
    public partial class Main : Form
    {
        private ServiceController selectedService { get; set; }

        public Main()
        {
            InitializeComponent();

            FillServices();
        }

        private ServiceController GetServiceByDisplayName(string displayName)
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (var service in services)
            {
                if (service.DisplayName == displayName)
                {
                    return service;
                }
            }

            return null;
        }

        private void FillServices()
        {
            ServiceController[] services = ServiceController.GetServices();

            List<string> lstServices = new List<string>();

            foreach (var service in services)
            {
                lstServices.Add(service.DisplayName);
            }

            lstServices.Sort();
            lbxServices.DataSource = lstServices;
        }

        private void lbxServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = lbxServices.GetItemText(lbxServices.SelectedItem);

            ServiceController[] services = ServiceController.GetServices();

            string serviceName = "";

            foreach (var service in services)
            {
                if (service.DisplayName == selected)
                {
                    serviceName = service.ServiceName;
                    selectedService = service;

                    lblStatus.Text = service.Status.ToString();
                    break;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedService == null) return;

                selectedService.Start();

                selectedService.WaitForStatus(ServiceControllerStatus.Running);

                MessageBox.Show(string.Format("The " + selectedService.DisplayName + " service status is now set to {0}.", selectedService.Status.ToString()));
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Could not start the service.");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedService == null) return;

                selectedService.Stop();

                selectedService.WaitForStatus(ServiceControllerStatus.Stopped);

                MessageBox.Show(string.Format("The " + selectedService.DisplayName + " service status is now set to {0}.", selectedService.Status.ToString()));
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Could not stop the service.");
            }
        }
    }
}