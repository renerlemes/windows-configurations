using System;
using System.Windows.Forms;
using Windows.Configurations.Updater;

namespace Windows.Configurations
{
    public partial class frmUpdater : Form
    {
        private readonly AvailableUpdate _update;

        public frmUpdater()
        {
            InitializeComponent();
        }

        internal frmUpdater(AvailableUpdate update) : this()
        {
            _update = update;

            if (update is null)
            {
                btnInstalar.Enabled = false;
                return;
            }

            txtChangelog.Text = string.IsNullOrWhiteSpace(update.Changelog)
                ? "Nenhuma nota de versão."
                : update.Changelog;
        }

        private async void btnInstalar_Click(object sender, EventArgs e)
        {
            if (_update is null)
                return;

            btnInstalar.Enabled = false;
            UseWaitCursor = true;

            try
            {
                string installer = await UpdateInstaller.DownloadAsync(_update);

                UpdateInstaller.Start(installer);

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Windows Configurations",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                btnInstalar.Enabled = true;
                UseWaitCursor = false;
            }
        }
    }
}
