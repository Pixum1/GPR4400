using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private int maxObjects = 10;
    private int maxSubdivision = 10;

    private int level;
    public Box box;
    private List<Boid> objects;
    public Octree[] nodes;

    public Octree(int _level, Box _newBox)
    {
        this.level = _level;
        this.box = _newBox;
        this.objects = new List<Boid>();
        this.nodes = new Octree[8];
    }

    public void Clear()
    {
        objects.Clear();

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].Clear();
                nodes[i] = null;
            }
        }
    }

    private void Split()
    {
        Vector3 subBoxSize = box.Size / 2f;

        nodes[0] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x - box.Size.x / 4, box.Center.y - box.Size.y / 4, box.Center.z - box.Size.z / 4)));
        nodes[1] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x + box.Size.x / 4, box.Center.y - box.Size.y / 4, box.Center.z - box.Size.z / 4)));
        nodes[2] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x + box.Size.x / 4, box.Center.y - box.Size.y / 4, box.Center.z + box.Size.z / 4)));
        nodes[3] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x - box.Size.x / 4, box.Center.y - box.Size.y / 4, box.Center.z + box.Size.z / 4)));
        nodes[4] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x - box.Size.x / 4, box.Center.y + box.Size.y / 4, box.Center.z - box.Size.z / 4)));
        nodes[5] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x + box.Size.x / 4, box.Center.y + box.Size.y / 4, box.Center.z - box.Size.z / 4)));
        nodes[6] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x + box.Size.x / 4, box.Center.y + box.Size.y / 4, box.Center.z + box.Size.z / 4)));
        nodes[7] = new Octree(level + 1, new Box(subBoxSize.x, subBoxSize.y, subBoxSize.z, new Vector3(box.Center.x - box.Size.x / 4, box.Center.y + box.Size.y / 4, box.Center.z + box.Size.z / 4)));

    }


    public void Insert(Boid _boid)
    {
        //--if node is not a leaf node
        if (nodes[0] != null)
        {
            int index = box.GetQuadrant(_boid.transform.position); //<- Convert boid position into index of quadtree node

            //-- if boid can be put into one specific node
            if (index != -1)
            {
                nodes[index].Insert(_boid); //<- call function until leaf node is reached

                return;
            }
        }

        objects.Add(_boid); //<- add boid to the List of Objects

        //-- if the maximum object count has been reached and the node can be split
        if (objects.Count > maxObjects && level < maxSubdivision)
        {
            //-- if there are no child nodes
            if (nodes[0] == null)
                Split();

            for (int i = 0; i < objects.Count; i++)
            {
                int index = box.GetQuadrant(objects[i].transform.position); //<- Convert boid position into index of quadtree node
                //-- if object can be put into one specific node
                if (index != -1)
                {
                    nodes[index].Insert(objects[i]); //<- insert into child node
                }
            }
            objects.Clear();
        }
    }

    public List<Boid> Retrieve(List<Boid> retrievedObjects, Boid _boid)
    {
        int index = box.GetQuadrant(_boid.transform.position); //<- Find Position of given Boid

        //-- Retrieve all objects of all subnodes containing this Boid
        if (index != -1 && nodes[0] != null)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].Retrieve(retrievedObjects, _boid);
            }
        }

        retrievedObjects.AddRange(objects); //<- Add all objects of this node to a list

        return retrievedObjects;
    }


    public void Show()
    {
        Color c = new Color(0, 0, 0, 0);
        if (objects.Count > 0)
            c = new Color(0, 1, 0, ((objects.Count / 100f) * maxObjects) - .25f);

        if (level != 0)
            box.Show(c);

        if (nodes[0] != null)
            for (int i = 0; i < nodes.Length; i++)
                nodes[i].Show();
    }
    public void ShowOutlines()
    {
        Color c = Color.red;

        box.Show(c);
    }
}