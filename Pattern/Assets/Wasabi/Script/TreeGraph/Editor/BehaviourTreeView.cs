using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Wasabi.TreeGraph;
using System;
using System.Linq;
using Node = Wasabi.TreeGraph.Node;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;

    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits>{}

    BehaviourTree tree;
    public BehaviourTreeView()
    { 
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Wasabi/Script/TreeGraph/Editor/Data/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;


        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;


        if(tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        //Create Node
        tree.nodes.ForEach(n => CreateNodeView(n));

        //create Edge
        tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });

    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem => { 
                NodeView nodeView = elem as NodeView;
                if(nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if(graphViewChange.edgesToCreate!= null)
        {
            graphViewChange.edgesToCreate.ForEach(edge => { 
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node,childView.node);
            });
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach(node =>
            {
                NodeView view = node as NodeView;
                view.SortChildren();
            });
        }
        return graphViewChange;
    }

    string GetTypeBasePath(Type type,Type startType)
    {
        string basePath = type.Name;
        
        while(type.BaseType != startType)
        {
            type = type.BaseType;
            basePath = type.Name+"/"+basePath;
        }
        return startType.Name+"/"+basePath;
    }

    Vector2 createPosition;
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        
        createPosition = (Vector2)viewTransform.position * -1 ;
        createPosition += evt.localMousePosition;
        createPosition /= viewTransform.scale.x;
        
        var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
        foreach(var type in types)
        {
            evt.menu.AppendAction($"ActionNode/{type.Name}", (a)=>CreateNode(type));
        }

        types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        foreach (var type in types)
        {
            //evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", (a) => CreateNode(type));
            //evt.menu.AppendAction($"CompositeNode/{type.Name}", (a) => CreateNode(type));
            evt.menu.AppendAction($"{GetTypeBasePath(type,typeof(CompositeNode))}", (a) => CreateNode(type));

        }
        types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        foreach (var type in types)
        {
            
            evt.menu.AppendAction($"DecoratorNode/{type.Name}", (a) => CreateNode(type));
        }
    }
    void CreateNode(System.Type type)
    {
        Wasabi.TreeGraph.Node node = tree.CreateNode(type);
        node.position = createPosition;
        
        CreateNodeView(node);
    }
    void CreateNodeView(Wasabi.TreeGraph.Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        
        AddElement(nodeView);
    }
    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
}
