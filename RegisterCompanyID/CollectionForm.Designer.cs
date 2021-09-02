
namespace RegisterCompanyID
{
    partial class CollectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUploadNames = new System.Windows.Forms.Button();
            this.cmbSymbol = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lstName = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name : ";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(79, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(146, 20);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Symbol :";
            // 
            // btnUploadNames
            // 
            this.btnUploadNames.Location = new System.Drawing.Point(239, 56);
            this.btnUploadNames.Name = "btnUploadNames";
            this.btnUploadNames.Size = new System.Drawing.Size(64, 34);
            this.btnUploadNames.TabIndex = 4;
            this.btnUploadNames.Text = "Upload Names";
            this.btnUploadNames.UseVisualStyleBackColor = true;
            this.btnUploadNames.Click += new System.EventHandler(this.btnUploadNames_Click);
            // 
            // cmbSymbol
            // 
            this.cmbSymbol.FormattingEnabled = true;
            this.cmbSymbol.Items.AddRange(new object[] {
            "SPY",
            "QQQ",
            "IWM",
            "TLT",
            "TSLA",
            "NFLX",
            "AAPL",
            "AMZN",
            "FB",
            "GOOGL",
            "NVDA"});
            this.cmbSymbol.Location = new System.Drawing.Point(79, 159);
            this.cmbSymbol.Name = "cmbSymbol";
            this.cmbSymbol.Size = new System.Drawing.Size(146, 21);
            this.cmbSymbol.TabIndex = 5;
            this.cmbSymbol.Text = "SPY";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "From Date :";
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "yyyy-MM-dd";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(79, 200);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(146, 20);
            this.dtFrom.TabIndex = 7;
            this.dtFrom.Value = new System.DateTime(2021, 9, 1, 0, 0, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "To Date : ";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "yyyy-MM-dd";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(79, 238);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(146, 20);
            this.dtTo.TabIndex = 9;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(115, 276);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(134, 47);
            this.btnDownload.TabIndex = 10;
            this.btnDownload.Text = "Download CSV";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lstName
            // 
            this.lstName.FormattingEnabled = true;
            this.lstName.Location = new System.Drawing.Point(79, 49);
            this.lstName.Name = "lstName";
            this.lstName.Size = new System.Drawing.Size(146, 95);
            this.lstName.TabIndex = 11;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(240, 97);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(63, 37);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear Names";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 339);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(285, 23);
            this.progressBar.TabIndex = 13;
            // 
            // CollectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 374);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lstName);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbSymbol);
            this.Controls.Add(this.btnUploadNames);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CollectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collection";
            this.Load += new System.EventHandler(this.CollectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUploadNames;
        private System.Windows.Forms.ComboBox cmbSymbol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ListBox lstName;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}