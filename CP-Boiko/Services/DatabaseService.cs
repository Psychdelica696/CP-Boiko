using SQLite;
using CP_Boiko.Models;

namespace CP_Boiko.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "todos.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TodoItem>().Wait();
        }

        public Task<List<TodoItem>> GetItemsAsync() =>
            _database.Table<TodoItem>().ToListAsync();

        public Task<int> SaveItemAsync(TodoItem item) =>
            item.Id != 0 ? _database.UpdateAsync(item) : _database.InsertAsync(item);
    }
}