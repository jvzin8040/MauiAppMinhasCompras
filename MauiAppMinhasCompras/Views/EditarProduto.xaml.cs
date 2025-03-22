using MauiAppMinhasCompras.Models; // using models adicionada no arquivo
namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }
    private async void ToolbarItem_Clicked(object sender, EventArgs e) 
    {
        try // metodo try catch para tratar exceções
        {
            Produto  produto_anexado = BindingContext as Produto; /* como vai chegar um BindingContext,
                                                                  ele vai ser guardado na classe Produto como um produto anexado */

            Produto p = new Produto // um novo produto vai ser criado, porem vamos configurar para ele atualizar um produto existente
            {
                Id = produto_anexado.Id, // o id do produto vai ser o mesmo do produto anexado
                Descricao = txt_descricao.Text, // a descrição do produto vai ser a mesma do txt_descricao
                Quantidade = Convert.ToDouble(txt_quantidade.Text), // a quantidade do produto vai ser a mesma do txt_quantidade
                Preco = Convert.ToDouble(txt_preco.Text) // o preço do produto vai ser o mesmo do txt_preco
            };

            await App.Db.Update(p); // o produto vai ser atualizado no banco de dados
            await DisplayAlert("Sucesso", "Produto atualizado com sucesso", "OK"); // uma mensagem de sucesso vai ser exibida
            await Navigation.PopAsync(); // a página vai ser fechada e retornar para a página anterior
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}

