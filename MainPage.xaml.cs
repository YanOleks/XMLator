using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using XMLator.Parsers;
using XMLator.Transformers;
using Microsoft.Maui.Storage;

namespace XMLator
{
    public partial class MainPage : ContentPage
    {
        private string selectedXslFilePath;
        FilePickerFileType customFileType = new(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".xml", ".XML" } },
        });
        private Dictionary<string, List<string>> _attributes = new();
        private IParser _parser;
        private Stream _currentStream;

        public MainPage()
        {
            InitializeComponent();
            _parser = new SAXParser();
        }

        private async void OnSelectFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Виберіть XML файл",
                FileTypes = customFileType
            });

            if (result != null)
            {
                _currentStream = await result.OpenReadAsync();
                LoadAttributes(_currentStream);
            }
        }
        private void LoadAttributes(Stream xmlStream)
        {
            var method = MethodPicker.SelectedItem?.ToString() ?? "SAX";

            _parser = method switch
            {
                "DOM" => new DOMParser(),
                "LINQ" => new LINQParser(),
                _ => new SAXParser()
            };

            _attributes = _parser.GetAllAttributes(xmlStream);

            AttributesStack.Children.Clear();

            foreach (var attribute in _attributes)
            {
                var label = new Label { Text = attribute.Key, FontSize = 16 };
                var picker = new Picker();

                picker.Items.Add("Не вибрано");
                foreach (var value in attribute.Value)
                {
                    picker.Items.Add(value);
                }

                var stack = new StackLayout();
                stack.Children.Add(label);
                stack.Children.Add(picker);
                AttributesStack.Children.Add(stack);
            }
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            var searchQuery = new Dictionary<string, string>();
            foreach (StackLayout stack in AttributesStack.Children)
            {
                var picker = stack.Children[1] as Picker;
                if (picker != null && picker.SelectedItem != null && picker.SelectedItem.ToString() != "Не вибрано")
                {
                    var attributeName = (stack.Children[0] as Label)?.Text;
                    if (!string.IsNullOrWhiteSpace(attributeName))
                    {
                        searchQuery[attributeName] = picker.SelectedItem.ToString();
                    }
                }
            }

            if (searchQuery.Count > 0 && _currentStream != null)
            {
                _currentStream.Position = 0;

                var result = _parser.Search(_currentStream, searchQuery);
                SearchResults.Text = string.Join("\n", result);
            }
            else
            {
                SearchResults.Text = "Будь ласка, виберіть атрибути для пошуку.";
            }
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            AttributesStack.Children.Clear();
            SearchResults.Text = string.Empty;
            _currentStream?.Dispose(); // Якщо потік відкритий, закриваємо його
            _currentStream = null;
        }
        private async void OnSelectXslFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Виберіть XSL файл",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
                    { DevicePlatform.WinUI, new[] { ".xsl", ".XSL" } }
                })
            });

            if (result != null)
            {
                selectedXslFilePath = result.FullPath;
                await DisplayAlert("Файл вибрано", $"Файл {result.FileName} вибрано.", "OK");
            }
            else
            {
                await DisplayAlert("Помилка", "Не вдалося вибрати файл", "OK");
            }
        }
        private async void OnTransformToHtmlClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedXslFilePath))
            {
                await DisplayAlert("Помилка", "Будь ласка, виберіть XSL файл", "OK");
                return;
            }

            string outputFilePath = "D:\\Projects\\Univer\\OOP\\test.html";

            var transformer = new XmlToHtmlTransformer(selectedXslFilePath);

            _currentStream.Position = 0;
            using var stream = _currentStream; 
            transformer.Transform(stream, outputFilePath);

            await DisplayAlert("Трансформація", $"Файл успішно трансформовано в HTML! Збережено за адресою: {outputFilePath}", "OK");
        }
    }
}
