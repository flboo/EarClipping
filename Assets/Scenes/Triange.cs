using System;
using System.Collections.Generic;
using UnityEngine;

namespace PolygonTool
{


    public enum CompareAxle
    {
        X,
        Y,
        Z
    }


    public class Triange
    {
        private CompareAxle _compareAxle = CompareAxle.Y;

        private List<Vector3> _polygonVertexs = new List<Vector3>();

        private List<int> _vertexsSequence = new List<int>();

        private NodeManager _nodeManager = new NodeManager();

        public Triange(List<Vector3> polygonVertexs)
        {
            this._polygonVertexs = polygonVertexs;
            _nodeManager.Init(polygonVertexs);
        }

        public void SetCompareAxle(CompareAxle compareAxle)
        {
            this._compareAxle = compareAxle;
        }

        public int[] GetTriangles()
        {
            while (_nodeManager.LinkedListLength >= 3)
            {
                SplitResult sr = SplitPolygon();
                //
                if (sr == null)
                {
                    Debug.Log("null");
                    return null;
                }
            }

            return _vertexsSequence.ToArray();
        }

        /// <summary>
        /// 计算凹顶点，凸顶点，耳朵
        /// </summary>
        private SplitResult SplitPolygon()
        {
            //凹
            List<Node> _concaveVertexs = new List<Node>();
            //凸
            List<Node> _raisedVertexs = new List<Node>();
            //耳
            List<Node> _polygonEars = new List<Node>();
            //起始节点
            Node currentNode = _nodeManager.FirstNode;

            for (int i = 0; i < _nodeManager.LinkedListLength; i++)
            {
                Vector3 one = currentNode.vertex - currentNode.lastNode.vertex;
                Vector3 two = currentNode.nextNode.vertex - currentNode.vertex;
                Vector3 crossRes = Vector3.Cross(one, two);

                if (_compareAxle == CompareAxle.Y)
                {
                    if (crossRes.y > 0)
                        _concaveVertexs.Add(currentNode);
                    else
                        _raisedVertexs.Add(currentNode);
                }

                if (_compareAxle == CompareAxle.X)
                {
                    if (crossRes.x > 0)
                        _concaveVertexs.Add(currentNode);
                    else
                        _raisedVertexs.Add(currentNode);
                }

                if (_compareAxle == CompareAxle.Z)
                {
                    if (crossRes.z > 0)
                        _concaveVertexs.Add(currentNode);
                    else
                        _raisedVertexs.Add(currentNode);
                }

                _polygonEars.Add(currentNode);
                currentNode = currentNode.nextNode;
            }

            for (int i = 0; i < _concaveVertexs.Count; i++)
            {
                _polygonEars.Remove(_concaveVertexs[i]);
            }

            List<int> needRemoveIdList = new List<int>();
            for (int i = 0; i < _polygonEars.Count; i++)
            {
                Node earNode = _polygonEars[i];
                Node compareNode = earNode.nextNode.nextNode;

                while (compareNode != earNode.lastNode)
                {
                    bool isIn = IsInTriange(compareNode.vertex, earNode.lastNode.vertex, earNode.vertex,
                        earNode.nextNode.vertex);

                    if (isIn == true)
                    {
                        if (_polygonEars.Contains(_polygonEars[i]))
                        {
                            needRemoveIdList.Add(_polygonEars[i].id);
                        }
                        break;
                    }
                    compareNode = compareNode.nextNode;
                }
            }

            for (int j = 0; j < needRemoveIdList.Count; j++)
            {
                for (int i = 0; i < _polygonEars.Count; i++)
                {
                    if (_polygonEars[i].id == needRemoveIdList[j])
                    {
                        _polygonEars.RemoveAt(i);
                    }
                }
            }

            Debug.Log("凸点");
            for (int i = 0; i < _raisedVertexs.Count; i++)
            {
                Debug.Log(_raisedVertexs[i].id);
            }

            Debug.Log("凹点");
            for (int i = 0; i < _concaveVertexs.Count; i++)
            {
                Debug.Log(_concaveVertexs[i].id);
            }

            Debug.Log("耳朵");
            for (int i = 0; i < _polygonEars.Count; i++)
            {
                Debug.Log(_polygonEars[i].id);
            }

            if (_polygonEars.Count == 0)
            {
                return null;
            }

            _vertexsSequence.Add(_polygonEars[0].lastNode.id);
            _vertexsSequence.Add(_polygonEars[0].id);
            _vertexsSequence.Add(_polygonEars[0].nextNode.id);
            _nodeManager.RemoveNode(_polygonEars[0]);

            return new SplitResult(_raisedVertexs, _concaveVertexs, _polygonEars);
        }

        public bool IsInTriange(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
            if (p == a || p == b || p == c)
            {
                return false;
            }
            Vector3 pa = p - a;
            Vector3 pb = p - b;
            Vector3 pc = p - c;

            Vector3 t1 = Vector3.Cross(pa, pb);
            Vector3 t2 = Vector3.Cross(pb, pc);
            Vector3 t3 = Vector3.Cross(pc, pa);

            return t1.y >= 0 && t2.y >= 0 && t3.y >= 0 || t1.y <= 0 && t2.y <= 0 && t3.y <= 0;
        }

        public class NodeManager
        {
            private List<Node> _nodeList = new List<Node>();

            public int LinkedListLength
            {
                get { return _nodeList.Count; }
            }

            public Node FirstNode
            {
                get { return _nodeList[0]; }
            }

            public void Init(List<Vector3> vertexs)
            {
                for (int i = 0; i < vertexs.Count; i++)
                {
                    Node node = new Node(i, vertexs[i]);
                    _nodeList.Add(node);
                }

                for (int i = 0; i < LinkedListLength; i++)
                {
                    if (i == 0)
                    {
                        _nodeList[i].lastNode = _nodeList[LinkedListLength - 1];
                        _nodeList[i].nextNode = _nodeList[1];
                    }
                    else if (i == LinkedListLength - 1)
                    {
                        _nodeList[i].lastNode = _nodeList[LinkedListLength - 2];
                        _nodeList[i].nextNode = _nodeList[0];
                    }
                    else
                    {
                        _nodeList[i].lastNode = _nodeList[i - 1];
                        _nodeList[i].nextNode = _nodeList[i + 1];
                    }
                }
            }

            public void RemoveNode(Node node)
            {
                _nodeList.Remove(node);
                node.lastNode.nextNode = node.nextNode;
                node.nextNode.lastNode = node.lastNode;
            }
        }

        public class Node
        {
            public int id;
            public Vector3 vertex;
            public Node lastNode;

            public Node nextNode;

            public Node(int id, Vector3 vertex)
            {
                this.id = id;
                this.vertex = vertex;
            }

        }

        public class SplitResult
        {
            public List<Node> tuVert;

            public List<Node> aoVert;

            public List<Node> ears;

            public SplitResult(List<Node> raisedVertexs, List<Node> concaveVertexs, List<Node> polygonEars)
            {
                this.tuVert = raisedVertexs;
                this.aoVert = concaveVertexs;
                this.ears = polygonEars;
            }

        }
    }
}