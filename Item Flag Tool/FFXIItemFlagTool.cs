using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

public class FFXIItemFlagTool : Form
{
    // Define the hexadecimal flags as a C# class and list
    private class HexItem
    {
        public required string FlagName { get; set; }
        public int HexValue { get; set; }
    }

    private List<HexItem> hexNumbers = new List<HexItem>
    {
        new HexItem { FlagName = "WALLHANGING", HexValue = 0x0001 },
        new HexItem { FlagName = "ITEM FLAG 01", HexValue = 0x0002 },
        new HexItem { FlagName = "AVAILABLE FROM MYSTERY BOX (GOBBIE BOX ECT.)", HexValue = 0x0004 },
        new HexItem { FlagName = "AVAILABLE FROM MOG GARDEN", HexValue = 0x0008 },
        new HexItem { FlagName = "CAN MAIL TO SAME ACCOUNT", HexValue = 0x0010 },
        new HexItem { FlagName = "INSCRIBABLE", HexValue = 0x0020 },
        new HexItem { FlagName = "CANNOT PUT UP FOR AUCTION", HexValue = 0x0040 },
        new HexItem { FlagName = "ITEM IS A SCROLL", HexValue = 0x0080 },
        new HexItem { FlagName = "LINKSHELL (PEARL/SACK)", HexValue = 0x0100 },
        new HexItem { FlagName = "CAN USE ITEM (EXAMPLE: CHARGED ITEMS)", HexValue = 0x0200 },
        new HexItem { FlagName = "CAN TRADE TO AN NPC", HexValue = 0x0400 },
        new HexItem { FlagName = "CAN EQUIP ITEM", HexValue = 0x0800 },
        new HexItem { FlagName = "NOT SELABLE", HexValue = 0x1000 },
        new HexItem { FlagName = "NO DELIVERY FROM AH", HexValue = 0x2000 },
        new HexItem { FlagName = "EX", HexValue = 0x4000 },
        new HexItem { FlagName = "RARE", HexValue = 0x8000 }
    };

    private Label totalSumLabel;
    private FlowLayoutPanel hexListPanel;

    public FFXIItemFlagTool()
    {
        // Form properties
        Text = "FFXI ITEM FLAG TOOL";
        BackColor = ColorTranslator.FromHtml("#2D3748"); // Dark Grey
        Size = new Size(400, 655);
        StartPosition = FormStartPosition.CenterScreen;

        // Main layout panel for the entire form content
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            RowCount = 3,
            ColumnCount = 1
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for Title and description
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Row for flags, will take up remaining space
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Row for result panel
        Controls.Add(mainLayout);

        // Top Section: Title and description
        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(0, 0, 0, 10)
        };
        mainLayout.Controls.Add(topPanel, 0, 0);

        var titleLabel = new Label
        {
            Text = "FFXI ITEM FLAG TOOL",
            Font = new Font("Arial", 16, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#E2E8F0"), // Off-white
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleCenter
        };
        topPanel.Controls.Add(titleLabel);

        var descriptionLabel = new Label
        {
            Text = "Select the flags you want to sum. The total will update automatically.",
            Font = new Font("Arial", 8),
            ForeColor = ColorTranslator.FromHtml("#A0AEC0"), // Gray
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleCenter
        };
        topPanel.Controls.Add(descriptionLabel);

        // Middle Section: Checkbox List
        var checkboxListPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(0, 0, 0, 10),
        };
        mainLayout.Controls.Add(checkboxListPanel, 0, 1);

        hexListPanel = checkboxListPanel; // Assign the new panel to the class variable

        // Bottom Section: Result
        var resultPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            BackColor = ColorTranslator.FromHtml("#4A5568"),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 10, 0, 0),
            ColumnCount = 1,
            RowCount = 3,
            AutoSize = true,
        };
        resultPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        resultPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        resultPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        resultPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainLayout.Controls.Add(resultPanel, 0, 2);

        var resultLabel = new Label
        {
            Text = "ITEM FLAG",
            Font = new Font("Arial", 10, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#E2E8F0"),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        resultPanel.Controls.Add(resultLabel, 0, 0);

        totalSumLabel = new Label
        {
            Text = "0",
            Font = new Font("Arial", 18, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#63B3ED"), // Light Blue
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        resultPanel.Controls.Add(totalSumLabel, 0, 1);

        var copyButton = new Button
        {
            Text = "Copy",
            BackColor = ColorTranslator.FromHtml("#4299E1"),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 },
            Anchor = AnchorStyles.None,
            Width = 100,
            Height = 30,
            Margin = new Padding(0, 10, 0, 0)
        };
        resultPanel.Controls.Add(copyButton, 0, 2);

        copyButton.Click += (sender, e) =>
        {
            try
            {
                Clipboard.SetText(totalSumLabel.Text);
                MessageBox.Show("Copied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Could not copy to clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        // Populate the checkboxes
        RenderHexList();
    }

    private void RenderHexList()
    {
        hexListPanel.Controls.Clear();
        foreach (var item in hexNumbers)
        {
            var checkBox = new CheckBox
            {
                Text = item.FlagName,
                ForeColor = ColorTranslator.FromHtml("#E2E8F0"),
                Tag = item.HexValue, // Store the hex value in the Tag property
                AutoSize = true,
                FlatStyle = FlatStyle.Standard
            };
            checkBox.CheckedChanged += CheckBox_CheckedChanged;
            hexListPanel.Controls.Add(checkBox);
        }
    }

    private void CheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateTotalSum();
    }

    private void UpdateTotalSum()
    {
        int totalDecimal = 0;
        foreach (var control in hexListPanel.Controls)
        {
            if (control is CheckBox checkBox && checkBox.Checked)
            {
                if (checkBox.Tag is int tagValue)
                {
                    totalDecimal += tagValue;
                }
            }
        }
        totalSumLabel.Text = totalDecimal.ToString();
    }
}
