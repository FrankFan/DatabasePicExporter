namespace DatabasePicExporter
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUid = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtDb = new System.Windows.Forms.TextBox();
            this.txtDetail = new System.Windows.Forms.TextBox();
            this.btnExe = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnOpenIt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "uid:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "pwd:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Database:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(107, 27);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(123, 21);
            this.txtServer.TabIndex = 4;
            // 
            // txtUid
            // 
            this.txtUid.Location = new System.Drawing.Point(107, 62);
            this.txtUid.Name = "txtUid";
            this.txtUid.Size = new System.Drawing.Size(123, 21);
            this.txtUid.TabIndex = 5;
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(107, 94);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(123, 21);
            this.txtPwd.TabIndex = 6;
            // 
            // txtDb
            // 
            this.txtDb.Location = new System.Drawing.Point(107, 128);
            this.txtDb.Name = "txtDb";
            this.txtDb.Size = new System.Drawing.Size(123, 21);
            this.txtDb.TabIndex = 7;
            // 
            // txtDetail
            // 
            this.txtDetail.Location = new System.Drawing.Point(256, 26);
            this.txtDetail.Multiline = true;
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            this.txtDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDetail.Size = new System.Drawing.Size(286, 180);
            this.txtDetail.TabIndex = 8;
            // 
            // btnExe
            // 
            this.btnExe.Location = new System.Drawing.Point(90, 172);
            this.btnExe.Name = "btnExe";
            this.btnExe.Size = new System.Drawing.Size(75, 23);
            this.btnExe.TabIndex = 9;
            this.btnExe.Text = "执行";
            this.btnExe.UseVisualStyleBackColor = true;
            this.btnExe.Click += new System.EventHandler(this.btbExe_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(32, 243);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(403, 21);
            this.txtStatus.TabIndex = 10;
            // 
            // btnOpenIt
            // 
            this.btnOpenIt.Location = new System.Drawing.Point(462, 240);
            this.btnOpenIt.Name = "btnOpenIt";
            this.btnOpenIt.Size = new System.Drawing.Size(75, 23);
            this.btnOpenIt.TabIndex = 11;
            this.btnOpenIt.Text = "OpenIt";
            this.btnOpenIt.UseVisualStyleBackColor = true;
            this.btnOpenIt.Click += new System.EventHandler(this.btnOpenIt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 321);
            this.Controls.Add(this.btnOpenIt);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnExe);
            this.Controls.Add(this.txtDetail);
            this.Controls.Add(this.txtDb);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUid);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Database Pic  Exporter    by fanyong@gmail.com";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUid;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.TextBox txtDetail;
        private System.Windows.Forms.Button btnExe;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnOpenIt;
    }
}

