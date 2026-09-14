namespace IndustrialAPS.Domain.Algorithms.Tree
{
    public class BOMTreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; } // Quantidade necessária deste item para o pai
        public List<BOMTreeNode> Children { get; set; } = new List<BOMTreeNode>();

        public BOMTreeNode(int id, string name, decimal quantity)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
        }

        // Adiciona um filho ao nó
        public void AddChild(BOMTreeNode child)
        {
            Children.Add(child);
        }

        // Percursos (opcionais, para demonstrar)
        public void PreOrder(Action<BOMTreeNode> action)
        {
            action(this);
            foreach (var child in Children)
            {
                child.PreOrder(action);
            }
        }

        public void PostOrder(Action<BOMTreeNode> action)
        {
            foreach (var child in Children)
            {
                child.PostOrder(action);
            }
            action(this);
        }
    }
}