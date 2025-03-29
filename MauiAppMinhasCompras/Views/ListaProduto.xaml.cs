namespace MauiAppMinhasCompras.Views;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
// explicação do código abaixo:
public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        CarregarCategorias(); // inicializa o método CarregarCategorias
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try 
        {
            lista.Clear(); 
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try 
        {
            string q = e.NewTextValue;
            lst_produtos.IsRefreshing = true;
            lista.Clear();
            List<Produto> tmp = await App.Db.Search(q);
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try 
        { 
        double soma = lista.Sum(i => i.Total);
        string msg = $" O total é {soma:C}";
        DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e) 
    {

        try  
        {
            MenuItem selecionado = sender as MenuItem; 
            Produto p = selecionado.BindingContext as Produto; 
            bool confirm = await DisplayAlert("Confirmação",
                $"Deseja excluir o produto {p.Descricao}?", "Sim", "Não"); 
            if (confirm) 
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p); 
            }
        }

        catch (Exception ex)
        {
          await DisplayAlert("Ops", ex.Message, "OK");
        }


    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try 
        {
            Produto p = e.SelectedItem as Produto; 
            Navigation.PushAsync(new Views.EditarProduto 
            {
                BindingContext = p, 
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try 
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        } finally 
        {
            lst_produtos.IsRefreshing = false;
        }
    }



    private void CarregarCategorias() // método CarregarCategorias
    {
        var categorias = new List<string> // cria uma lista de categorias
    {
        "Todos",
        "Alimentos",
        "Higiene",
        "Limpeza"
    };
        pck_categoria_filtro.ItemsSource = categorias; // atribui a lista de categorias ao Picker
    }




    private async Task FiltrarProdutosPorCategoria(string categoria) // método FiltrarProdutosPorCategoria
    {
        lista.Clear(); // limpa a lista de produtos
        List<Produto> produtos; // cria uma lista produtos

        if (categoria == "Todos")   // se a categoria for "Todos"
        {
            produtos = await App.Db.GetAll(); // busca todos os produtos
        }
        else 
        {
            produtos = await App.Db.SearchByCategory(categoria); // busca produtos pela categoria
        }

        produtos.ForEach(i => lista.Add(i)); // adiciona os produtos na lista
    }

    private async void pck_categoria_SelectedIndexChanged(object sender, EventArgs e) 
    {
        string categoriaSelecionada = pck_categoria_filtro.SelectedItem.ToString(); // pega a categoria selecionada
        await FiltrarProdutosPorCategoria(categoriaSelecionada); // chama o método FiltrarProdutosPorCategoria
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioGastosPorCategoria());
        // navega para a página RelatorioGastosPorCategoria
    }
}
