using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Utils {

    public class PolymorphicGraph<T> 
    {
        private Dictionary<ulong, List<ulong>> _AdjacencyList { get; set; }
        private List<Node> _Nodes { get; set; }

        private class Node
        {
            public T Element {get; set;}
            public ulong Id {get; set;}

            public Node(ulong id, T elem)
            {
                Id = id;
                Element = elem;
            }            
        }

        public PolymorphicGraph()
        {
            this._AdjacencyList = new Dictionary<ulong, List<ulong>>();
            this._Nodes = new List<Node>();
        }   

        public ulong Add(T elem)
        {
            Node n = new Node((ulong)this._Nodes.Count, elem);
            this._AdjacencyList.Add(n.Id, new List<ulong>());
            this._Nodes.Add(n);

            return n.Id;
        }

        public void AddRelation(ulong xId, ulong yId)
        {
            this._AdjacencyList[xId].Add(yId);
            this._AdjacencyList[yId].Add(xId);
        }

        public override string ToString()
        {
            string ret = "";

            foreach(Node n in this._Nodes)
            {
                string adjString = string.Join(",", this._AdjacencyList[n.Id].ToArray());
                ret += $"{n.Id} -> {adjString}\n";
            }
            return ret;
        }

        public void ConfigureBalancedBinaryTree()
        {
            if (this._Nodes.Count == 0)
            {
                Console.WriteLine("Node List Empty");
                return;
            }

            ulong left(ulong n) {
                return n * 2 + 1;
            };

            ulong right(ulong n) {
                return n * 2 + 2;
            };
            /**
             * To configure this structure into a binary tree, we need to have a notion of left and right
             * since Ids are sequential we can use them to create the binary tree. We will use the adjacency
             * list to do this. Going in sequential order, starting from the root node we will perform the following
             * algorithm: 
             *  
             *  Add a relation between the root node and root node + 1, the left node,
             *  Add a relation between the root node + 2 and the right node,
             *  
             *  Add one to the current node that is being added relations to
             *  
             * Add a relation between the current node and current node + 1...
             */
            
            var al = new Dictionary<ulong, List<ulong>>();
            al[0] = new List<ulong>();
            for(ulong rootId = 0; rootId < (ulong)this._Nodes.Count; rootId++)
            {
                ulong leftId = left(rootId);
                ulong rightId = right(rootId);

                if(leftId < (ulong)this._Nodes.Count)
                {
                    al[leftId] = new List<ulong>();
                    al[rootId].Add(leftId);
                    al[leftId].Add(rootId);

                }
                if(rightId < (ulong)this._Nodes.Count)
                {
                    al[rightId] = new List<ulong>();
                    al[rootId].Add(rightId);
                    al[rightId].Add(rootId);
                }             
            }
            this._AdjacencyList = al;
        }
    }
}