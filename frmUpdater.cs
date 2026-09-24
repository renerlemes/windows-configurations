using System;
using System.Windows.Forms;
using SoundSwitch.Updater;

namespace SoundSwitch
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

            // Marquee até o primeiro relato: entre o clique e a resposta do servidor
            // não existe percentual, e uma barra parada em zero pareceria travada.
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.Visible = true;

            Progress<int> progress = new(percent =>
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = Math.Clamp(percent, 0, 100);
            });

            try
            {
                string installer = await UpdateInstaller.DownloadAsync(_update, progress);

                UpdateInstaller.Start(installer);

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SoundSwitch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                progressBar.Visible = false;
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;

                btnInstalar.Enabled = true;
                UseWaitCursor = false;
            }
        }
    }
}
