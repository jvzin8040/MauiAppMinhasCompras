using MauiAppMinhasCompras.Models; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls; 

namespace MauiAppMinhasCompras.Views
{
    public partial class RelatorioGastosPorCategoria : ContentPage
    {
        public RelatorioGastosPorCategoria()
        {
            InitializeComponent();
            CarregarCategorias(); // chama o metodo CarregarCategorias
        }

        private void CarregarCategorias()
        {
            var categorias = new List<string> { "Todos", "Alimentos", "Higiene", "Limpeza" }; 
            // cria uma lista de categorias
            CategoriaPicker.ItemsSource = categorias; 
            // adiciona a lista de categorias ao CategoriaPicker
        }

        private async void OnGerarRelatorioClicked(object sender, EventArgs e)
        {
            if (CategoriaPicker.SelectedItem == null) // se o categoriaPicker não tiver nada selecionado
            {
                await DisplayAlert("Atenção", "Por favor, selecione uma categoria.", "OK"); 
                return; 
            }

            string categoriaSelecionada = CategoriaPicker.SelectedItem.ToString(); // pega a categoria selecionada
            List<Produto> produtos; // cria uma lista produtos

            if (categoriaSelecionada == "Todos") // se a categoria selecionada for todos
            {
                produtos = await App.Db.GetAll(); // pega todos os produtos
            }
            else
            {
                produtos = await App.Db.SearchByCategory(categoriaSelecionada); 
                // pega os produtos pela categoria selecionada
            }

            double totalGasto = produtos.Sum(p => p.Preco * p.Quantidade); // soma o total gasto

            var relatorio = new List<RelatorioItem> // cria uma lista de relatorio
            {
                new RelatorioItem // adiciona um item ao relatorio
                {
                    NomeCategoria = categoriaSelecionada, // pega a categoria selecionada
                    TotalGasto = totalGasto.ToString("C") 
                    // pega o total gasto e converte para string com o formato de moeda
                }
            };

            RelatorioListView.ItemsSource = relatorio; // adiciona o relatorio ao RelatorioListView
        }
    }

    public class RelatorioItem // cria a classe RelatorioItem
    {
        public string NomeCategoria { get; set; } // cria a propriedade NomeCategoria
        public string TotalGasto { get; set; } // cria a propriedade TotalGasto
    }
}