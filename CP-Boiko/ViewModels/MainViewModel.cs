using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using CP_Boiko.Models;
using CP_Boiko.Services;

namespace CP_Boiko.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _currentDateTime = string.Empty;
        private string _userInput = string.Empty;
        private string _apiResult = string.Empty;
        private readonly DatabaseService _databaseService = new();

        public string CurrentDateTime
        {
            get => _currentDateTime;
            set { _currentDateTime = value; OnPropertyChanged(); }
        }

        public bool HasInput => !string.IsNullOrEmpty(_userInput);

        public bool HasApiResult => !string.IsNullOrEmpty(_apiResult);

        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasInput));
            }
        }

        public string ApiResult
        {
            get => _apiResult;
            set
            {
                _apiResult = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasApiResult));
            }
        }

        public string DeviceDetails =>
            $"Платформа: {DeviceInfo.Current.Platform}\n" +
            $"Модель: {DeviceInfo.Current.Model}\n" +
            $"Виробник: {DeviceInfo.Current.Manufacturer}\n" +
            $"Версія ОС: {DeviceInfo.Current.VersionString}\n" +
            $"Тип: {DeviceInfo.Current.Idiom}";

        public ICommand UpdateTimeCommand { get; }
        public ICommand FetchApiCommand { get; }

        public MainViewModel()
        {
            UpdateTimeCommand = new Command(UpdateTime);
            FetchApiCommand = new Command(async () => await FetchDataFromApiAsync());
            CurrentDateTime = DateTime.Now.ToString("F");
        }

        private void UpdateTime()
        {
            CurrentDateTime = DateTime.Now.ToString("F");
        }

        private async Task FetchDataFromApiAsync()
        {
            try
            {
                ApiResult = "Завантаження...";
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var todoItem = JsonSerializer.Deserialize<TodoItem>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (todoItem != null)
                    {
                        await _databaseService.SaveItemAsync(todoItem);
                        var items = await _databaseService.GetItemsAsync();
                        ApiResult = $"✅ Отримано з API:\n" +
                                    $"ID: {todoItem.Id}\n" +
                                    $"Заголовок: {todoItem.Title}\n" +
                                    $"Виконано: {todoItem.Completed}\n\n" +
                                    $"💾 Збережено в SQLite.\n" +
                                    $"Всього записів: {items.Count}";
                    }
                }
            }
            catch (Exception ex)
            {
                ApiResult = $"❌ Помилка: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}