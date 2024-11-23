using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YourNameSpace;

namespace RecoilControl
{
    public partial class MainForm : Form
    {
        Thread t;
        private const string ConfigDirectory = @"C:\Recoil Control Configs\";

        public MainForm()
        {
            InitializeComponent();
            Directory.CreateDirectory(ConfigDirectory);
            LoadConfigList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainTimer.Start();
            t = new Thread(Recoil.Loop);
            t.Start();
        }

        private void Enable()
        {
            ChangeEnabled(false);
            Recoil.sleeptime = (int)numericUpDown2.Value;
            Recoil.strength = (int)numericUpDown1.Value;
        }

        private void Disable()
        {
            ChangeEnabled(true);
        }

        private void ChangeEnabled(bool enabled)
        {
            numericUpDown1.Enabled = enabled;
            numericUpDown2.Enabled = enabled;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Recoil.Enabled)
            {
                lblEnabledorDisabled.Text = "Enabled";
                lblEnabledorDisabled.ForeColor = Color.FromArgb(51, 255, 160);
                Enable();
            }
            else
            {
                lblEnabledorDisabled.Text = "Disabled";
                lblEnabledorDisabled.ForeColor = Color.Red;
                Disable();
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();
        }
        private void LoadConfigList()
        {
            comboBoxConfigs.Items.Clear();

            if (Directory.Exists(ConfigDirectory))
            {
                var configFiles = Directory.GetFiles(ConfigDirectory, "*.json")
                                           .Select(Path.GetFileNameWithoutExtension)
                                           .ToArray();

                comboBoxConfigs.Items.AddRange(configFiles);
            }
        }
        private void btnSaveConfig_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfigName.Text))
            {
                CustomMessageBox.Show("Please enter a valid config name.", "Error");
                return;
            }

            string configName = txtConfigName.Text.Trim();
            string configPath = Path.Combine(@"C:\Recoil Control Configs\", configName + ".json");

            var config = new
            {
                SleepTime = (int)numericUpDown2.Value,
                Strength = (int)numericUpDown1.Value
            };

            try
            {
                Directory.CreateDirectory(@"C:\Recoil Control Configs\"); // Ensure the directory exists
                File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
                CustomMessageBox.Show($"Config '{configName}' saved successfully!", "Success");
                LoadConfigList(); // Refresh the dropdown list
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"Failed to save config. Error: {ex.Message}", "Error");
            }
        }
        private void btnLoadConfig_Click_1(object sender, EventArgs e)
        {
            if (comboBoxConfigs.SelectedItem == null)
            {
                CustomMessageBox.Show("Please select a config to load.", "Error");
                return;
            }

            string configName = comboBoxConfigs.SelectedItem.ToString();
            string configPath = Path.Combine(@"C:\Recoil Control Configs\", configName + ".json");

            if (!File.Exists(configPath))
            {
                CustomMessageBox.Show($"Config file '{configName}' not found!", "Error");
                return;
            }

            try
            {
                var config = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(configPath));
                numericUpDown2.Value = (int)config.SleepTime;
                numericUpDown1.Value = (int)config.Strength;
                CustomMessageBox.Show($"Config '{configName}' loaded successfully!", "Success");
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"Failed to load config. Error: {ex.Message}", "Error");
            }
        }
        private void txtConfigName_TextChanged(object sender, EventArgs e)
        {

        }
        private void comboBoxConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
