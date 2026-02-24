namespace ProductsImporter
{
    partial class FrmInportListini
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtPercorsoFile;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.CheckBox ChkImportaPrezzi;
        private System.Windows.Forms.CheckBox ChkCreaArticoli;
        private System.Windows.Forms.Button btnImportaListino;
        private System.Windows.Forms.ProgressBar progressBarImport;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPercorsoFile = new System.Windows.Forms.TextBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.ChkImportaPrezzi = new System.Windows.Forms.CheckBox();
            this.ChkCreaArticoli = new System.Windows.Forms.CheckBox();
            this.btnImportaListino = new System.Windows.Forms.Button();
            this.progressBarImport = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPercorsoFile
            // 
            this.txtPercorsoFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPercorsoFile.Location = new System.Drawing.Point(12, 169);
            this.txtPercorsoFile.Name = "txtPercorsoFile";
            this.txtPercorsoFile.Size = new System.Drawing.Size(770, 30);
            this.txtPercorsoFile.TabIndex = 0;
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSfoglia.Location = new System.Drawing.Point(798, 169);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(185, 55);
            this.btnSfoglia.TabIndex = 1;
            this.btnSfoglia.Text = "Sfoglia...";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // ChkImportaPrezzi
            // 
            this.ChkImportaPrezzi.AutoSize = true;
            this.ChkImportaPrezzi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkImportaPrezzi.Location = new System.Drawing.Point(18, 228);
            this.ChkImportaPrezzi.Name = "ChkImportaPrezzi";
            this.ChkImportaPrezzi.Size = new System.Drawing.Size(156, 29);
            this.ChkImportaPrezzi.TabIndex = 2;
            this.ChkImportaPrezzi.Text = "Importa prezzi";
            this.ChkImportaPrezzi.UseVisualStyleBackColor = true;
            // 
            // ChkCreaArticoli
            // 
            this.ChkCreaArticoli.AutoSize = true;
            this.ChkCreaArticoli.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkCreaArticoli.Location = new System.Drawing.Point(230, 228);
            this.ChkCreaArticoli.Name = "ChkCreaArticoli";
            this.ChkCreaArticoli.Size = new System.Drawing.Size(137, 29);
            this.ChkCreaArticoli.TabIndex = 3;
            this.ChkCreaArticoli.Text = "Crea articoli";
            this.ChkCreaArticoli.UseVisualStyleBackColor = true;
            // 
            // btnImportaListino
            // 
            this.btnImportaListino.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportaListino.Location = new System.Drawing.Point(18, 283);
            this.btnImportaListino.Name = "btnImportaListino";
            this.btnImportaListino.Size = new System.Drawing.Size(177, 50);
            this.btnImportaListino.TabIndex = 4;
            this.btnImportaListino.Text = "Importa listino";
            this.btnImportaListino.UseVisualStyleBackColor = true;
            this.btnImportaListino.Click += new System.EventHandler(this.btnImportaListino_Click);
            // 
            // progressBarImport
            // 
            this.progressBarImport.Location = new System.Drawing.Point(210, 283);
            this.progressBarImport.Name = "progressBarImport";
            this.progressBarImport.Size = new System.Drawing.Size(995, 41);
            this.progressBarImport.TabIndex = 5;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Location = new System.Drawing.Point(0, 353);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1243, 415);
            this.txtLog.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Excel Files|*.xlsx;*.xls";
            this.openFileDialog1.Title = "Seleziona file listino";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1243, 143);
            this.panel1.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ProductsImporter.Properties.Resources.Gemini_Generated_Image_vfk2zwvfk2zwvfk2;
            this.pictureBox1.Location = new System.Drawing.Point(19, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(258, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(340, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(525, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "IMPORTATORE ARTICOLI";
            // 
            // FrmInportListini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1243, 768);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.progressBarImport);
            this.Controls.Add(this.btnImportaListino);
            this.Controls.Add(this.ChkCreaArticoli);
            this.Controls.Add(this.ChkImportaPrezzi);
            this.Controls.Add(this.btnSfoglia);
            this.Controls.Add(this.txtPercorsoFile);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInportListini";
            this.Text = "Importa prodotti";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}

