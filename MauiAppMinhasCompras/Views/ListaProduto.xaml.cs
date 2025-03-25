namespace MauiAppMinhasCompras.Views;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try // manteve o código da agenda anterior e foi adicionado um try catch
        {
            lista.Clear(); // limpa a lista quando a pagina recarregar

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
        try // manteve codigo anterior e foi adicionado um try catch para tratar exceções
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
        try { 

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

        try  // metodo try catch para tratar exceções
        {
            MenuItem selecionado = sender as MenuItem; // sempre que um item for clicado no menu, ele vai ser guardado na variável selecionado

            Produto p = selecionado.BindingContext as Produto; // como vai chegar um BindingContext, ele vai ser guardado na variável Produto p

            bool confirm = await DisplayAlert("Confirmação",
                $"Deseja excluir o produto {p.Descricao}?", "Sim", "Não"); // pergunta para o usuario se ele deseja excluir o produto

            if (confirm) // se o usuario clicar em sim, o produto vai ser excluido
            {
                await App.Db.Delete(p.Id); // o produto vai ser excluido do banco de dados
                lista.Remove(p); // o produto vai ser removido da lista
            }
        }

        catch (Exception ex)
        {
          await DisplayAlert("Ops", ex.Message, "OK");
        }


    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try // metodo try catch para tratar exceções
        {
            Produto p = e.SelectedItem as Produto; // gaurda o produto selecionado na variável p

            Navigation.PushAsync(new Views.EditarProduto // o item selecionado vai ser enviado para a página de edição
            {
                BindingContext = p, // guarda o produto selecionado na variável BindingContext
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try // manteve o código da agenda anterior e foi adicionado um try catch
        {
            lista.Clear(); // limpa a lista quando a pagina recarregar

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
}
