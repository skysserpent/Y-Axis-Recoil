using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace YourNameSpace
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(string message, string title)
        {
            InitializeComponent();
            this.Text = title;
            this.labelMessage.Text = message;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Overriding OnLoad to add rounded corners
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int borderRadius = 8; // Radius for the rounded corners
            int width = this.Width;
            int height = this.Height;

            // Create a GraphicsPath for rounded corners
            GraphicsPath path = new GraphicsPath();
            // Right side is the rounding left side is the positioning 
            path.AddArc(0, 0, borderRadius, borderRadius, 180, 150); // Top-left corner
            path.AddArc(width - borderRadius - 1, 0, borderRadius, borderRadius, 270, 150); // Top-right corner
            path.AddArc(width - borderRadius - 1, height - borderRadius - 1, borderRadius, borderRadius, 0, 150); // Bottom-right corner
            path.AddArc(0, height - borderRadius - 1, borderRadius, borderRadius, 150, 150); // Bottom-left corner
            path.CloseAllFigures();

            // Apply the rounded region to the form
            this.Region = new Region(path);

            // Optional: To make sure the background color is filled in after rounding
            this.BackColor = Color.FromArgb(27, 27, 27); // Example color, you can change it
        }

        public static void Show(string message, string title = "Message")
        {
            CustomMessageBox msgBox = new CustomMessageBox(message, title);
            msgBox.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomMessageBox_Load(object sender, EventArgs e)
        {
            // Additional code can be added here if needed
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
