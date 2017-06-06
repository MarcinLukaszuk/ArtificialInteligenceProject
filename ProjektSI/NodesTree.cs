using System.Collections.Generic;
using System.Linq;

namespace ProjektSI
{


    public class Node
    {

        public Node father;
        public Pole poleField { get; set; }


        public Node(Node father)
        {
            this.father = father;
        }


    }
    public class NodesTree
    {

        LinkedList<Node> nodesTreeee;


        public NodesTree(Pole root)
        {
            nodesTreeee = new LinkedList<Node>();
            nodesTreeee.AddFirst(new Node(null) { poleField = root, father = null });
        }

        public void addChild(Pole fat, Pole child)
        {
            var listaojcow = nodesTreeee.Where(x => x.poleField == fat);
            if (listaojcow.Count() == 1)
            {
                Node ojciec = listaojcow.Single();
                nodesTreeee.AddLast(new Node(ojciec) { poleField = child });
            }

        }


        public List<Pole> ReturnWay(Pole father)
        {
            List<Pole> stos = new List<Pole>();

            Node lastElem = nodesTreeee.Where(x => x.poleField == father).Single();

            while (lastElem.father != null)
            {
                stos.Add(lastElem.poleField);
                lastElem = lastElem.father;
            }
            return stos;
        }
    }
}

