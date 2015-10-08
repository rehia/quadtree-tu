using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadtree
{
    public class Quadtree
    {
        private Dictionary<string, Item> items;

        public Quadtree(int width, int height)
        {
            this.RootNode = new Node(0, 0, width, height);
            this.items = new Dictionary<string, Item>();
        }

        public Node RootNode { get; private set; }

        public void Push(Item item)
        {
            if (items.ContainsKey(item.Key)) 
            {
                return;
            }

            var isInQuadtree = this.RootNode.Push(item);

            if (isInQuadtree)
            {
                this.items.Add(item.Key, item);
            }
        }

        public override string ToString()
        {
            foreach (var key in this.items.Keys)
            {
                Console.WriteLine(this.items[key]);
            }
            return string.Format("R{0}-T({1})", this.RootNode, this.items.Count);
        }
    }

    public class Node
    {
        private enum Cardinal
        {
            NW, NE, SE, SW
        }

        private double x;
        private double y;
        private double width;
        private double height;
        private List<Item> items;
        private Dictionary<Cardinal, Node> childNodes;

        public Node(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.items = new List<Item>();
            this.childNodes = new Dictionary<Cardinal, Node>();
        }

        public IEnumerable<Item> Items { get; private set; }

        public int ItemsCount
        {
            get 
            {
                return this.items.Count;
            }
        }

        public bool Push(Item item) 
        {
            if (this.IsNotInBounds(item))
            {
                return false;
            }

            if (this.HasReachedCapacity())
            {
                this.PushToChildren(item);
            }
            else
            {
                this.items.Add(item);
            }
            return true;
        }

        private bool IsNotInBounds(Item item)
        {
            return item.X < this.x ||
                item.X >= this.x + width ||
                item.Y < this.y ||
                item.Y >= this.y + height;
        }

        private bool HasReachedCapacity()
        {
            return !this.IsLeaf() ||
                this.items.Count == 4;
        }

        private void PushToChildren(Item item)
        {
            if (this.IsLeaf())
            {
                this.SplitInLeaves();
            }

            this.childNodes[Cardinal.NW].Push(item);
            this.childNodes[Cardinal.NE].Push(item);
            this.childNodes[Cardinal.SE].Push(item);
            this.childNodes[Cardinal.SW].Push(item);
        }

        public bool IsLeaf()
        {
            return this.childNodes.Count == 0;
        }

        private void SplitInLeaves()
        {
            this.CreateLeaves();
            foreach (var item in this.items)
            {
                this.childNodes[Cardinal.NW].Push(item);
                this.childNodes[Cardinal.NE].Push(item);
                this.childNodes[Cardinal.SE].Push(item);
                this.childNodes[Cardinal.SW].Push(item);
            }
            this.items.Clear();
        }

        private void CreateLeaves()
        {
            var middleX = (this.x + this.width) / 2;
            var middleY = (this.y + this.height) / 2;
            var halfWidth = this.width / 2;
            var halfHeight = this.height / 2;
            this.childNodes.Add(
                Cardinal.NW,
                new Node(this.x, middleY, halfWidth, halfHeight));
            this.childNodes.Add(
                Cardinal.NE,
                new Node(middleX, middleY, halfWidth, halfHeight));
            this.childNodes.Add(
                Cardinal.SE,
                new Node(middleX, this.y, halfWidth, halfHeight));
            this.childNodes.Add(
                Cardinal.SW,
                new Node(this.x, this.y, halfWidth, halfHeight));
        }

        public Node ChildNode(string cardinal)
        {
            return this.childNodes[(Cardinal)Enum.Parse(typeof(Cardinal), cardinal)];
        }

        public override string ToString()
        {
            if (this.IsLeaf())
            {
                return string.Format("[{1} - {2}]({0})", this.items.Count, x, y);
            }
            return string.Format(
                "\r\n[{4} - {5}][NW{0}NE{1}SE{2}SW{3}]",
                this.ChildNode("NW"),
                this.ChildNode("NE"),
                this.ChildNode("SE"),
                this.ChildNode("SW"), x, y);
        }
    }

    public class Item
    {
        public string Key { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-[{1} - {2}]", Key, X, Y);
        }
    }
}
