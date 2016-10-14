using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Quadtree.Tests
{
    [TestFixture]
    class QuadtreeBehavior
    {
        [Test]
        public void ShouldPushAnItemInTheQuadtreeWhenInBounds()
        {
            var quadtree = new Quadtree(100, 100);
            var item = new Item("one", 10, 10);

            quadtree.Push(item);

            Assert.That(quadtree.RootNode.ItemsCount, Is.EqualTo(1));
        }

        [Test]
        public void ShouldNotPushAnItemInTheQuadtreeWhenOutOfBounds()
        {
            var quadtree = new Quadtree(100, 100);
            var item = new Item("out", 150, 10);

            quadtree.Push(item);

            Assert.That(quadtree.RootNode.ItemsCount, Is.EqualTo(0));
        }

        [Test]
        public void ShouldNotPushTheSameItemTwice()
        {
            var quadtree = new Quadtree(100, 100);
            var item = new Item("out", 10, 10);

            quadtree.Push(item);
            quadtree.Push(item);

            Assert.That(quadtree.RootNode.ItemsCount, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSplitQuadtreeWhenReachingRootNodeWhenReachingCapacity()
        {
            var quadtree = new Quadtree(100, 100);

            quadtree.Push(new Item("sw1", 10, 10));
            quadtree.Push(new Item("sw2", 20, 30));
            quadtree.Push(new Item("nw1", 60, 10));
            quadtree.Push(new Item("ne1", 80, 70));
            quadtree.Push(new Item("se1", 10, 70));

            Assert.That(quadtree.RootNode.ChildNode("SW").ItemsCount, Is.EqualTo(2));
        }

        [Test]
        public void ShouldEmptyRootNodeWhenReachingCapacity()
        {
            var quadtree = new Quadtree(100, 100);

            quadtree.Push(new Item("sw1", 10, 10));
            quadtree.Push(new Item("sw2", 20, 30));
            quadtree.Push(new Item("nw1", 60, 10));
            quadtree.Push(new Item("ne1", 80, 70));
            quadtree.Push(new Item("se1", 10, 70));

            Assert.That(quadtree.RootNode.ItemsCount, Is.EqualTo(0));
        }

        [Test]
        [Category("a")]
        [Category("b")]
        public void ShouldSplitChildNodesWhenReachingCapacity()
        {
            var quadtree = new Quadtree(100, 100);

            quadtree.Push(new Item("ne1", 82, 84));
            quadtree.Push(new Item("ne2", 97, 90));
            quadtree.Push(new Item("ne3", 76, 98));
            quadtree.Push(new Item("ne4", 86, 78));
            quadtree.Push(new Item("ne5", 60, 83));

            Assert.That(quadtree.RootNode.ChildNode("NE").ChildNode("SW").ItemsCount, Is.EqualTo(0));
            Assert.That(quadtree.RootNode.ChildNode("NE").ChildNode("NW").ItemsCount, Is.EqualTo(1));
            Assert.That(quadtree.RootNode.ChildNode("NE").ChildNode("SE").ItemsCount, Is.EqualTo(0));
            Assert.That(quadtree.RootNode.ChildNode("NE").ChildNode("NE").ItemsCount, Is.EqualTo(4));
        }
    }
}
