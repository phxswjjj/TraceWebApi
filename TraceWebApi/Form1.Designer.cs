﻿
namespace TraceWebApi
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbxSetting = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTestWebApi = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.btnExecute = new System.Windows.Forms.Button();
            this.gbxSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxSetting
            // 
            this.gbxSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxSetting.Controls.Add(this.numInterval);
            this.gbxSetting.Controls.Add(this.label2);
            this.gbxSetting.Controls.Add(this.txtTestWebApi);
            this.gbxSetting.Controls.Add(this.label1);
            this.gbxSetting.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.gbxSetting.Location = new System.Drawing.Point(12, 12);
            this.gbxSetting.Name = "gbxSetting";
            this.gbxSetting.Size = new System.Drawing.Size(776, 108);
            this.gbxSetting.TabIndex = 0;
            this.gbxSetting.TabStop = false;
            this.gbxSetting.Text = "Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Test Web Api：";
            // 
            // txtTestWebApi
            // 
            this.txtTestWebApi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestWebApi.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtTestWebApi.Location = new System.Drawing.Point(152, 24);
            this.txtTestWebApi.Name = "txtTestWebApi";
            this.txtTestWebApi.Size = new System.Drawing.Size(618, 30);
            this.txtTestWebApi.TabIndex = 1;
            this.txtTestWebApi.Text = "https://gorest.co.in/public/v1/users";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Interval(sec)：";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(152, 63);
            this.numInterval.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(118, 30);
            this.numInterval.TabIndex = 3;
            this.numInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnExecute.Location = new System.Drawing.Point(12, 126);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(105, 42);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Start";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.gbxSetting);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gbxSetting.ResumeLayout(false);
            this.gbxSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxSetting;
        private System.Windows.Forms.TextBox txtTestWebApi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExecute;
    }
}

