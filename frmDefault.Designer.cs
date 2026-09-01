using System.Drawing;
using System.Windows.Forms;

namespace Windows.Configurations
{
    partial class frmDefault
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefault));
            tabDefault = new TabControl();
            tabAudio = new TabPage();
            label10 = new Label();
            label9 = new Label();
            cbAudioDeviceChangeNotification = new CheckBox();
            lvAudioRecord = new ListView();
            lvAudioPlayback = new ListView();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            txtDeviceRecordShortcut = new TextBox();
            label5 = new Label();
            txtDevicePlaybackShortcut = new TextBox();
            cbAudioMuteOnLock = new CheckBox();
            tabPainelControle = new TabPage();
            cbPainelControleNeverSleepOrTurnOffDisplay = new CheckBox();
            cbPainelControleLidCloseDoNothing = new CheckBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            cbPainelControleDisableStartupSound = new CheckBox();
            cbPainelControleNoSoundScheme = new CheckBox();
            cbPainelControleUAC = new CheckBox();
            tabPersonalizacao = new TabPage();
            cbPersonalizacaoDisableItemsTaskbar = new CheckBox();
            cbPersonalizacaoTaskbarAlignAndSettings = new CheckBox();
            label4 = new Label();
            notifyIcon = new NotifyIcon(components);
            cmDevices = new ContextMenuStrip(components);
            cmOptions = new ContextMenuStrip(components);
            lblTrayAppVersion = new ToolStripMenuItem();
            atualizacaoDisponivelToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparatorUpdate = new ToolStripSeparator();
            configuraçõesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            sairToolStripMenuItem = new ToolStripMenuItem();
            tabGeral = new TabPage();
            cbGeralInitializeWindows = new CheckBox();
            tabDefault.SuspendLayout();
            tabAudio.SuspendLayout();
            tabPainelControle.SuspendLayout();
            tabPersonalizacao.SuspendLayout();
            cmOptions.SuspendLayout();
            tabGeral.SuspendLayout();
            SuspendLayout();
            // 
            // tabDefault
            // 
            tabDefault.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabDefault.Controls.Add(tabGeral);
            tabDefault.Controls.Add(tabAudio);
            tabDefault.Controls.Add(tabPainelControle);
            tabDefault.Controls.Add(tabPersonalizacao);
            tabDefault.Location = new Point(12, 12);
            tabDefault.Name = "tabDefault";
            tabDefault.SelectedIndex = 0;
            tabDefault.Size = new Size(776, 451);
            tabDefault.TabIndex = 0;
            // 
            // tabAudio
            // 
            tabAudio.Controls.Add(label10);
            tabAudio.Controls.Add(label9);
            tabAudio.Controls.Add(cbAudioDeviceChangeNotification);
            tabAudio.Controls.Add(lvAudioRecord);
            tabAudio.Controls.Add(lvAudioPlayback);
            tabAudio.Controls.Add(label8);
            tabAudio.Controls.Add(label7);
            tabAudio.Controls.Add(label6);
            tabAudio.Controls.Add(txtDeviceRecordShortcut);
            tabAudio.Controls.Add(label5);
            tabAudio.Controls.Add(txtDevicePlaybackShortcut);
            tabAudio.Controls.Add(cbAudioMuteOnLock);
            tabAudio.Location = new Point(4, 24);
            tabAudio.Name = "tabAudio";
            tabAudio.Size = new Size(768, 423);
            tabAudio.TabIndex = 2;
            tabAudio.Text = "Áudio";
            tabAudio.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label10.Location = new Point(464, 82);
            label10.Name = "label10";
            label10.Size = new Size(86, 15);
            label10.TabIndex = 11;
            label10.Text = "Configurações";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label9.Location = new Point(464, 12);
            label9.Name = "label9";
            label9.Size = new Size(48, 15);
            label9.TabIndex = 10;
            label9.Text = "Atalhos";
            // 
            // cbAudioDeviceChangeNotification
            // 
            cbAudioDeviceChangeNotification.AutoSize = true;
            cbAudioDeviceChangeNotification.Location = new Point(464, 101);
            cbAudioDeviceChangeNotification.Name = "cbAudioDeviceChangeNotification";
            cbAudioDeviceChangeNotification.Size = new Size(241, 19);
            cbAudioDeviceChangeNotification.TabIndex = 9;
            cbAudioDeviceChangeNotification.Text = "Mostrar notificação ao alterar dispositivo";
            cbAudioDeviceChangeNotification.UseVisualStyleBackColor = true;
            // 
            // lvAudioRecord
            // 
            lvAudioRecord.CheckBoxes = true;
            lvAudioRecord.Location = new Point(13, 227);
            lvAudioRecord.Name = "lvAudioRecord";
            lvAudioRecord.Size = new Size(441, 176);
            lvAudioRecord.TabIndex = 8;
            lvAudioRecord.UseCompatibleStateImageBehavior = false;
            // 
            // lvAudioPlayback
            // 
            lvAudioPlayback.CheckBoxes = true;
            lvAudioPlayback.Location = new Point(13, 30);
            lvAudioPlayback.Name = "lvAudioPlayback";
            lvAudioPlayback.Size = new Size(441, 176);
            lvAudioPlayback.TabIndex = 0;
            lvAudioPlayback.UseCompatibleStateImageBehavior = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(13, 209);
            label8.Name = "label8";
            label8.Size = new Size(59, 15);
            label8.TabIndex = 7;
            label8.Text = "Gravação";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(13, 12);
            label7.Name = "label7";
            label7.Size = new Size(74, 15);
            label7.TabIndex = 7;
            label7.Text = "Reprodução";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(618, 30);
            label6.Name = "label6";
            label6.Size = new Size(56, 15);
            label6.TabIndex = 6;
            label6.Text = "Gravação";
            // 
            // txtDeviceRecordShortcut
            // 
            txtDeviceRecordShortcut.Location = new Point(618, 49);
            txtDeviceRecordShortcut.Name = "txtDeviceRecordShortcut";
            txtDeviceRecordShortcut.Size = new Size(132, 23);
            txtDeviceRecordShortcut.TabIndex = 5;
            txtDeviceRecordShortcut.TextAlign = HorizontalAlignment.Center;
            txtDeviceRecordShortcut.Enter += shortcutInput_Enter;
            txtDeviceRecordShortcut.KeyDown += txtDeviceRecordShortcut_KeyDown;
            txtDeviceRecordShortcut.Leave += shortcutInput_Leave;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(464, 30);
            label5.Name = "label5";
            label5.Size = new Size(71, 15);
            label5.TabIndex = 4;
            label5.Text = "Reprodução";
            // 
            // txtDevicePlaybackShortcut
            // 
            txtDevicePlaybackShortcut.Location = new Point(464, 48);
            txtDevicePlaybackShortcut.Name = "txtDevicePlaybackShortcut";
            txtDevicePlaybackShortcut.Size = new Size(132, 23);
            txtDevicePlaybackShortcut.TabIndex = 3;
            txtDevicePlaybackShortcut.TextAlign = HorizontalAlignment.Center;
            txtDevicePlaybackShortcut.Enter += shortcutInput_Enter;
            txtDevicePlaybackShortcut.KeyDown += txtDevicePlaybackShortcut_KeyDown;
            txtDevicePlaybackShortcut.Leave += shortcutInput_Leave;
            // 
            // cbAudioMuteOnLock
            // 
            cbAudioMuteOnLock.AutoSize = true;
            cbAudioMuteOnLock.Location = new Point(464, 124);
            cbAudioMuteOnLock.Name = "cbAudioMuteOnLock";
            cbAudioMuteOnLock.Size = new Size(250, 19);
            cbAudioMuteOnLock.TabIndex = 2;
            cbAudioMuteOnLock.Text = "Ao bloquear o computador, ativar o mudo";
            cbAudioMuteOnLock.UseVisualStyleBackColor = true;
            // 
            // tabPainelControle
            // 
            tabPainelControle.Controls.Add(cbPainelControleNeverSleepOrTurnOffDisplay);
            tabPainelControle.Controls.Add(cbPainelControleLidCloseDoNothing);
            tabPainelControle.Controls.Add(label3);
            tabPainelControle.Controls.Add(label2);
            tabPainelControle.Controls.Add(label1);
            tabPainelControle.Controls.Add(cbPainelControleDisableStartupSound);
            tabPainelControle.Controls.Add(cbPainelControleNoSoundScheme);
            tabPainelControle.Controls.Add(cbPainelControleUAC);
            tabPainelControle.Location = new Point(4, 24);
            tabPainelControle.Name = "tabPainelControle";
            tabPainelControle.Padding = new Padding(3);
            tabPainelControle.Size = new Size(768, 423);
            tabPainelControle.TabIndex = 0;
            tabPainelControle.Text = "Painel de Controle";
            tabPainelControle.UseVisualStyleBackColor = true;
            // 
            // cbPainelControleNeverSleepOrTurnOffDisplay
            // 
            cbPainelControleNeverSleepOrTurnOffDisplay.AutoSize = true;
            cbPainelControleNeverSleepOrTurnOffDisplay.Location = new Point(13, 196);
            cbPainelControleNeverSleepOrTurnOffDisplay.Name = "cbPainelControleNeverSleepOrTurnOffDisplay";
            cbPainelControleNeverSleepOrTurnOffDisplay.Size = new Size(403, 19);
            cbPainelControleNeverSleepOrTurnOffDisplay.TabIndex = 5;
            cbPainelControleNeverSleepOrTurnOffDisplay.Text = "Desliga o vídeo e suspende o computador: Nunca (bateria e conectado)";
            cbPainelControleNeverSleepOrTurnOffDisplay.UseVisualStyleBackColor = true;
            // 
            // cbPainelControleLidCloseDoNothing
            // 
            cbPainelControleLidCloseDoNothing.AutoSize = true;
            cbPainelControleLidCloseDoNothing.Location = new Point(13, 168);
            cbPainelControleLidCloseDoNothing.Name = "cbPainelControleLidCloseDoNothing";
            cbPainelControleLidCloseDoNothing.Size = new Size(311, 19);
            cbPainelControleLidCloseDoNothing.TabIndex = 4;
            cbPainelControleLidCloseDoNothing.Text = "Ao fechar a tampa, não faz nada (bateria e conectado)";
            cbPainelControleLidCloseDoNothing.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(13, 144);
            label3.Name = "label3";
            label3.Size = new Size(109, 15);
            label3.TabIndex = 3;
            label3.Text = "Opções de Energia";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(13, 92);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 3;
            label2.Text = "Contas de Usuário";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(13, 12);
            label1.Name = "label1";
            label1.Size = new Size(101, 15);
            label1.TabIndex = 3;
            label1.Text = "Hardware e Sons";
            // 
            // cbPainelControleDisableStartupSound
            // 
            cbPainelControleDisableStartupSound.AutoSize = true;
            cbPainelControleDisableStartupSound.Location = new Point(13, 64);
            cbPainelControleDisableStartupSound.Name = "cbPainelControleDisableStartupSound";
            cbPainelControleDisableStartupSound.Size = new Size(266, 19);
            cbPainelControleDisableStartupSound.TabIndex = 2;
            cbPainelControleDisableStartupSound.Text = "Não tocar o som na inicialização do Windows";
            cbPainelControleDisableStartupSound.UseVisualStyleBackColor = true;
            // 
            // cbPainelControleNoSoundScheme
            // 
            cbPainelControleNoSoundScheme.AutoSize = true;
            cbPainelControleNoSoundScheme.Location = new Point(13, 36);
            cbPainelControleNoSoundScheme.Name = "cbPainelControleNoSoundScheme";
            cbPainelControleNoSoundScheme.Size = new Size(273, 19);
            cbPainelControleNoSoundScheme.TabIndex = 1;
            cbPainelControleNoSoundScheme.Text = "Define o esquema de som como Nenhum som";
            cbPainelControleNoSoundScheme.UseVisualStyleBackColor = true;
            // 
            // cbPainelControleUAC
            // 
            cbPainelControleUAC.AutoSize = true;
            cbPainelControleUAC.Location = new Point(13, 116);
            cbPainelControleUAC.Name = "cbPainelControleUAC";
            cbPainelControleUAC.Size = new Size(348, 19);
            cbPainelControleUAC.TabIndex = 0;
            cbPainelControleUAC.Text = "Define o Controle de Conta de Usuário como Nunca notificar";
            cbPainelControleUAC.UseVisualStyleBackColor = true;
            // 
            // tabPersonalizacao
            // 
            tabPersonalizacao.Controls.Add(cbPersonalizacaoDisableItemsTaskbar);
            tabPersonalizacao.Controls.Add(cbPersonalizacaoTaskbarAlignAndSettings);
            tabPersonalizacao.Controls.Add(label4);
            tabPersonalizacao.Location = new Point(4, 24);
            tabPersonalizacao.Name = "tabPersonalizacao";
            tabPersonalizacao.Size = new Size(768, 423);
            tabPersonalizacao.TabIndex = 1;
            tabPersonalizacao.Text = "Personalização";
            tabPersonalizacao.UseVisualStyleBackColor = true;
            // 
            // cbPersonalizacaoDisableItemsTaskbar
            // 
            cbPersonalizacaoDisableItemsTaskbar.AutoSize = true;
            cbPersonalizacaoDisableItemsTaskbar.Location = new Point(13, 62);
            cbPersonalizacaoDisableItemsTaskbar.Name = "cbPersonalizacaoDisableItemsTaskbar";
            cbPersonalizacaoDisableItemsTaskbar.Size = new Size(364, 19);
            cbPersonalizacaoDisableItemsTaskbar.TabIndex = 10;
            cbPersonalizacaoDisableItemsTaskbar.Text = "Oculta Pesquisa e desativa Visão de tarefas, Widgets e Continuar";
            cbPersonalizacaoDisableItemsTaskbar.UseVisualStyleBackColor = true;
            // 
            // cbPersonalizacaoTaskbarAlignAndSettings
            // 
            cbPersonalizacaoTaskbarAlignAndSettings.AutoSize = true;
            cbPersonalizacaoTaskbarAlignAndSettings.Location = new Point(13, 35);
            cbPersonalizacaoTaskbarAlignAndSettings.Name = "cbPersonalizacaoTaskbarAlignAndSettings";
            cbPersonalizacaoTaskbarAlignAndSettings.Size = new Size(385, 19);
            cbPersonalizacaoTaskbarAlignAndSettings.TabIndex = 9;
            cbPersonalizacaoTaskbarAlignAndSettings.Text = "Alinha a barra à esquerda e aplica os comportamentos configurados";
            cbPersonalizacaoTaskbarAlignAndSettings.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(13, 12);
            label4.Name = "label4";
            label4.Size = new Size(97, 15);
            label4.TabIndex = 8;
            label4.Text = "Barra de Tarefas";
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "Windows Configurations";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            // 
            // cmDevices
            // 
            cmDevices.Name = "cmDevices";
            cmDevices.Size = new Size(61, 4);
            // 
            // cmOptions
            // 
            cmOptions.Items.AddRange(new ToolStripItem[] { lblTrayAppVersion, atualizacaoDisponivelToolStripMenuItem, toolStripSeparatorUpdate, configuraçõesToolStripMenuItem, toolStripSeparator1, sairToolStripMenuItem });
            cmOptions.Name = "cmOptions";
            cmOptions.Size = new Size(152, 98);
            // 
            // lblTrayAppVersion
            // 
            lblTrayAppVersion.Name = "lblTrayAppVersion";
            lblTrayAppVersion.Size = new Size(151, 22);
            lblTrayAppVersion.Text = "Windows Configurations";
            lblTrayAppVersion.Click += lblTrayAppVersion_Click;
            // 
            // atualizacaoDisponivelToolStripMenuItem
            // 
            atualizacaoDisponivelToolStripMenuItem.Name = "atualizacaoDisponivelToolStripMenuItem";
            atualizacaoDisponivelToolStripMenuItem.Size = new Size(151, 22);
            atualizacaoDisponivelToolStripMenuItem.Text = "Atualização disponível";
            atualizacaoDisponivelToolStripMenuItem.Visible = false;
            atualizacaoDisponivelToolStripMenuItem.Click += atualizacaoDisponivelToolStripMenuItem_Click;
            // 
            // toolStripSeparatorUpdate
            // 
            toolStripSeparatorUpdate.Name = "toolStripSeparatorUpdate";
            toolStripSeparatorUpdate.Size = new Size(148, 6);
            // 
            // configuraçõesToolStripMenuItem
            // 
            configuraçõesToolStripMenuItem.Name = "configuraçõesToolStripMenuItem";
            configuraçõesToolStripMenuItem.Size = new Size(151, 22);
            configuraçõesToolStripMenuItem.Text = "Configurações";
            configuraçõesToolStripMenuItem.Click += configuraçõesToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(148, 6);
            // 
            // sairToolStripMenuItem
            // 
            sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            sairToolStripMenuItem.Size = new Size(151, 22);
            sairToolStripMenuItem.Text = "Sair";
            sairToolStripMenuItem.Click += sairToolStripMenuItem_Click;
            // 
            // tabGeral
            // 
            tabGeral.Controls.Add(cbGeralInitializeWindows);
            tabGeral.Location = new Point(4, 24);
            tabGeral.Name = "tabGeral";
            tabGeral.Size = new Size(768, 423);
            tabGeral.TabIndex = 3;
            tabGeral.Text = "Geral";
            tabGeral.UseVisualStyleBackColor = true;
            // 
            // cbGeralInitializeWindows
            // 
            cbGeralInitializeWindows.AutoSize = true;
            cbGeralInitializeWindows.Location = new Point(13, 12);
            cbGeralInitializeWindows.Name = "cbGeralInitializeWindows";
            cbGeralInitializeWindows.Size = new Size(147, 19);
            cbGeralInitializeWindows.TabIndex = 9;
            cbGeralInitializeWindows.Text = "Iniciar com o Windows";
            cbGeralInitializeWindows.UseVisualStyleBackColor = true;
            // 
            // frmDefault
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 475);
            Controls.Add(tabDefault);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "frmDefault";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Windows Configurations";
            FormClosing += frmDefault_FormClosing;
            tabDefault.ResumeLayout(false);
            tabAudio.ResumeLayout(false);
            tabAudio.PerformLayout();
            tabPainelControle.ResumeLayout(false);
            tabPainelControle.PerformLayout();
            tabPersonalizacao.ResumeLayout(false);
            tabPersonalizacao.PerformLayout();
            cmOptions.ResumeLayout(false);
            tabGeral.ResumeLayout(false);
            tabGeral.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabDefault;
        private TabPage tabPainelControle;
        private CheckBox cbPainelControleUAC;
        private CheckBox cbPainelControleNoSoundScheme;
        private CheckBox cbPainelControleDisableStartupSound;
        private Label label2;
        private Label label1;
        private Label label3;
        private CheckBox cbPainelControleLidCloseDoNothing;
        private CheckBox cbPainelControleNeverSleepOrTurnOffDisplay;
        private TabPage tabPersonalizacao;
        private CheckBox cbPersonalizacaoDisableItemsTaskbar;
        private CheckBox cbPersonalizacaoTaskbarAlignAndSettings;
        private Label label4;
        private TabPage tabAudio;
        private CheckBox cbAudioMuteOnLock;
        private Label label6;
        private TextBox txtDeviceRecordShortcut;
        private Label label5;
        private TextBox txtDevicePlaybackShortcut;
        private NotifyIcon notifyIcon;
        private Label label8;
        private Label label7;
        private ListView lvAudioRecord;
        private ListView lvAudioPlayback;
        private ContextMenuStrip cmDevices;
        private ContextMenuStrip cmOptions;
        private ToolStripMenuItem lblTrayAppVersion;
        private ToolStripMenuItem atualizacaoDisponivelToolStripMenuItem;
        private ToolStripSeparator toolStripSeparatorUpdate;
        private ToolStripMenuItem configuraçõesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem sairToolStripMenuItem;
        private CheckBox cbAudioDeviceChangeNotification;
        private Label label10;
        private Label label9;
        private TabPage tabGeral;
        private CheckBox cbGeralInitializeWindows;
    }
}
