namespace IndustrialAPS.Domain.Algorithms.Tree
{
    public class BOMTree
    {
        public BOMTreeNode Root { get; private set; }

        public BOMTree(BOMTreeNode root)
        {
            Root = root;
        }

        // Calcula a quantidade total de cada material necessário
        // para produzir uma certa quantidade do produto raiz.
        public Dictionary<string, decimal> CalculateMaterialRequirements(decimal productionQuantity)
        {
            var requirements = new Dictionary<string, decimal>();
            TraverseAndCalculate(Root, productionQuantity, requirements);
            return requirements;
        }

        private void TraverseAndCalculate(BOMTreeNode node, decimal parentQuantity, Dictionary<string, decimal> requirements)
        {
            // A quantidade real deste nó é a quantidade do pai multiplicada pela quantidade necessária por unidade
            decimal totalQuantity = node.Quantity * parentQuantity;

            // Se for uma folha (material), adiciona ao dicionário
            if (node.Children.Count == 0)
            {
                if (requirements.ContainsKey(node.Name))
                {
                    requirements[node.Name] += totalQuantity;
                }
                else
                {
                    requirements[node.Name] = totalQuantity;
                }
            }
            else
            {
                // Se não for folha, continua percorrendo os filhos
                foreach (var child in node.Children)
                {
                    TraverseAndCalculate(child, totalQuantity, requirements);
                }
            }
        }

        // Retorna a árvore em formato de texto (para debug)
        public string PrintTree()
        {
            var result = "";
            Root.PreOrder(node =>
            {
                result += $"{new string(' ', node.Children.Count * 2)}- {node.Name} (x{node.Quantity})\n";
            });
            return result;
        }
    }
}