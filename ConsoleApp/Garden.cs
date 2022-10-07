using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; }
        private ICollection<string> Items { get; }
        private ILogger? _logger;

        public Garden(int size, ILogger logger) : this(size)
        {
            _logger = logger;
        }

        public Garden(int size)
        {
            if(size < 0)
                throw new ArgumentOutOfRangeException("size");
            Size = size;
            Items = new List<string>();
        }

        public bool Plant(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Roślina musi posiadać nazwę!", nameof(name));

            if (Items.Count() >= Size)
            {
                _logger?.Log($"Brak miejsca w ogrodzie na {name}");
                return false;
            }

            if (Items.Contains(name))
            {
                var newName = name + (Items.Count(x => x.StartsWith(name)) + 1);
                _logger?.Log($"Roślina {name} zmieniła nazwę na {newName}");
                name = newName;
            }

            Items.Add(name);
            _logger?.Log($"Roślina {name} została dodana do ogrodu");
            return true;
        }

        public ICollection<string> GetPlants()
        {
            return Items.ToList();
        }

        public bool Remove(string name)
        {
            if(!Items.Contains(name))
                return false;

            Items.Remove(name);
            _logger?.Log($"Roślina {name} została usunięta z ogrodu");
            return true;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public int Count()
        {
            return Items.Count();
        }

        public string? ShowLastLog()
        {
            var log = _logger?.GetLogsAsync(DateTime.Now.AddHours(-1), DateTime.Now).Result;
            return log?.Split("\n").Last();
        }
    }
}
