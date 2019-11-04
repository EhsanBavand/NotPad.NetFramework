using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string clipboard;
        string FILE_NAME;
        bool isModified = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void miCopy_Click(object sender, RoutedEventArgs e)
        {
            clipboard = tbText.SelectedText;
            lblStatusBar.Text = "Highlighted text copied. ";
        }

        private void miPaste_Click(object sender, RoutedEventArgs e)
        {
            tbText.SelectedText = clipboard .Replace(Environment.NewLine, " ");
            lblStatusBar.Text = "Highlighted text pasted. ";
        }

        private void miClear_Click(object sender, RoutedEventArgs e)
        {
            tbText.SelectedText = "";
            lblStatusBar.Text = "All text cleared. ";
        }

        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            if (isModified)
            { 
                MessageBoxResult result = MessageBox.Show("Save before opening a new file?", "Save?",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch (result.ToString())
                {
                    case "No":
                        tbText.Text = "";
                        FILE_NAME = null;
                        lblStatusBar.Text = "Previous file closed without saving, new file opened. ";
                        break;
                    case "Yes":
                        if (FILE_NAME != null) Save();
                        if (FILE_NAME == null)
                        {
                            SaveAs();
                            if (FILE_NAME == null) return;
                        }
                        tbText.Text = "";
                        lblStatusBar.Text = "Successfully saved to " + FILE_NAME + " , new file opened.";
                        FILE_NAME = null;
                        break;
                    default :
                        return;
                }
            }
            else
            {
                tbText.Text = "";
                lblStatusBar.Text = "New file opened. ";
            }
        }        

        private void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "*"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Txt docs (.txt)|*.txt"; // Filter files by extension

            bool? result = dlg.ShowDialog();
            
            if (result == true)
            {
                try
                {
                    FILE_NAME = dlg.FileName;
                    File.WriteAllText(FILE_NAME, tbText.Text);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                lblStatusBar.Text = "Save cancelled. ";
            }
        }

        private void Save()
        {
            try
            {
                File.WriteAllText(FILE_NAME, tbText.Text);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            FILE_NAME = dlg.FileName;
            tbText.Text = File.ReadAllText(FILE_NAME);
            lblStatusBar.Text = "Successfully opened file " + FILE_NAME;
            isModified = false;
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            if (FILE_NAME != null && isModified)
            {
                Save();
                lblStatusBar.Text = "Changes successfully saved: " + FILE_NAME;
            }
            else if (FILE_NAME == null)
            {
                SaveAs();
                lblStatusBar.Text = "File successfully saved: " + FILE_NAME;
            }
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            if (isModified) this.Close();
            else
            {
                MessageBoxResult result = MessageBox.Show("Save before exiting?", "Save?",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch (result.ToString())
                {
                    case "No":
                        this.Close();
                        break;
                    case "Yes":
                        if (FILE_NAME != null) Save();
                        if (FILE_NAME == null) SaveAs();
                        break;
                    default:
                        return;
                }
            }
        }

        private void tbText_TextChanged(object sender, TextChangedEventArgs e)
        {
            isModified = true;
            if (FILE_NAME != null) lblStatusBar.Text = "Update changes before exiting. ";
            if (FILE_NAME == null) lblStatusBar.Text = "Save work before exiting. ";
        }
    } // MW
} // NS
