using System.Drawing;
using System.Windows.Forms;

namespace SoundSwitch
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
            tabReproducao = new TabPage();
            lbReproducaoAtalho = new Label();
            txtReproducaoAtalho = new TextBox();
            lvAudioReproducao = new ListView();
            tabGravacao = new TabPage();
            lbGravacaoAtalho = new Label();
            txtGravacaoAtalho = new TextBox();
            lvAudioGravacao = new ListView();
            tabConfiguracoes = new TabPage();
            lbConfigGeral = new Label();
            cbConfigGeralIniciarWindows = new CheckBox();
            lbConfigAudio = new Label();
            cbConfigAudioMudoBloquear = new CheckBox();
            lbConfigNotificacoes = new Label();
            cbConfigNotificacoesMostrar = new CheckBox();
            lbConfigVersao = new Label();
            lbConfigVersaoAtual = new Label();
            lbConfigVersaoDisponivel = new Label();
            btnConfigVersaoAtualizar = new Button();
            pbConfigVersao = new ProgressBar();
            notifyIcon = new NotifyIcon(components);
            cmDevices = new ContextMenuStrip(components);
            cmOptions = new ContextMenuStrip(components);
            lblTrayAppVersion = new ToolStripMenuItem();
            configuraçõesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            sairToolStripMenuItem = new ToolStripMenuItem();
            tabDefault.SuspendLayout();
            tabReproducao.SuspendLayout();
            tabGravacao.SuspendLayout();
            tabConfiguracoes.SuspendLayout();
            cmOptions.SuspendLayout();
            SuspendLayout();
            // 
            // tabDefault
            // 
            tabDefault.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabDefault.Controls.Add(tabReproducao);
            tabDefault.Controls.Add(tabGravacao);
            tabDefault.Controls.Add(tabConfiguracoes);
            tabDefault.Location = new Point(12, 12);
            tabDefault.Name = "tabDefault";
            tabDefault.SelectedIndex = 0;
            tabDefault.Size = new Size(776, 451);
            tabDefault.TabIndex = 0;
            // 
            // tabReproducao
            // 
            tabReproducao.Controls.Add(lbReproducaoAtalho);
            tabReproducao.Controls.Add(txtReproducaoAtalho);
            tabReproducao.Controls.Add(lvAudioReproducao);
            tabReproducao.Location = new Point(4, 24);
            tabReproducao.Name = "tabReproducao";
            tabReproducao.Padding = new Padding(3);
            tabReproducao.Size = new Size(768, 423);
            tabReproducao.TabIndex = 4;
            tabReproducao.Text = "Reprodução";
            tabReproducao.UseVisualStyleBackColor = true;
            // 
            // lbReproducaoAtalho
            // 
            lbReproducaoAtalho.AutoSize = true;
            lbReproducaoAtalho.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbReproducaoAtalho.Location = new Point(6, 400);
            lbReproducaoAtalho.Name = "lbReproducaoAtalho";
            lbReproducaoAtalho.Size = new Size(43, 15);
            lbReproducaoAtalho.TabIndex = 12;
            lbReproducaoAtalho.Text = "Atalho";
            // 
            // txtReproducaoAtalho
            // 
            txtReproducaoAtalho.Location = new Point(60, 397);
            txtReproducaoAtalho.Name = "txtReproducaoAtalho";
            txtReproducaoAtalho.Size = new Size(132, 23);
            txtReproducaoAtalho.TabIndex = 11;
            txtReproducaoAtalho.TextAlign = HorizontalAlignment.Center;
            txtReproducaoAtalho.Enter += shortcutInput_Enter;
            txtReproducaoAtalho.KeyDown += txtReproducaoAtalho_KeyDown;
            txtReproducaoAtalho.Leave += shortcutInput_Leave;
            // 
            // lvAudioReproducao
            // 
            lvAudioReproducao.CheckBoxes = true;
            lvAudioReproducao.Location = new Point(6, 6);
            lvAudioReproducao.Name = "lvAudioReproducao";
            lvAudioReproducao.Size = new Size(756, 387);
            lvAudioReproducao.TabIndex = 8;
            lvAudioReproducao.UseCompatibleStateImageBehavior = false;
            // 
            // tabGravacao
            // 
            tabGravacao.Controls.Add(lbGravacaoAtalho);
            tabGravacao.Controls.Add(txtGravacaoAtalho);
            tabGravacao.Controls.Add(lvAudioGravacao);
            tabGravacao.Location = new Point(4, 24);
            tabGravacao.Name = "tabGravacao";
            tabGravacao.Size = new Size(768, 423);
            tabGravacao.TabIndex = 5;
            tabGravacao.Text = "Gravação";
            tabGravacao.UseVisualStyleBackColor = true;
            // 
            // lbGravacaoAtalho
            // 
            lbGravacaoAtalho.AutoSize = true;
            lbGravacaoAtalho.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbGravacaoAtalho.Location = new Point(6, 400);
            lbGravacaoAtalho.Name = "lbGravacaoAtalho";
            lbGravacaoAtalho.Size = new Size(43, 15);
            lbGravacaoAtalho.TabIndex = 12;
            lbGravacaoAtalho.Text = "Atalho";
            // 
            // txtGravacaoAtalho
            // 
            txtGravacaoAtalho.Location = new Point(60, 397);
            txtGravacaoAtalho.Name = "txtGravacaoAtalho";
            txtGravacaoAtalho.Size = new Size(132, 23);
            txtGravacaoAtalho.TabIndex = 11;
            txtGravacaoAtalho.TextAlign = HorizontalAlignment.Center;
            txtGravacaoAtalho.Enter += shortcutInput_Enter;
            txtGravacaoAtalho.KeyDown += txtGravacaoAtalho_KeyDown;
            txtGravacaoAtalho.Leave += shortcutInput_Leave;
            // 
            // lvAudioGravacao
            // 
            lvAudioGravacao.CheckBoxes = true;
            lvAudioGravacao.Location = new Point(6, 6);
            lvAudioGravacao.Name = "lvAudioGravacao";
            lvAudioGravacao.Size = new Size(756, 387);
            lvAudioGravacao.TabIndex = 9;
            lvAudioGravacao.UseCompatibleStateImageBehavior = false;
            // 
            // tabConfiguracoes
            // 
            tabConfiguracoes.Controls.Add(lbConfigGeral);
            tabConfiguracoes.Controls.Add(cbConfigGeralIniciarWindows);
            tabConfiguracoes.Controls.Add(lbConfigAudio);
            tabConfiguracoes.Controls.Add(cbConfigAudioMudoBloquear);
            tabConfiguracoes.Controls.Add(lbConfigNotificacoes);
            tabConfiguracoes.Controls.Add(cbConfigNotificacoesMostrar);
            tabConfiguracoes.Controls.Add(lbConfigVersao);
            tabConfiguracoes.Controls.Add(lbConfigVersaoAtual);
            tabConfiguracoes.Controls.Add(lbConfigVersaoDisponivel);
            tabConfiguracoes.Controls.Add(btnConfigVersaoAtualizar);
            tabConfiguracoes.Controls.Add(pbConfigVersao);
            tabConfiguracoes.Location = new Point(4, 24);
            tabConfiguracoes.Name = "tabConfiguracoes";
            tabConfiguracoes.Size = new Size(768, 423);
            tabConfiguracoes.TabIndex = 2;
            tabConfiguracoes.Text = "Configurações";
            tabConfiguracoes.UseVisualStyleBackColor = true;
            // 
            // lbConfigGeral
            // 
            lbConfigGeral.AutoSize = true;
            lbConfigGeral.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbConfigGeral.Location = new Point(13, 12);
            lbConfigGeral.Name = "lbConfigGeral";
            lbConfigGeral.Size = new Size(37, 15);
            lbConfigGeral.TabIndex = 0;
            lbConfigGeral.Text = "Geral";
            // 
            // cbConfigGeralIniciarWindows
            // 
            cbConfigGeralIniciarWindows.AutoSize = true;
            cbConfigGeralIniciarWindows.Location = new Point(13, 31);
            cbConfigGeralIniciarWindows.Name = "cbConfigGeralIniciarWindows";
            cbConfigGeralIniciarWindows.Size = new Size(147, 19);
            cbConfigGeralIniciarWindows.TabIndex = 1;
            cbConfigGeralIniciarWindows.Text = "Iniciar com o Windows";
            cbConfigGeralIniciarWindows.UseVisualStyleBackColor = true;
            // 
            // lbConfigAudio
            // 
            lbConfigAudio.AutoSize = true;
            lbConfigAudio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbConfigAudio.Location = new Point(13, 62);
            lbConfigAudio.Name = "lbConfigAudio";
            lbConfigAudio.Size = new Size(39, 15);
            lbConfigAudio.TabIndex = 2;
            lbConfigAudio.Text = "Áudio";
            // 
            // cbConfigAudioMudoBloquear
            // 
            cbConfigAudioMudoBloquear.AutoSize = true;
            cbConfigAudioMudoBloquear.Location = new Point(13, 81);
            cbConfigAudioMudoBloquear.Name = "cbConfigAudioMudoBloquear";
            cbConfigAudioMudoBloquear.Size = new Size(250, 19);
            cbConfigAudioMudoBloquear.TabIndex = 3;
            cbConfigAudioMudoBloquear.Text = "Ao bloquear o computador, ativar o mudo";
            cbConfigAudioMudoBloquear.UseVisualStyleBackColor = true;
            // 
            // lbConfigNotificacoes
            // 
            lbConfigNotificacoes.AutoSize = true;
            lbConfigNotificacoes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbConfigNotificacoes.Location = new Point(13, 112);
            lbConfigNotificacoes.Name = "lbConfigNotificacoes";
            lbConfigNotificacoes.Size = new Size(76, 15);
            lbConfigNotificacoes.TabIndex = 4;
            lbConfigNotificacoes.Text = "Notificações";
            // 
            // cbConfigNotificacoesMostrar
            // 
            cbConfigNotificacoesMostrar.AutoSize = true;
            cbConfigNotificacoesMostrar.Location = new Point(13, 131);
            cbConfigNotificacoesMostrar.Name = "cbConfigNotificacoesMostrar";
            cbConfigNotificacoesMostrar.Size = new Size(262, 19);
            cbConfigNotificacoesMostrar.TabIndex = 5;
            cbConfigNotificacoesMostrar.Text = "Mostrar notificação ao alterar um dispositivo";
            cbConfigNotificacoesMostrar.UseVisualStyleBackColor = true;
            // 
            // lbConfigVersao
            // 
            lbConfigVersao.AutoSize = true;
            lbConfigVersao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbConfigVersao.Location = new Point(400, 12);
            lbConfigVersao.Name = "lbConfigVersao";
            lbConfigVersao.Size = new Size(46, 15);
            lbConfigVersao.TabIndex = 6;
            lbConfigVersao.Text = "Versão";
            // 
            // lbConfigVersaoAtual
            // 
            lbConfigVersaoAtual.AutoSize = true;
            lbConfigVersaoAtual.Location = new Point(400, 31);
            lbConfigVersaoAtual.Name = "lbConfigVersaoAtual";
            lbConfigVersaoAtual.Size = new Size(43, 15);
            lbConfigVersaoAtual.TabIndex = 7;
            lbConfigVersaoAtual.Text = "Atual:";
            // 
            // lbConfigVersaoDisponivel
            // 
            lbConfigVersaoDisponivel.AutoSize = true;
            lbConfigVersaoDisponivel.Location = new Point(400, 50);
            lbConfigVersaoDisponivel.Name = "lbConfigVersaoDisponivel";
            lbConfigVersaoDisponivel.Size = new Size(73, 15);
            lbConfigVersaoDisponivel.TabIndex = 8;
            lbConfigVersaoDisponivel.Text = "Disponível: —";
            // 
            // btnConfigVersaoAtualizar
            // 
            btnConfigVersaoAtualizar.Location = new Point(400, 73);
            btnConfigVersaoAtualizar.Name = "btnConfigVersaoAtualizar";
            btnConfigVersaoAtualizar.Size = new Size(100, 23);
            btnConfigVersaoAtualizar.TabIndex = 9;
            btnConfigVersaoAtualizar.Text = "Atualizar";
            btnConfigVersaoAtualizar.UseVisualStyleBackColor = true;
            btnConfigVersaoAtualizar.Visible = false;
            btnConfigVersaoAtualizar.Click += btnConfigVersaoAtualizar_Click;
            // 
            // pbConfigVersao
            // 
            pbConfigVersao.Location = new Point(400, 102);
            pbConfigVersao.Name = "pbConfigVersao";
            pbConfigVersao.Size = new Size(200, 15);
            pbConfigVersao.TabIndex = 10;
            pbConfigVersao.Visible = false;
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "SoundSwitch";
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
            cmOptions.Items.AddRange(new ToolStripItem[] { lblTrayAppVersion, configuraçõesToolStripMenuItem, toolStripSeparator1, sairToolStripMenuItem });
            cmOptions.Name = "cmOptions";
            cmOptions.Size = new Size(193, 76);
            // 
            // lblTrayAppVersion
            // 
            lblTrayAppVersion.Name = "lblTrayAppVersion";
            lblTrayAppVersion.Size = new Size(192, 22);
            lblTrayAppVersion.Text = "SoundSwitch";
            // 
            // configuraçõesToolStripMenuItem
            // 
            configuraçõesToolStripMenuItem.Name = "configuraçõesToolStripMenuItem";
            configuraçõesToolStripMenuItem.Size = new Size(192, 22);
            configuraçõesToolStripMenuItem.Text = "Configurações";
            configuraçõesToolStripMenuItem.Click += configuraçõesToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(189, 6);
            // 
            // sairToolStripMenuItem
            // 
            sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            sairToolStripMenuItem.Size = new Size(192, 22);
            sairToolStripMenuItem.Text = "Sair";
            sairToolStripMenuItem.Click += sairToolStripMenuItem_Click;
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
            Text = "SoundSwitch";
            FormClosing += frmDefault_FormClosing;
            tabDefault.ResumeLayout(false);
            tabReproducao.ResumeLayout(false);
            tabReproducao.PerformLayout();
            tabGravacao.ResumeLayout(false);
            tabGravacao.PerformLayout();
            tabConfiguracoes.ResumeLayout(false);
            tabConfiguracoes.PerformLayout();
            cmOptions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabDefault;
        private TabPage tabConfiguracoes;
        private Label lbConfigGeral;
        private CheckBox cbConfigGeralIniciarWindows;
        private Label lbConfigAudio;
        private CheckBox cbConfigAudioMudoBloquear;
        private Label lbConfigNotificacoes;
        private CheckBox cbConfigNotificacoesMostrar;
        private Label lbConfigVersao;
        private Label lbConfigVersaoAtual;
        private Label lbConfigVersaoDisponivel;
        private Button btnConfigVersaoAtualizar;
        private ProgressBar pbConfigVersao;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip cmDevices;
        private ContextMenuStrip cmOptions;
        private ToolStripMenuItem lblTrayAppVersion;
        private ToolStripMenuItem configuraçõesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem sairToolStripMenuItem;
        private TabPage tabReproducao;
        private ListView lvAudioReproducao;
        private Label lbReproducaoAtalho;
        private TextBox txtReproducaoAtalho;
        private TabPage tabGravacao;
        private Label lbGravacaoAtalho;
        private TextBox txtGravacaoAtalho;
        private ListView lvAudioGravacao;
    }
}
