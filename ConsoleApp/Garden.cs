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

        public Garden(int size)
        {
            Size = size;
            Items = new List<string>();
        }

        public bool Plant(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Roślina musi posiadać nazwę!", nameof(name));
            if (Items.Contains(name))
                throw new ArgumentException("Roślina już istnieje w ogrodzie", nameof(name));

            if(Items.Count() >= Size)
                return false;

            Items.Add(name);
            return true;
        }

        public ICollection<string> GetPalnts()
        {
            return Items.ToList();
        }
    }
}
