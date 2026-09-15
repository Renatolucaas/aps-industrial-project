using IndustrialAPS.Domain.Algorithms.Tree;
using IndustrialAPS.Domain.Entities;

namespace IndustrialAPS.Domain.Services
{
    public class BOMService
    {
        // Monta a árvore BOM a partir de um produto e uma lista de BOMItems
        public BOMTree BuildBOMTree(Product product, List<BOMItem> allBomItems, List<Material> allMaterials)
        {
            var root = new BOMTreeNode(product.Id, product.Name, 1);
            BuildChildren(root, product.Id, allBomItems, allMaterials);
            return new BOMTree(root);
        }

        private void BuildChildren(BOMTreeNode parentNode, int productId, List<BOMItem> allBomItems, List<Material> allMaterials)
        {
            // Pega todos os itens de BOM que pertencem a este produto
            var bomItems = allBomItems.Where(b => b.ProductId == productId).ToList();

            foreach (var item in bomItems)
            {
                // Tenta encontrar o material correspondente
                var material = allMaterials.FirstOrDefault(m => m.Id == item.ComponentId);

                if (material != null)
                {
                    // É um material folha
                    var childNode = new BOMTreeNode(material.Id, material.Name, item.Quantity);
                    parentNode.AddChild(childNode);
                }
                // Se não for material, pode ser um subproduto (não implementado nesta versão simplificada)
            }
        }

        // Calcula os requisitos de material para uma quantidade de produção
        public Dictionary<string, decimal> CalculateMaterialRequirements(BOMTree tree, decimal productionQuantity)
        {
            return tree.CalculateMaterialRequirements(productionQuantity);
        }
    }
}