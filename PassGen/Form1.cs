using Newtonsoft.Json;
using PassGen.Core;
using PassGen.Strength;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassGen
{
    public partial class Form1 : Form
    {
        private readonly NumericUpDown _length = new NumericUpDown();
        private readonly CheckBox _lower = new CheckBox(), _upper = new CheckBox();
        private readonly CheckBox _digits = new CheckBox(), _symbols = new CheckBox();
        private readonly TextBox _output = new TextBox();
        private readonly Label _strength = new Label();

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PassGen", "settings.json");

        public Form1()
        {
            InitializeComponent();

            Text = "Password Generator";
            ClientSize = new Size(430, 215);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var lblLength = new Label { Text = "Length:", Left = 15, Top = 20, Width = 55 };
            _length.SetBounds(75, 18, 60, 24);
            _length.Minimum = 4; _length.Maximum = 128; _length.Value = 16;

            _lower.Text = "a-z"; _lower.SetBounds(15, 55, 55, 24);
            _upper.Text = "A-Z"; _upper.SetBounds(90, 55, 55, 24);
            _digits.Text = "2-9"; _digits.SetBounds(165, 55, 55, 24);
            _symbols.Text = "!@#"; _symbols.SetBounds(240, 55, 60, 24);

            _output.SetBounds(15, 90, 400, 28);
            _output.Font = new Font("Consolas", 12F);
            _output.ReadOnly = true;

            _strength.SetBounds(15, 124, 400, 20);
            _strength.Text = "Entropy: -";

            var btnGenerate = new Button { Text = "Generate" };
            btnGenerate.SetBounds(15, 155, 120, 34);
            btnGenerate.Click += (s, e) => Generate();

            var btnCopy = new Button { Text = "Copy" };
            btnCopy.SetBounds(150, 155, 120, 34);
            btnCopy.Click += (s, e) =>
            {
                if (_output.Text.Length > 0) Clipboard.SetText(_output.Text);
            };

            Controls.AddRange(new Control[] { lblLength, _length, _lower, _upper,
                _digits, _symbols, _output, _strength, btnGenerate, btnCopy });

            LoadSettings();
            FormClosing += (s, e) => SaveSettings();
        }

        private PasswordOptions CurrentOptions()
        {
            return new PasswordOptions
            {
                Length = (int)_length.Value,
                UseLower = _lower.Checked,
                UseUpper = _upper.Checked,
                UseDigits = _digits.Checked,
                UseSymbols = _symbols.Checked
            };
        }

        private void Generate()
        {
            try
            {
                var opts = CurrentOptions();
                _output.Text = PasswordGenerator.Generate(opts);      

                double bits = StrengthMeter.BitsOfEntropy(opts);     
                _strength.Text = string.Format("Entropy: {0:F1} bits  ({1})",
                    bits, StrengthMeter.Rate(bits));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Password Generator",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadSettings()                                 
        {
            try
            {
                if (!File.Exists(SettingsPath)) { ApplyDefaults(); return; }

                var o = JsonConvert.DeserializeObject<PasswordOptions>(
                    File.ReadAllText(SettingsPath));
                if (o == null) { ApplyDefaults(); return; }

                _length.Value = Math.Min(Math.Max(o.Length, 4), 128);
                _lower.Checked = o.UseLower;
                _upper.Checked = o.UseUpper;
                _digits.Checked = o.UseDigits;
                _symbols.Checked = o.UseSymbols;
            }
            catch { ApplyDefaults(); }
        }

        private void ApplyDefaults()
        {
            _lower.Checked = _upper.Checked = _digits.Checked = _symbols.Checked = true;
        }

        private void SaveSettings()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                File.WriteAllText(SettingsPath,
                    JsonConvert.SerializeObject(CurrentOptions(), Formatting.Indented));
            }
            catch { }
        }
    }
}
