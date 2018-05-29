namespace ConvoEditor
{
    partial class FNewProject
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbxProjName = new System.Windows.Forms.TextBox();
            this.tbxBasePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.fbdProjFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCBrowse = new System.Windows.Forms.Button();
            this.tbxConvPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSBrowse = new System.Windows.Forms.Button();
            this.tbxScriptPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Name:";
            // 
            // tbxProjName
            // 
            this.tbxProjName.Location = new System.Drawing.Point(13, 30);
            this.tbxProjName.Name = "tbxProjName";
            this.tbxProjName.Size = new System.Drawing.Size(272, 20);
            this.tbxProjName.TabIndex = 1;
            // 
            // tbxBasePath
            // 
            this.tbxBasePath.Location = new System.Drawing.Point(13, 70);
            this.tbxBasePath.Name = "tbxBasePath";
            this.tbxBasePath.Size = new System.Drawing.Size(535, 20);
            this.tbxBasePath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project Base Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(554, 68);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(554, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(473, 177);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCBrowse
            // 
            this.btnCBrowse.Location = new System.Drawing.Point(554, 108);
            this.btnCBrowse.Name = "btnCBrowse";
            this.btnCBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCBrowse.TabIndex = 9;
            this.btnCBrowse.Text = "Browse";
            this.btnCBrowse.UseVisualStyleBackColor = true;
            this.btnCBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbxConvPath
            // 
            this.tbxConvPath.Location = new System.Drawing.Point(13, 110);
            this.tbxConvPath.Name = "tbxConvPath";
            this.tbxConvPath.Size = new System.Drawing.Size(535, 20);
            this.tbxConvPath.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Conversation File Save Path:";
            // 
            // btnSBrowse
            // 
            this.btnSBrowse.Location = new System.Drawing.Point(554, 148);
            this.btnSBrowse.Name = "btnSBrowse";
            this.btnSBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSBrowse.TabIndex = 12;
            this.btnSBrowse.Text = "Browse";
            this.btnSBrowse.UseVisualStyleBackColor = true;
            this.btnSBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbxScriptPath
            // 
            this.tbxScriptPath.Location = new System.Drawing.Point(13, 150);
            this.tbxScriptPath.Name = "tbxScriptPath";
            this.tbxScriptPath.Size = new System.Drawing.Size(535, 20);
            this.tbxScriptPath.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Script Export Base Path:";
            // 
            // FNewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 211);
            this.Controls.Add(this.btnSBrowse);
            this.Controls.Add(this.tbxScriptPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCBrowse);
            this.Controls.Add(this.tbxConvPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbxBasePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxProjName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FNewProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxProjName;
        private System.Windows.Forms.TextBox tbxBasePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.FolderBrowserDialog fbdProjFolder;
        private System.Windows.Forms.Button btnCBrowse;
        private System.Windows.Forms.TextBox tbxConvPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSBrowse;
        private System.Windows.Forms.TextBox tbxScriptPath;
        private System.Windows.Forms.Label label4;
    }
}