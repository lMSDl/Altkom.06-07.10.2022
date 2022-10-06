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

            if(Items.Count() >= Size)
                return false;

            if (Items.Contains(name))
            {
                name = name + (Items.Count(x => x.StartsWith(name)) + 1);
            }

            Items.Add(name);
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
    }
}
