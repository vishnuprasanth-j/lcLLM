﻿using EnvDTE;
using EnvDTE80;
using lcLLM.Helpers;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TextSelection = EnvDTE.TextSelection;

namespace lcLLM.Windows
{
    /// <summary>
    /// Interaction logic for RefactorSuggestionWindowControl.xaml
    /// </summary>
    public partial class RefactorSuggestionWindowControl : UserControl
    {
        private string _activeDocumentPath;

        public RefactorSuggestionWindowControl()
        {
            InitializeComponent();
        }

        public void DisplaySuggestion(string suggestion, string activeDocumentPath)
        {
            SuggestionBox.Text = suggestion;
            _activeDocumentPath = activeDocumentPath;
        }

        private void ReplaceSelectedTextInIDE(string newText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (Package.GetGlobalService(typeof(DTE)) is not DTE dte)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_activeDocumentPath))
            {
                var document = dte.Documents?.Item(_activeDocumentPath);
                document?.Activate();

                var textSelection = document?.Selection as TextSelection;
                textSelection?.Insert(newText, (int)vsInsertFlags.vsInsertFlagsContainNewText);
            }
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            ReplaceSelectedTextInIDE(SuggestionBox.Text);
            BtnClose_Click(sender, e);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = System.Windows.Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            var window = new System.Windows.Window
            {
                Title = "About EntwineLLM",
                Background = Brushes.Black,
                Content = aboutWindow,
                Width = 500,
                Height = 330,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _ = SaveGeneratedCodeAsync(SuggestionBox.Text);
        }

        private static string GenerateFileName(string baseName)
        {
            return $"{baseName}_{DateTime.Now:yyyyMMddHHmmss}.cs";
        }

        private static void SaveCodeToFile(string code, string filePath)
        {
            File.WriteAllText(filePath, code);
        }

        private static async Task SaveGeneratedCodeAsync(string code)
        {
            const string baseName = "SuggestedCode";
            var fileName = GenerateFileName(baseName);
            var projectDirectory = GetActiveProjectDirectory();
            var filePath = Path.Combine(projectDirectory, fileName);

            SaveCodeToFile(code, filePath);

            ProjectHelper.AddFileToSolution(filePath);

            await Task.Yield();
        }

        private static string GetActiveProjectDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            var project = ProjectHelper.GetActiveProject(dte);

            return project == null ?
                throw new InvalidOperationException("No active project found.")
                : Path.GetDirectoryName(project.FullName);
        }

        private async void BtnFollowUp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FollowupBox.Text))
            {
                WindowHelper.WarningBox("A follow-up prompt must be set to proceed");
                return;
            }
            string prompt = FollowupBox.Text;

            await RefactoringHelper.RunManualPromptAsync(prompt);
            FollowupBox.Text = "";
        }

        private void FollowupBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter)
            {
                return;
            }

            BtnFollowUp_Click(sender, e);
        }
    }
}
