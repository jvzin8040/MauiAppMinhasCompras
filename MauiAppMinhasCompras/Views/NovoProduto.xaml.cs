using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
        BindingContext = new Produto(); // Adiciona o BindingContext
        pck_categoria.ItemsSource = new List<string> { "Alimentos", "Higiene", "Limpeza", "Moda" };
        // Adiciona as categorias no arquivo NovoProduto.xaml

    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{

			Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text),
			    Categoria = pck_categoria.SelectedItem.ToString() // Adiciona a categoria e converte para string

            };


			await App.Db.Insert(p);
			await DisplayAlert("Sucesso", "Produto inserido com sucesso", "OK");
			await Navigation.PopAsync();

        } catch (Exception ex)
		{
           await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    
}